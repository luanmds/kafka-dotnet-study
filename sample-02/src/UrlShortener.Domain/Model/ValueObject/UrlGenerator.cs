namespace UrlShortener.Domain.Model.ValueObject
{
    public class UrlGenerator
    {
        public static string GenerateShortUrl(string scheme, string host)
        {
            var uri = $"{scheme}://{host}";            
            
            return "";
        }
    }
}