namespace Sneakers.API.Services;

public interface ISneakerService
{
    Task<Sneaker> AddSneaker(Sneaker newSneaker);
    Task<Order> AddOrder(Order order);
    Task<List<Brand>> GetBrands();
    Task<List<Occasion>> GetOccasions();
    Task<Sneaker> GetSneakerById(string id);
    Task SetupData();
}

public class SneakerService : ISneakerService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IOccasionRepository _occasionRepository;
    private readonly ISneakerRepository _sneakerRepository;
    private readonly IOrderRepository _orderRepository;

    public SneakerService(IBrandRepository brandRepository, IOccasionRepository occasionRepository, ISneakerRepository sneakerRepository, IOrderRepository orderRepository)
    {
        _brandRepository = brandRepository;
        _occasionRepository = occasionRepository;
        _sneakerRepository = sneakerRepository;
        _orderRepository = orderRepository;
    }

    public async Task<List<Brand>> GetBrands() => await _brandRepository.GetAllBrands();

    public async Task<List<Occasion>> GetOccasions() => await _occasionRepository.GetAllOccassions();

    public async Task<Sneaker> AddSneaker(Sneaker newSneaker)
    {
        await _sneakerRepository.AddSneaker(newSneaker);
        return newSneaker;
    }

    public async Task<Order> AddOrder(Order newOrder)
    {
        if (newOrder == null) throw new ArgumentException();

        Sneaker sneaker = await _sneakerRepository.GetSneaker(newOrder.SneakerId);
        if (sneaker == null) throw new ArgumentException();

        var order = await _orderRepository.AddOrder(newOrder);

        await _sneakerRepository.UpdateStock(order.SneakerId, --sneaker.Stock);

        return order;
    }

    public async Task<Sneaker> GetSneakerById(string id) => await _sneakerRepository.GetSneaker(id);

    public async Task SetupData()
    {
        try
        {
            if (!(await _brandRepository.GetAllBrands()).Any())
                await _brandRepository.AddBrands(new List<Brand>() { new Brand() { Name = "ASICS" }, new Brand() { Name = "CONVERSE" }, new Brand() { Name = "JORDAN" }, new Brand() { Name = "PUMA" } });

            if (!(await _occasionRepository.GetAllOccassions()).Any())
                await _occasionRepository.AddOccasions(new List<Occasion>() { new Occasion() { Description = "Sports" }, new Occasion() { Description = "Casual" }, new Occasion() { Description = "Skate" }, new Occasion() { Description = "Diner" } });
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}