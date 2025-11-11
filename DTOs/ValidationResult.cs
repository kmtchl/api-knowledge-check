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
