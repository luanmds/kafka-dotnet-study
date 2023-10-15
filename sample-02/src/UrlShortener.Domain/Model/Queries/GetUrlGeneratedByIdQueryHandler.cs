using System.Threading.Tasks;
using UrlShortener.Domain.Model.Entities;
using UrlShortener.Domain.Repository;

namespace UrlShortener.Domain.Model.Queries
{
    public class GetUrlGeneratedByIdQueryHandler
    {
        private MongoRepository _repository;

        public GetUrlGeneratedByIdQueryHandler(MongoRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUrlGeneratedByIdQuery> HandleAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            
            return new GetUrlGeneratedByIdQuery()
            {
                ShortenedUrl = entity.UrlCode
            };
        }
    }
}