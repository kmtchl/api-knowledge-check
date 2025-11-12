using Microsoft.AspNetCore.Mvc;
using Xunit;

public class ProductsControllerShould
{
    private readonly ProductsController _controller;

    public ProductsControllerShould()
    {
        var repo = new ProductRepository();
        var validator = new ProductValidator();
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ProductsController>();
        _controller = new ProductsController(repo, validator, logger);
    }

    [Fact]
    public void GetAll_ReturnsOkResult()
    {
        var result = _controller.GetAll();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void Get_InvalidId_ReturnsNotFound()
    {
        var result = _controller.Get(999);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Create_InvalidModel_ReturnsBadRequest()
    {
        var dto = new ProductGetDto { Name = "", Price = 0 };
        var result = _controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
