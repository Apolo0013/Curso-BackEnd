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
using BackEnd.Model.Auth;
//Service
using BackEnd.Service.Auth;
//Common
using BackEnd.Common.ErrorMSGAuth;
using BackEnd.Common.SucessoMSGAuth;

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
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        //buscando o usuario
        var result = await _service.CheckLoginUser(login);
        if (result.msg == ErrorMSGAuth.ERROR_USER_NOFIND) // se ele nao estive registrado.
            return BadRequest(new ReturnAuthModel()
            {
                Code = ErrorMSGAuth.ERROR_USER_NOFIND,
                Sucesso = false
            });
        //Configura o Jwt Token os cambal do cookie
        var user = await _service.GetUserIndex(result.index);
        //string jwt = await JwtGerar(user);
        //Coloca no cookie
        //await CookieAppend(jwt);
        return Ok(new ReturnAuthModel()
        {
            Code = SucessoMSGAuth.CONTA_AUTENTICADA_LOGIN,
            Sucesso = true
        });
    }

    //Registrar o usuario
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarDTO dados)
    {
        var result = await _service.AuthRegister(dados);
        if (result.msg == ErrorMSGAuth.ERROR_EMAIL_EM_USO)
            return BadRequest(new ReturnAuthModel
            {
                Code = ErrorMSGAuth.ERROR_EMAIL_EM_USO,
                Sucesso = false
            });
        else if (result.msg == ErrorMSGAuth.ERROR_NOME_EM_USO)
            return BadRequest(new ReturnAuthModel()
            {
                Code = ErrorMSGAuth.ERROR_NOME_EM_USO,
                Sucesso = false
            });
        else // se ele chegou aqui, pq foi registrado...
        {
            // pegando o usuario    
            var user = await _service.GetUserID(result.id);
            if (user == null)
                return BadRequest(new ReturnAuthModel
                {
                    Code = ErrorMSGAuth.ERROR_INTERNO_SISTEMA,
                    Sucesso = false
                });
            // !Garantia que vai registrar e estara logado
            // add ele no cookie e jwt
            //string jwt = await JwtGerar(user); // gerar
            //await CookieAppend(jwt); // e salvar no token
            return Ok(new ReturnAuthModel()
            {
                Code = SucessoMSGAuth.CONTA_REGISTRADA,
                Sucesso = true
            });
        }
    }

    [Authorize]
    [HttpGet("Me")]
    public async Task<IActionResult> GetMe()
    {
        //Pegando o ID dentro do cookie.
        string? id = User.FindFirst("ID")?.Value;
        if (id != null)
        {
            var user = await _service.GetUserID(id); //buscando o usuario no banco de dados
            if (user == null) // se for null ou seja, algo deu errado em busca o usuario. Algo que nao deveria acontece
                return BadRequest(new ReturnAuthModel()
                {
                    Code = ErrorMSGAuth.ERROR_INTERNO_SISTEMA,
                    Sucesso = false
                });
            else // senao deu certo vamos retorna o usuario normalmente
                return Ok(new TUser()
                {
                    ID = id,
                    Nome = user.Name,
                    Role = user.Role
                });
        }
        else return BadRequest(new ReturnAuthModel()
        {
            Code = ErrorMSGAuth.ERROR_INTERNO_SISTEMA,      
            Sucesso = false
        });
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
