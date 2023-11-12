using ScoreCalculator.Domain.MessageBus;

namespace ScoreCalculator.Domain.Model.Commands;

public class RemoveDataOfProcess : Command
{    
    public RemoveDataOfProcess(string sagaKey) : base(sagaKey)
    {
        
    }

}