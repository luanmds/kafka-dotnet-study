using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Domain.Model.Commands;

public class CalculateScore : Command
{   
    public CustomerScore CustomerScore { get; private set; }
    public string ProcessId { get; }
    public bool IsEndOfProcess { get; }

    public CalculateScore(CustomerScore data, string processId, bool isEndOfProcess, string sagaKey) : base(sagaKey)
    {
        CustomerScore = data;        
        ProcessId = processId;
        IsEndOfProcess = isEndOfProcess;
    }

}