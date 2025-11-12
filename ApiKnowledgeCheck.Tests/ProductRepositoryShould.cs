using Xunit;

public class ProductRepositoryShould
{
    private readonly ProductRepository sut = new ProductRepository();

    [Fact]
    public void GetAll_ExistingProducts_ShouldReturnProducts()
    {
        var products = sut.GetAll();
        Assert.NotEmpty(products);
    }

    [Fact]
    public void Create_ValidProduct_ShouldCreateProduct()
    {
        sut.Create(new Product(99, "TestProduct", 10));

        var result = sut.GetById(99);
        Assert.Equal(99, result!.Id);
        Assert.Equal("TestProduct", result!.Name);
        Assert.Equal(10, result!.Price);
    }

    [Fact]
    public void Delete_ExistingProduct_ShouldDeleteProduct()
    {
        sut.Create(new Product(99, "TestProduct", 10));
        
        sut.Delete(99);

        var result = sut.GetById(99);

        Assert.Null(result);
    }

    [Fact]
    public void GetById_ExistingProduct_ShouldReturnProduct()
    {
        var product = new Product(99, "TestProduct", 10);
        sut.Create(product);

        var result = sut.GetById(product.Id);

        Assert.NotNull(result);
        Assert.Equal(result.Id, product.Id);
        Assert.Equal(result.Name, product.Name);
        Assert.Equal(result.Price, product.Price);
    }

    [Fact]
    public void GetById_NonExistingProduct_ShouldReturnNull()
    {
        var result = sut.GetById(-1);

        Assert.Null(result);
    }

}