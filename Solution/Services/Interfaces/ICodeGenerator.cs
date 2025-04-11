namespace UrlShortener.Services.Interfaces
{
    public interface ICodeGenerator
    {
        Task<string> GenerateUniqueCodeAsync();
    }
}
