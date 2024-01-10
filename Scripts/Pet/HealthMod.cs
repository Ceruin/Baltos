public class HealthMod
{
    public int HP { get; set; } = 5;

    public void IncreaseHP(int amount)
    {
        HP += amount;
    }

    public void DecreaseHP(int amount)
    {
        HP -= amount;
    }
}
