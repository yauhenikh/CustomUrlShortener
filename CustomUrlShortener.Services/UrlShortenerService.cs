using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CustomUrlShortener.DataAccess.DataContext;
using CustomUrlShortener.DataAccess.Entities;

namespace CustomUrlShortener.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly CustomUrlShortenerContext _context;

        public UrlShortenerService(CustomUrlShortenerContext context)
        {
            _context = context;
        }

        public async Task<List<UrlInfo>> GetAllAsync()
        {
            return await _context.UrlInfos.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            UrlInfo url = await _context.UrlInfos.FindAsync(id);

            if (url == null)
            {
                throw new ArgumentException("Cannot find UrlInfo with given id");
            }

            _context.UrlInfos.Remove(url);

            await _context.SaveChangesAsync();
        }

        public async Task<UrlInfo> ShortenUrlAsync(string longUrl, string token = "")
        {
            UrlInfo urlInfo = await _context.UrlInfos.Where(u => u.LongUrl == longUrl).FirstOrDefaultAsync();

            if (urlInfo != null)
            {
                return urlInfo;
            }

            if (!string.IsNullOrEmpty(token))
            {
                if (await _context.UrlInfos.Where(u => u.Token == token).AnyAsync())
                {
                    throw new ArgumentException("Token conflict exception");
                }
            }
            else
            {
                token = await GetNewTokenAsync();
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is empty");
            }

            urlInfo = new UrlInfo()
            {
                LongUrl = longUrl,
                Token = token,
                Created = DateTime.Now,
                ClickNumber = 0
            };

            _context.UrlInfos.Add(urlInfo);

            await _context.SaveChangesAsync();

            return urlInfo;
        }

        public async Task<string> IncreaseClicksAsync(string token)
        {
            UrlInfo urlInfo = await _context.UrlInfos.Where(u => u.Token == token).FirstOrDefaultAsync();

            if (urlInfo == null)
            {
                throw new ArgumentException("Url Info not found");
            }

            urlInfo.ClickNumber++;

            _context.UrlInfos.Update(urlInfo);

            await _context.SaveChangesAsync();

            return urlInfo.LongUrl;
        }

        private async Task<string> GetNewTokenAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                string token = Guid.NewGuid().ToString().Substring(0, 7);

                if (!await _context.UrlInfos.Where(u => u.Token == token).AnyAsync())
                {
                    return token;
                }
            }

            return null;
        }
    }
}
