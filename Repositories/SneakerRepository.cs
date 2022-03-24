namespace Sneakers.API.Repositories;

public interface ISneakerRepository
{
    Task<Sneaker> AddSneaker(Sneaker newSneaker);
    Task<Sneaker> GetSneaker(string id);
    Task<Sneaker> UpdateStock(string sneakerId, int stock);
}

public class SneakerRepository : ISneakerRepository
{
    private readonly IMongoContext _context;
    public SneakerRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<Sneaker> AddSneaker(Sneaker newSneaker)
    {
        await _context.SneakerCollection.InsertOneAsync(newSneaker);
        return newSneaker;
    }

    public async Task<Sneaker> UpdateStock(string sneakerId, int stock)
    {

        try
        {
            var filter = Builders<Sneaker>.Filter.Eq("Id", sneakerId);
            var update = Builders<Sneaker>.Update.Set("Stock", stock);
            var result = await _context.SneakerCollection.UpdateOneAsync(filter, update);
            return await GetSneaker(sneakerId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    public async Task<Sneaker> GetSneaker(string id) => await _context.SneakerCollection.Find(s => s.SneakerId == id).FirstOrDefaultAsync();
}