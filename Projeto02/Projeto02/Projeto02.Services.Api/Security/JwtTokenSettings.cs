namespace Projeto02.Services.Api.Security
{
    /// <summary>
    /// Classe para capturar os parametros de configuração do appsettings.json
    /// </summary>
    public class JwtTokenSettings
    {
        public string? SecretKey { get; set; }
        public int ExpirationInHours { get; set; }
    }
}
