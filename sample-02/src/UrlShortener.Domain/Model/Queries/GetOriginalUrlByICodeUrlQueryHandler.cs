using System.Threading.Tasks;
using UrlShortener.Domain.Model.Entities;
using UrlShortener.Domain.Repository;

namespace UrlShortener.Domain.Model.Queries
{
    public class GetOriginalUrlByCodeUrlQueryHandler
    {
        private MongoRepository _repository;

        public GetOriginalUrlByCodeUrlQueryHandler(MongoRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetOriginalUrlByCodeUrlQuery> HandleAsync(string code)
        {
            var entity = await _repository.GetAsync(code);
            
            return new GetOriginalUrlByCodeUrlQuery()
            {
                OriginalUrl = entity.OriginalURL
            };
        }
    }
}