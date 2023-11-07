using System;

namespace ScoreCalculator.Domain.Model.Entities;

public class CustomerScoreData : Entity
{
    public decimal Debts { get; set; }
    public int Score { get; internal set; }
    public DateTime CreatedAt { get; }

    public CustomerScoreData(decimal debts)
    {
        CreatedAt = DateTime.Now;
        Debts = debts;
    }

    public SetScore(int score)
    {
        Score ?= score;
    }
}