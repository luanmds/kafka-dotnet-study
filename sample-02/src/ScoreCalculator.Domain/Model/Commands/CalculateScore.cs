using ScoreCalculator.Domain.MessageBus;
using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Domain.Model.Commands;

public class CalculateScore : Command
{   
    public CustomerScore CustomerScore { get; private set; }
    
    public CalculateScore(CustomerScore data, string sagaKey) : base(sagaKey)
    {
        CustomerScore = data;        
    }

}