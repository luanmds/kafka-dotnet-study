using Newtonsoft.Json;

namespace ScoreCalculator.Domain.Model.Entities;

public class CustomerScore : Entity
{
    [JsonProperty(PropertyName = "Debts")]
    public double Debts { get; set; }

    [JsonProperty(PropertyName = "CreatedAt")]
    public DateTime CreatedAt { get; }

    public CustomerScore(double debts)
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        Debts = debts;
    }

    public CustomerScore()
    {

    }
}