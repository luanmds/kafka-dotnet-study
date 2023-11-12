namespace ScoreCalculator.Domain.Model.Entities;

public class CustomerScore : Entity
{
    public decimal Debts { get; set; }
    public int? Score { get; internal set; }
    public DateTime CreatedAt { get; }

    public CustomerScore(decimal debts)
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Debts = debts;
    }

    public void SetScore(int score)
    {
        Score ??= score;
    }
}