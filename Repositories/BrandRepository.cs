namespace Sneakers.API.Repositories;

public interface IBrandRepository
{
    Task<List<Brand>> AddBrands(List<Brand> brands);
    Task<List<Brand>> GetAllBrands();
}

public class BrandRepository : IBrandRepository
{
    private readonly IMongoContext _context;
    public BrandRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Brand>> GetAllBrands() => await _context.BrandsCollection.Find(_ => true).ToListAsync();

    public async Task<List<Brand>> AddBrands(List<Brand> brands)
    {
        await _context.BrandsCollection.InsertManyAsync(brands);
        return brands;
    }
}