using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Domain.Model.Commands;

public class StartCalculateProcess : Command
{
    public CalculateProcess CalculateProcess { get; private set; }
    
    public StartCalculateProcess(CalculateProcess calculateProcess, string sagaKey) : base(sagaKey)
    {
        CalculateProcess = calculateProcess;
    }

}