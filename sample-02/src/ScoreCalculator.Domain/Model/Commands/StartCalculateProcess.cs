using ScoreCalculator.Domain.MessageBus;

namespace ScoreCalculator.Domain.Model.Commands;

public class StartCalculateProcess : Command
{
    public string ProcessId { get; private set; }
    
    public StartCalculateProcess(string processId, string sagaKey) : base(sagaKey)
    {
        ProcessId = processId;
    }

}