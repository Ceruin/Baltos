using Godot;

// useful: https://stackoverflow.com/questions/73161622/how-to-make-godot-character-jump
public partial class Player : CharacterBody3D
{
    #region Fields
    private const float KickCooldown = 1.0f;
    private AnimatedSprite3D _sprite;
    private Vector3 _velocity = Vector3.Zero;

    // Set the cooldown time in seconds
    private float kickTimer = 0.0f;

    public CoyoteMod Coyote = new();
    #endregion Fields
    #region Properties
    public GravityMod Gravity { get; set; } = new();
    public JumpMod JumpFactor { get; set; } = new();
    public SpeedMod Speed { get; set; } = new();
    #endregion Properties
    #region Methods

    private void _on_kick_area_body_entered(PhysicsBody3D body)
    {
        // Check if the entered area is a kickable object
        if (body is RigidBody3D rigidBody)
        {
            // Call the kick method
            Kick(rigidBody);
        }
    }

    private void HandleInput(double delta)
    {
        var direction = Vector3.Zero;

        Camera3D camera = GetNode<Camera3D>("CameraPivot/Camera3D");
        var forward = -camera.GlobalTransform.Basis.Z;
        var right = camera.GlobalTransform.Basis.X;

        if (InputMod.Up) direction += forward;
        if (InputMod.Down) direction -= forward;
        if (InputMod.Left) direction -= right;
        if (InputMod.Right) direction += right;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position - direction, Vector3.Up);
        }

        Coyote.Process(this);

        if (InputMod.Jump) Jump(); // Implement Jump Buffering

        MoveTowards(direction);
    }

    private void Jump()
    {
        if (Coyote.CanJump)
        {
            JumpFactor.Apply(ref _velocity, Gravity);
            Coyote.Reset();
        }
    }

    private void Kick(RigidBody3D body)
    {
        // Set the kick cooldown
        kickTimer = KickCooldown;

        // Handle the kick logic here
        GD.Print("Kick!");

        // You can access the object that triggered the kick using area.GetOverlappingBodies()
        // Apply force or perform any other actions as needed

        // Get the forward direction of the player (normalized)
        var node = GetNode<Node3D>("Pivot");
        Vector3 kickDirection = node.Transform.Basis.Z.Normalized();

        // Set the kick force magnitude (adjust as needed)
        float kickForceMagnitude = 10.0f;

        // Calculate the kick force vector
        Vector3 kickForce = kickDirection * kickForceMagnitude;
        kickForce.Y += 1;
        // Apply the kick force
        body.ApplyCentralImpulse(kickForce);
        GD.Print("Forward Direction: " + kickDirection);
        // todo: make ball apply same physics as player
        // todo: player gravity to world.
    }

    private void Move()
    {
        Velocity = _velocity;
        //GD.Print("Velocity: " + Velocity);
        MoveAndSlide();
    }

    private void MoveTowards(Vector3 direction)
    {
        float targetSpeed = direction.Length() > 0 ? Speed.Speed : 0f;
        _velocity.X = Mathf.Lerp(_velocity.X, direction.X * targetSpeed, 0.1f);
        _velocity.Z = Mathf.Lerp(_velocity.Z, direction.Z * targetSpeed, 0.1f);
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleInput(delta);
        Gravity.Apply(this, ref _velocity, delta);
        Move();
    }

    public override void _Process(double delta)
    {
        // Handle kick cooldown
        if (kickTimer > 0)
            kickTimer -= (float)delta;
    }

    public override void _Ready()
    {
    }

    #endregion Methods
}