using Godot;

public class GravityMod
{
    #region Properties

    [Export]
    public float Gravity { get; set; } = -9.8f;

    #endregion Properties
    // todo: gravity feels heavy

    #region Methods

    public void Apply(CharacterBody3D character, ref Vector3 velocity, double delta)
    {
        //GD.Print($@"On Cieling: {IsOnCeiling()}");
        if (character.IsOnCeiling())
        {
            velocity.Y = 0;
            velocity.Y += Gravity * (float)delta;
        }
        else if (!character.IsOnFloor())
            velocity.Y += Gravity * (float)delta;
        else if (velocity.Y <= 0)
            velocity.Y = 0;
        //GD.Print($@"Velocity: {_velocity.Y}");
    }

    #endregion Methods
}