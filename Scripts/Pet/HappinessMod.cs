using System.ComponentModel.DataAnnotations;

public class HappinessMod
{
    [Range(0, 100)]
    public int Happiness { get; private set; } = 100;
    public enum State { Happy, Neutral, Sad }

    public void IncreaseHappiness(int amount)
    {
        Happiness += amount;
        if (Happiness > 100)
        {
            Happiness = 100;
        }
    }

    public void DecreaseHappiness(int amount)
    {
        Happiness -= amount;
        if (Happiness < 0)
        {
            Happiness = 0;
        }
    }

    public State GetState()
    {
        if (Happiness >= 70)
        {
            return State.Happy;
        }
        else if (Happiness >= 30)
        {
            return State.Neutral;
        }
        else
        {
            return State.Sad;
        }
    }
}
