using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Projeto02.Services.Api.Security
{
    /// <summary>
    /// Classe para configuração e injeção de dependência do JWT
    /// </summary>
    public static class JwtConfiguration
    {
        public static void AddJwtBearerConfiguration(WebApplicationBuilder builder)
        {
            //lendo o arquivo appsettings.json para obter os parametros de geração dos tokens
            var settingsSection = builder.Configuration.GetSection("JwtTokenSettings");
            builder.Services.Configure<JwtTokenSettings>(settingsSection);

            //capturando os parametros..
            var appSettings = settingsSection.Get<JwtTokenSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            //defindo o tipo de autenticação utilizado pelo projeto (JWT BEARER)
            builder.Services.AddAuthentication(
                auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    bearer =>
                    {
                        bearer.RequireHttpsMetadata = false;
                        bearer.SaveToken = true;
                        bearer.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    }
                );

            builder.Services.AddTransient(map => new JwtTokenService(appSettings));                
        }
    }
}
