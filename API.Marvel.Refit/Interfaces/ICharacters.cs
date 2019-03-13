using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace API.Marvel.Refit.Interfaces
{
    [Headers("MediaTypeWithQualityHeaderValue: application/json")]
    public interface ICharacters
    {
        [Get("/characters")]
        Task<HttpResponseMessage> Character(string ts, string apikey, string hash, string name);
    }
}