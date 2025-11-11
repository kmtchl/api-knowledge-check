public class ProductValidator : IProductValidator
{   
    public ValidationResult Validate(ProductGetDto product)
    {
        var validationResult = new ValidationResult();

        if (string.IsNullOrWhiteSpace(product.Name))
            validationResult.AddError("Product name cannot be empty");

        if (product.Price <= 0)
            validationResult.AddError("Product price must be greater than zero");

        return validationResult;
    }

    public ValidationResult Validate(ProductUpdateDto product)
    {
        var validationResult = new ValidationResult();

        if (string.IsNullOrWhiteSpace(product.Name))
            validationResult.AddError("Product name cannot be empty");

        if (product.Price <= 0)
            validationResult.AddError("Product price must be greater than zero");

        return validationResult; 
    }
}