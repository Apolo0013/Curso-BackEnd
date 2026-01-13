using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
//DTO
using BackEnd.DTO.Auth;
//Model
using BackEnd.Model.User;
//Service
using BackEnd.Service.Auth;
//Common
using BackEnd.Common.ErrorMSGAuth;
using BackEnd.Common.SucessoMSGAuth;
using System.Text.Json;


[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    //Service
    private readonly AuthService _service;
    //IConfig
    private readonly IConfiguration _config;
    public AuthController(AuthService service, IConfiguration config)
    {
        _service = service; // instancia do servico
        _config = config; // instancia do configuracao
    }

    //Rota para receber o login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        //buscando o usuario
        var result = await _service.CheckLoginUser(login);
        if (result.msg == ErrorMSGAuth.ERROR_USER_NOFIND) // se ele nao estive registrado.
            return BadRequest("Usuario nao entrado");
        //Configura o Jwt Token os cambal do cookie
        var user = await _service.GetUserIndex(result.index);
        string jwt = await JwtGerar(user);
        //Coloca no cookie
        await CookieAppend(jwt);
        return Ok("Logado com Sucesso");
    }


    //Registrar o usuario
    [HttpPost("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarDTO dados)
    {
        
        return Ok();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    [HttpGet("ADMIN")]
    public async Task<IActionResult> ADMIN()
    {
        string? ID = User.FindFirst("ID")!.Value;
        var user = await _service.GetUserID(ID);
        Console.WriteLine($"Nome: {user.Nome}");
        Console.WriteLine("SO ADM ACESSA AQUI");
        return Ok();
    }

    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "USER"
    )]
    [HttpGet("USER")]
    public async Task<IActionResult> USER()
    {
        string? ID = User.FindFirst("ID")!.Value;
        var user = await _service.GetUserID(ID);
        Console.WriteLine($"Nome: {user.Nome}");
        Console.WriteLine("SO USER e ADMIN ACESSA AQUI");
        return Ok();
    }


    public async Task<string> JwtGerar(UserModel? user)
    //Responsavel por criar o token.
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:secret"]!);
        var tokenhandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("ID" , user!.ID.ToString()),
                new Claim(ClaimTypes.Role, user!.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenhandler.CreateToken(tokenDescriptor);
        var jwt = tokenhandler.WriteToken(token);

        return jwt;
    }

    public async Task CookieAppend(string jwt)
    //Responsavel por coloca o Cookie
    {
        Response.Cookies.Append("AUTENTICACAO_TOKEN", jwt, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true, //_environment.IsProduction()
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(1)
        });
    }
}
