using UrlShortener.Domain.Model.Entities;

namespace UrlShortener.Domain.Model.Queries
{
    public class GetUrlGeneratedByIdQuery : IQuery<GetUrlGeneratedByIdQuery>
    {
        public string ShortenedUrl { get; set; }
    }
}