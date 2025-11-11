public interface IProductRepository
{
    void Create(Product product);
    void Delete(int id);
    Product? GetById(int id);
    List<Product> GetAll();
    void Update(Product product);
}