using System;

namespace UrlShortener.Domain.Model.Commands
{
    public class Command
    {
        public string Id { get; }

        public Command()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}