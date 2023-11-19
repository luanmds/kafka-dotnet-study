using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Domain.Model.Commands;

public class CalculateScore : Command
{   
    public CustomerScore CustomerScore { get; set; }
    public string ProcessId { get; }
    public bool IsEndOfProcess { get; }

    public CalculateScore(CustomerScore customerScore, string processId, bool isEndOfProcess, string sagaKey) : base(sagaKey)
    {
        CustomerScore = customerScore;        
        ProcessId = processId;
        IsEndOfProcess = isEndOfProcess;
    }

}