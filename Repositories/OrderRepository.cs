namespace Sneakers.API.Repositories;

public interface IOrderRepository
{
    Task<Order> AddOrder(Order order);
}

public class OrderRepository : IOrderRepository
{


    private IMongoContext _context;

    public OrderRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<Order> AddOrder(Order order)
    {
        await _context.OrdersCollection.InsertOneAsync(order);
        return order;
    }
}