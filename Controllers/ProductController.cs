using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepository, IProductValidator validator, ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll() => Ok(productRepository.GetAll());

    [HttpGet("{id}")]
    public ActionResult<ProductGetDto> Get(int id)
    {
        var product = productRepository.GetById(id);
        if (product == null)
        {
            logger.LogWarning($"Product with id {id} not found");
            return NotFound($"Product with id {id} does not exist");
        }

        return Ok(new ProductGetDto() { Name = product.Name, Price = product.Price });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductUpdateDto productDto)
    {
        var validationResult = validator.Validate(productDto);
        if (!validationResult.IsValid)
        {
            logger.LogError(string.Join(",", validationResult.Errors));
            return BadRequest(validationResult.Errors);
        }

        var existing = productRepository.GetById(id);
        if (existing is null)
        {
            logger.LogWarning($"Product with id {id} not found");
            return NotFound($"Product with ID {id} not found.");
        }

        productRepository.Update(new Product(productDto.Id, productDto.Name, productDto.Price));
        return NoContent();
    }

    [HttpPost]
    public ActionResult<Product> Create([FromBody] ProductGetDto product)
    {
        var validationResult = validator.Validate(product);
        if (!validationResult.IsValid)
        {
            logger.LogError(string.Join(",", validationResult.Errors));
            return BadRequest(validationResult.Errors);
        }
        var products = productRepository.GetAll();
        var newProduct = new Product(products.Max(p => p.Id) + 1, product.Name, product.Price);

        productRepository.Create(newProduct);
        return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
    }

    [HttpDelete("{id}")]
    public ActionResult<Product> Delete(int id)
    {
        if (id <= 0)
        {
            logger.LogError("Product Id sent that is less than or equal to 0: {id}", id);
            return BadRequest("Product Id cannot be less than or equal to 0");
        }
        
        var existing = productRepository.GetById(id);
        if (existing is null) return NotFound($"No product with id {id} found.");

        productRepository.Delete(id);

        return NoContent();
    }
}
