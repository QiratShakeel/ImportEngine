namespace ImportEngine.Features.Import.Models
{
    public class ValidationError
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new();
    }
}