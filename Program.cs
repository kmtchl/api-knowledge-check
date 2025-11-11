using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IProductValidator, ProductValidator>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

// API Objects
public record Product(int Id, string Name, decimal Price);

// DTO Objects
public class ProductDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

// Controllers
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductValidator validator) : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new(1, "Mouse", 25.5m),
        new(2, "Keyboard", 15.9m)
    };


    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll() => Ok(_products);

    [HttpPost]
    public ActionResult<Product>Create([FromBody]ProductDto product)
    {
        var validationResult = validator.Validate(product);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var newProduct = new Product(_products.Max(p => p.Id) + 1, product.Name, product.Price);
        _products!.Add(newProduct);

        return CreatedAtAction(nameof(Create), new {id = newProduct.Id}, newProduct);
    }
}

// Validation
public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();

    public void AddError(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            Errors.Add(message);
    }
}

public interface IProductValidator
{
    ValidationResult Validate(ProductDto product);
}

public class ProductValidator : IProductValidator
{
    public ValidationResult Validate(ProductDto product)
    {
        var validationResult = new ValidationResult();

        if (string.IsNullOrWhiteSpace(product.Name))
            validationResult.AddError("Product name cannot be empty");

        if (product.Price <= 0)
            validationResult.AddError("Product price must be greater than zero");

        return validationResult;
    }
}