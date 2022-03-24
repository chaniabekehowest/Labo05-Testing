namespace Sneakers.API.Repositories;

public interface IOccasionRepository
{
    Task<List<Occasion>> AddOccasions(List<Occasion> occasions);
    Task<List<Occasion>> GetAllOccassions();
}

public class OccasionRepository : IOccasionRepository
{
    private readonly IMongoContext _context;
    public OccasionRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Occasion>> GetAllOccassions() => await _context.OccasionCollection.Find(_ => true).ToListAsync();

    public async Task<List<Occasion>> AddOccasions(List<Occasion> occasions)
    {
        await _context.OccasionCollection.InsertManyAsync(occasions);
        return occasions;
    }
}