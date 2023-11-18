using ScoreCalculator.Domain.Model.Enumerations;

namespace ScoreCalculator.Domain.Model.Entities;

public class CalculateProcess : Entity
{
    public ProcessStatus Status { get; internal set; }
    public CalculateProcess() 
    {
        Id = Guid.NewGuid().ToString();
        Status = ProcessStatus.RUNNING;
    }

    public void CompleteProcess()
    {
        Status = ProcessStatus.COMPLETED;
    }

    public void CancelProcess()
    {
        Status = ProcessStatus.CANCELLED;
    }
}
