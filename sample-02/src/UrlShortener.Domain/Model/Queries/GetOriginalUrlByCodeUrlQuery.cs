using UrlShortener.Domain.Model.Entities;

namespace UrlShortener.Domain.Model.Queries
{
    public class GetOriginalUrlByCodeUrlQuery : IQuery<GetOriginalUrlByCodeUrlQuery>
    {
        public string OriginalUrl { get; set; }
    }
}