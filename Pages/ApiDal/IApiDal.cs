using System.Threading.Tasks;
using Pages.Models;

namespace Pages.ApiDal
{
    public interface IApiDal
    {
        bool Create<T>(T t, string uri);
        Task<T> Get<T>(string uri);
        Task<PlayerDto> GetPlayer(string avatar, string emailAddress, string passPhrase);
    }
}