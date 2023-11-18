using MediatR;

namespace ScoreCalculator.Domain.Model.Commands
{
    public class Command : IRequest<Unit>
    {
        public string Id { get; }
        public string SagaKey { get; private set; }

        public Command(string sagaKey)
        {
            Id = Guid.NewGuid().ToString();
            SagaKey = sagaKey;
        }
    }
}