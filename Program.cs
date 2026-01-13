using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens; 
using System.Text;
//Service
using BackEnd.Service.Auth;
//Repositorie / Data
using BackEnd.Data.User;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

string key = builder.Configuration["Jwt:secret"]!;

//Registrar os meu service
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserRepo>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Auth
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.UseSecurityTokenValidators = true;
        options.TokenHandlers.Clear();
        options.TokenHandlers.Add(new JsonWebTokenHandler());

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var raw = context.Request.Cookies["AUTENTICACAO_TOKEN"];
                context.Token = raw;
                return Task.CompletedTask;
            }
            ,

        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("JWT INVALIDO:");
            Console.WriteLine(context.Exception.Message);
            return Task.CompletedTask;
        }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:secret"]!)
            )
        };
    });

builder.Services.AddAuthorization();

// CORS para dev
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("FrontPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5182")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();    

app.UseCors("FrontPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
