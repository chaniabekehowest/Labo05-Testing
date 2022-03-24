var builder = WebApplication.CreateBuilder(args);


var mongoSettings = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(mongoSettings);

var apikeySettings = builder.Configuration.GetSection("ApiKeySettings");
builder.Services.Configure<ApiKeySettings>(apikeySettings);

builder.Services.AddTransient<IMongoContext, MongoContext>();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SneakerValidator>());

builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<ISneakerRepository, SneakerRepository>();
builder.Services.AddTransient<IOccasionRepository, OccasionRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<ISneakerService, SneakerService>();


var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapGet("/", () => "Hello World!");

app.MapGet("/setup", (ISneakerService sneakerService) => sneakerService.SetupData());

app.MapGet("/brands", (ISneakerService sneakerService) => sneakerService.GetBrands());

app.MapGet("/occasions", (ISneakerService sneakerService) => sneakerService.GetOccasions());

//app.MapGet("/sneakers", (ISneakerService sneakerService) => sneakerService.GetSneakers());

app.MapGet("/sneaker/{id}", (ISneakerService sneakerService, string id) => sneakerService.GetSneakerById(id));

app.MapPost("/sneaker", async (IValidator<Sneaker> validator, ISneakerService sneakerService, Sneaker sneaker) =>
{
    var validatorResult = validator.Validate(sneaker);
    if (!validatorResult.IsValid) return Results.BadRequest();
    var result = await sneakerService.AddSneaker(sneaker);
    return Results.Created($"/sneaker/{result.SneakerId}", result);
});

app.MapPost("/order", async (ISneakerService sneakerService, Order order) =>
{
    var result = await sneakerService.AddOrder(order);
    return Results.Created($"/order/{result.OrderId}", result);
});

app.Run();

//Hack om testen te doen werken 
public partial class Program { }
