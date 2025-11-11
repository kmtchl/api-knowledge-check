public class ProductRepository : IProductRepository
{
    private static List<Product> _products = new()
    {
        new(1, "Mouse", 25.5m),
        new(2, "Keyboard", 15.9m)
    };

    public List<Product> GetAll()
    {
        return _products;
    }

    public void Create(Product product)
    {
        _products.Add(product);
    }

    public void Update(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index >= 0) _products[index] = product;
    }

    public void Delete(int id)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is not null) _products.Remove(existing);
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
}