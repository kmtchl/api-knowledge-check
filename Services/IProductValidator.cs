public interface IProductValidator
{
    ValidationResult Validate(ProductGetDto product);
    ValidationResult Validate(ProductUpdateDto product);
}
