using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using API.Marvel.Refit.Model;

namespace API.Marvel.Refit
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
        internal static string GerarHash(string ts, string publicKey, string privateKey)
        {
            var factoryMd5 = MD5.Create();

            var bytes = Encoding.UTF8.GetBytes($"{ts}{privateKey}{publicKey}");
            var bytesHash = factoryMd5.ComputeHash(bytes);
            
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", string.Empty);
        }

        /// <summary>
        /// Retorna o description do Enum
        /// </summary>
        /// <param name="value">Enum</param>
        /// <returns></returns>
        internal static string GetEnumDescription(Enum value)
        {
            var enumType = value.GetType();

            if (!Enum.IsDefined(enumType, value))
                return string.Empty;

            // get attributes  
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return description
            return attributes.Any() ? ((DescriptionAttribute)attributes.First()).Description : value.ToString();
        }

        /// <summary>
        /// Gera o menu no console
        /// </summary>
        /// <returns></returns>
        internal static PersonagensEnum Menu()
        {
            Console.WriteLine($"{new string('-', 70)}\r");
            Console.WriteLine("Digite o número correspondente ao personagem: \n");

            var personagens = new StringBuilder();
            personagens.Append($"{PersonagensEnum.CapitaoAmerica}: {(int)(PersonagensEnum.CapitaoAmerica)}\r\n");
            personagens.Append($"{PersonagensEnum.DoutorEstranho}: {(int)(PersonagensEnum.DoutorEstranho)}\r\n");
            personagens.Append($"{PersonagensEnum.IronMan}: {(int)(PersonagensEnum.IronMan)}\r\n");
            personagens.Append($"{PersonagensEnum.SpiderMan}: {(int)(PersonagensEnum.SpiderMan)}\r\n");
            personagens.Append($"{PersonagensEnum.Thor}: {(int)(PersonagensEnum.Thor)}\r\n");
            personagens.Append($"{PersonagensEnum.Sair}: {(int)(PersonagensEnum.Sair)}\r");

            Console.WriteLine(personagens);
            Console.WriteLine($"{new string('-', 70)}\r");
            Console.Write("Opção escolhida: ");

            var opcao = Console.ReadLine();
            Console.WriteLine($"Carregando informações do Personagem: {GetEnumDescription((PersonagensEnum) Convert.ToInt32(opcao))}");

            return (PersonagensEnum)int.Parse(opcao);
        }
    }
}