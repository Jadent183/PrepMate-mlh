using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DynamoDBService
{
    private readonly IDynamoDBContext _context;

    public DynamoDBService(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task SaveItemAsync<T>(T item) where T : class
    {
        await _context.SaveAsync(item);
    }

    public async Task<T> LoadItemAsync<T>(string hashKey, string rangeKey = null) where T : class
    {
        return await _context.LoadAsync<T>(hashKey, rangeKey);
    }

    public async Task<List<T>> QueryAsync<T>(QueryOperationConfig queryConfig) where T : class
    {
        var search = _context.FromQueryAsync<T>(queryConfig);
        return await search.GetRemainingAsync();
    }

    public async Task DeleteItemAsync<T>(string hashKey, string rangeKey = null) where T : class
    {
        await _context.DeleteAsync<T>(hashKey, rangeKey);
    }
}