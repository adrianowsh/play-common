using System.Linq.Expressions;
using MongoDB.Driver;


namespace Play.Common.MongoDb;

public sealed class MongoRepository<T> : IRepository<T> where T : Entity
{
    private readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database)
    {
        dbCollection = database.GetCollection<T>($"{nameof(T)}s");
    }
    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        var result = await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        return result;
    }
    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        => await dbCollection.Find(filter).ToListAsync();

    public async Task<T> GetAsync(Guid id)
    {
        var filter = filterBuilder.Eq(e => e.Id, id);
        var result = await dbCollection.FindAsync(filter);
        return await result.FirstOrDefaultAsync();
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
         => await dbCollection.Find(filter).FirstOrDefaultAsync();

    public async Task CreateAsync(T entity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(entity));
        await dbCollection.InsertOneAsync(entity);
    }
    public async Task UpdateAsync(T entity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(entity));
        var filter = filterBuilder.Eq(e => e.Id, entity.Id);
        await dbCollection.ReplaceOneAsync(filter, entity);
    }
    public async Task RemoveAsync(Guid id)
    {
        var filter = filterBuilder.Eq(e => e.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}
