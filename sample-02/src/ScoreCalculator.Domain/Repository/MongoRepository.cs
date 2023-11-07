using System.Threading.Tasks;
using MongoDB.Driver;
using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Domain.Repository
{
    public class MongoRepository
    {
        private readonly IMongoCollection<ShortURL> _urlsCollection;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;

        public MongoRepository(IMongoClient mongoClient, DatabaseSettings databaseSettings)
        {
            _mongoClient = mongoClient;

            _mongoDatabase = _mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _urlsCollection = _mongoDatabase.GetCollection<ShortURL>(
                databaseSettings.UrlsCollectionName);
        }
        
        public async Task<ShortURL?> GetByIdAsync(string id) =>
            await _urlsCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        
        public async Task<ShortURL?> GetAsync(string urlCode) =>
            await _urlsCollection
                .Find(x => x.UrlCode == urlCode)
                .FirstOrDefaultAsync();

        public async Task CreateAsync(ShortURL url) =>
            await _urlsCollection.InsertOneAsync(url);
    }
}
