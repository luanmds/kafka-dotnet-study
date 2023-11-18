namespace ScoreCalculator.Domain.Model.Entities;

public class CustomerScore : Entity
{
    public double Debts { get; set; }
    public DateTime CreatedAt { get; }

    public CustomerScore(double debts)
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Debts = debts;
    }
}