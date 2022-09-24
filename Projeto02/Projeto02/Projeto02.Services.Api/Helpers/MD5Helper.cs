using System.Security.Cryptography;
using System.Text;

namespace Projeto02.Services.Api.Helpers
{
    /// <summary>
    /// Classe auxiliar para criptografia em MD5
    /// </summary>
    public class MD5Helper
    {
        public static string Get(string value)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value));
            var result = string.Empty;

            foreach (var item in hash)
                result += item.ToString("X2"); //X2 -> string hexadecimal
            
            return result;
        }
    }
}
