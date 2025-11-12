using Xunit;

namespace ApiKnowledgeCheck.Tests;
public class ProductValidatorShould
{
    [Fact]
    public void Validate_ValidProduct_ReturnValidResult()
    {
        ProductGetDto product = new ProductGetDto
        {
            Name = "test",
            Price = 10m
        };

        var sut = new ProductValidator();

        var result = sut.Validate(product);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", 10)]
    [InlineData(" ", 10)]
    public void Validate_InvalidName_ReturnInvalidResult(string productName, decimal productPrice)
    {
        ProductGetDto product = new ProductGetDto
        {
            Name = productName,
            Price = productPrice
        };

        var sut = new ProductValidator();

        var result = sut.Validate(product);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("Name", 0)]
    [InlineData("Name", -1)]
    [InlineData("Name", -100)]
    public void Validate_InvalidPrice_ReturnInvalidResult(string productName, decimal productPrice)
    {
        ProductGetDto product = new ProductGetDto
        {
            Name = productName,
            Price = productPrice
        };

        var sut = new ProductValidator();

        var result = sut.Validate(product);

        Assert.False(result.IsValid);
    }
}