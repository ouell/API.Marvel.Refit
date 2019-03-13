using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using API.Marvel.Refit.Helpers;
using API.Marvel.Refit.Interfaces;
using API.Marvel.Refit.Model;
using Newtonsoft.Json;
using Refit;

namespace API.Marvel.Refit
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();

            var ts = DateTime.Now.Ticks.ToString();
            var publicKey = config.GetSection("Marvel:PublicKey").Value;
            var hash = Util.GerarHash(ts, publicKey, config.GetSection("Marvel:PrivateKey").Value);
            var fim = 1;
            while (fim != 0)
            {
                var opcao = Menu();
                if (opcao == Personagens.Sair)
                    break;

                var characterAPI = RestService.For<ICharacters>(config.GetSection("UrlBase").Value);

                var resultado = characterAPI.Character(ts, publicKey, hash, GetEnumDescription(opcao)).Result;
                resultado.EnsureSuccessStatusCode();

                string conteudo =
                    resultado.Content.ReadAsStringAsync().Result;

                dynamic dados = JsonConvert.DeserializeObject(conteudo);

                var personagem = new Personagem
                {
                    Id = dados.data.results[0].id,
                    Nome = dados.data.results[0].name,
                    Descricao = dados.data.results[0].description,
                    UrlImagem =
                        $"{dados.data.results[0].thumbnail.path}.{dados.data.results[0].thumbnail.extension}"
                };

                Console.WriteLine(JsonConvert.SerializeObject(personagem));
            }
        }

        internal static Personagens Menu()
        {
            Console.WriteLine("API Marvel - Refit\r");
            Console.WriteLine($"{new string('-', 70)}\r");
            Console.WriteLine("Digite o número correspondente ao personagem: \n");
            
            var personagens = new StringBuilder();
            personagens.Append($"{Personagens.CapitaoAmerica}: {(int)(Personagens.CapitaoAmerica)}\r\n");
            personagens.Append($"{Personagens.DoutorEstranho}: {(int)(Personagens.DoutorEstranho)}\r\n");
            personagens.Append($"{Personagens.IronMan}: {(int)(Personagens.IronMan)}\r\n");
            personagens.Append($"{Personagens.SpiderMan}: {(int)(Personagens.SpiderMan)}\r\n");
            personagens.Append($"{Personagens.Thor}: {(int)(Personagens.Thor)}\r\n");
            personagens.Append($"{Personagens.Sair}: {(int)(Personagens.Sair)}\r");

            Console.WriteLine(personagens);
            Console.WriteLine($"{new string('-', 70)}\r");
            Console.Write("Opção escolhida: ");

            var opcao = Console.ReadLine();
            Console.WriteLine($"Opção escolhida: {opcao}");

            return (Personagens)int.Parse(opcao);
        }

        private static string GetEnumDescription(Enum value)
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
    }
}
