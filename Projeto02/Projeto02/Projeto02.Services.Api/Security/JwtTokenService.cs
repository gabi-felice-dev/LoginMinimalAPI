using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projeto02.Services.Api.Security
{
    /// <summary>
    /// Classe para gerar os TOKENS JWT
    /// </summary>
    public class JwtTokenService
    {
        //atributo
        private readonly JwtTokenSettings _jwtTokenSettings;

        //construtor para injeção de dependência
        public JwtTokenService(JwtTokenSettings jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings;
        }

        public string Get(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }),
                Expires = DateTime.Now.AddHours(_jwtTokenSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(accessToken);
        }
    }
}
