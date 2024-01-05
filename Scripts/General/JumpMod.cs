using Godot;

public partial class JumpMod : Node3D
{
    #region Properties

    [Export]
    public float JumpHeight { get; set; } = 5.0f;

    #endregion Properties
    // Desired jump height in meters

    #region Methods

    public void Apply(ref Vector3 velocity, GravityMod gravity)
    {
        velocity.Y = CalculateJumpForce(gravity);
    }

    public float CalculateJumpForce(GravityMod gravity)
    {
        // Calculate the jump force based on the desired jump height and gravity
        return Mathf.Sqrt(2.0f * Mathf.Abs(gravity.Gravity) * JumpHeight);
    }

    #endregion Methods
}