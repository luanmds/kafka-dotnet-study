using MongoDB.Driver;
using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Domain.Repository;

public class CalculateProcessRepository
{
    private const string CollectionName = nameof(CalculateProcess);
    private readonly IMongoCollection<CalculateProcess> _collection;
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;

    public CalculateProcessRepository(IMongoClient mongoClient, DatabaseSettings databaseSettings)
    {
        _mongoClient = mongoClient;

        _mongoDatabase = _mongoClient.GetDatabase(databaseSettings.DatabaseName);

        _collection = _mongoDatabase.GetCollection<CalculateProcess>(CollectionName);
    }
    
    public async Task<CalculateProcess?> GetByIdAsync(string id) =>
        await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
}
