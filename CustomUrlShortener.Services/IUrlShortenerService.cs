using System.Collections.Generic;
using System.Threading.Tasks;
using CustomUrlShortener.DataAccess.Entities;

namespace CustomUrlShortener.Services
{
    public interface IUrlShortenerService
    {
        Task<List<UrlInfo>> GetAllAsync();

        Task DeleteAsync(int id);

        Task<UrlInfo> ShortenUrlAsync(string longUrl, string token = "");

        Task<string> IncreaseClicksAsync(string token);
    }
}
