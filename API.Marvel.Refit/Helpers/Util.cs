using System;
using System.Security.Cryptography;
using System.Text;

namespace API.Marvel.Refit.Helpers
{
    /// <summary>
    /// Classe responsável pelos métodos comuns
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Gerador de hash
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="publicKey">Chave pública da API</param>
        /// <param name="privateKey">Chave privada da API</param>
        /// <returns>Hash</returns>
        public static string GerarHash(string ts, string publicKey, string privateKey)
        {
            var factoryMd5 = MD5.Create();

            var bytes = Encoding.UTF8.GetBytes($"{ts}{privateKey}{publicKey}");
            var bytesHash = factoryMd5.ComputeHash(bytes);
            
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", string.Empty);
        }
    }
}