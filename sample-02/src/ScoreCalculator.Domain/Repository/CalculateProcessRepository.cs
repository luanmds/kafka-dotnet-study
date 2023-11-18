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

    public async Task SaveAsync(CalculateProcess calculateProcess) 
    {
        await _collection.InsertOneAsync(calculateProcess);
    }

    public async Task UpdateAsync(CalculateProcess calculateProcess) 
    {
        var filter = Builders<CalculateProcess>.Filter
            .Eq(x => x.Id, calculateProcess.Id);

        var update = Builders<CalculateProcess>.Update.Set(x => x.Status, calculateProcess.Status);

        await _collection.UpdateOneAsync(filter, update);
    }
}
