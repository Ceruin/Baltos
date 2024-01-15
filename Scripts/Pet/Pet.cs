using Godot;

public partial class Pet : CharacterBody3D
{
    #region Fields
    private GrabbableMod Grab = new GrabbableMod();
    private HealthMod HP = new HealthMod();
    private HappinessMod Joy = new HappinessMod();
    private RandomizedPathingMod Pathing = new RandomizedPathingMod();
    private ParticleExplosionMod Explosion = new ParticleExplosionMod();
    #endregion Fields

    // Implement basic randomized pathing
    private void RandomizedPathing()
    {
        // Add code for randomized pathing here
    }

    // Implement happiness when grabbed/picked up
    private void OnGrabbed()
    {
        Joy.IncreaseHappiness();
    }

    // Implement sadness when thrown/collides with an object and loses HP
    private void OnCollision()
    {
        HP.DecreaseHealth();
        Joy.DecreaseHappiness();
    }

    // Check if HP is gone and trigger particle explosion
    private void CheckHealth()
    {
        if (HP.IsHealthZero())
        {
            Explosion.TriggerParticleExplosion();
        }
    }
}
