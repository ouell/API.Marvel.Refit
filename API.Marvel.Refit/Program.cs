using API.Marvel.Refit.Interfaces;
using API.Marvel.Refit.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Refit;
using System;
using System.IO;

namespace API.Marvel.Refit
{
    /// <summary>
    /// Classe responsável pelo programa
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();

            var ts = DateTime.Now.Ticks.ToString();
            var publicKey = config.GetSection("Marvel:PublicKey").Value;
            var hash = Util.GerarHash(ts, publicKey, config.GetSection("Marvel:PrivateKey").Value);

            Console.WriteLine("API Marvel - Refit\r");

            var opcao = PersonagensEnum.CapitaoAmerica;
            while (opcao != PersonagensEnum.Sair)
            {
                opcao = Util.Menu();
                if (opcao == PersonagensEnum.Sair)
                    break;

                var characterApi = RestService.For<ICharacters>(config.GetSection("UrlBase").Value);

                var resultado = characterApi.Character(ts, publicKey, hash, Util.GetEnumDescription(opcao)).Result;
                resultado.EnsureSuccessStatusCode();

                var conteudo = resultado.Content.ReadAsStringAsync().Result;
                
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
    }
}
