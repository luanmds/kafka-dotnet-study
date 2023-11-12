using ScoreCalculator.Domain.MessageBus;

namespace ScoreCalculator.Domain.Model.Commands;

public class CancelCalculateScore : Command
{    
    public string ProcessId { get; private set; }

    public CancelCalculateScore(string processId, string sagaKey) : base(sagaKey)
    {
        ProcessId = processId;
    }

}