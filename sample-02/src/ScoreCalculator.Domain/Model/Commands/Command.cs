using System;

namespace ScoreCalculator.Domain.Model.Commands
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