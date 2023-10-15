using System;
using System.Threading.Tasks;
using UrlShortener.Domain.Model.Entities;
using UrlShortener.Domain.Repository;

namespace UrlShortener.Domain.Model.Commands
{
    public class GenerateURL : Command
    {
        private readonly MongoRepository _repository;

        public GenerateURL(MongoRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Shorten(string url)
        {
            var urlCode = GenerateUrlCode();
            var entity = new ShortURL()
            {
                OriginalURL = url,
                UrlCode = urlCode
            };
            
            await _repository.CreateAsync(entity);
            return entity.Id;
        }

        private string GenerateUrlCode()
        {
            return Guid.NewGuid().ToString().Substring(0,6);
        }
    }
}