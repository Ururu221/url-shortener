using Microsoft.EntityFrameworkCore;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Services
{
    public class ShortenUrlService : ICodeGenerator
    {
        public const int NumberOfCharsInShortLink = 4;

        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private ApplicationDbContext _dbContext = null!;

        public ShortenUrlService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        private Random _random = new Random();

        public async Task<string> GenerateUniqueCodeAsync()
        {
            var CharArr = new char[NumberOfCharsInShortLink];

            while (true)
            {
                for (int i = 0; i < CharArr.Length; i++)
                {
                    CharArr[i] = Alphabet[_random.Next(NumberOfCharsInShortLink - 1)];
                }

                var code = new string(CharArr);

                if (!await _dbContext.ShortenedUrl.AnyAsync(s => s.Code == code))
                {
                    return code;
                }
            }
        }
    }
}
