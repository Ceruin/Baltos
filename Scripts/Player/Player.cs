using Godot;

// useful: https://stackoverflow.com/questions/73161622/how-to-make-godot-character-jump
public partial class Player : CharacterBody3D
{
    #region Fields
    private const float KickCooldown = 1.0f;
    private RigidBody3D _grabbedObject;
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

    private void Grab(RigidBody3D body)
    {
        _grabbedObject = body;
        _grabbedObject.GlobalTransform = GlobalTransform;
        _grabbedObject.LinearVelocity = Vector3.Zero;
        _grabbedObject.AngularVelocity = Vector3.Zero;
        _grabbedObject.CollisionLayer = 0;
        _grabbedObject.CollisionMask = 0;
        _grabbedObject.Mode = RigidBody.ModeEnum.Kinematic;
        _grabbedObject.LinearDamp = 0;
        _grabbedObject.AngularDamp = 0;
        _grabbedObject.LinearFactor = Vector3.Zero;
        _grabbedObject.AngularFactor = Vector3.Zero;
        _grabbedObject.GravityScale = 0;
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

        UpdateVelocity(direction);
    }

    private void Jump()
    {
        if (Coyote.CanJump)
        {
            JumpFactor.Apply(ref _velocity, Gravity);
            Coyote.Reset();
        }
    }

    private void Move()
    {
        Velocity = _velocity;
        //GD.Print("Velocity: " + Velocity);
        MoveAndSlide();
    }

    private void Release(RigidBody3D body)
    {
        if (_grabbedObject == null)
            return;

        _grabbedObject.CollisionLayer = 1;
        _grabbedObject.CollisionMask = 1;
        _grabbedObject.Mode = RigidBody.ModeEnum.Rigid;
        _grabbedObject.LinearDamp = 0.1f;
        _grabbedObject.AngularDamp = 0.1f;
        _grabbedObject.LinearFactor = Vector3.One;
        _grabbedObject.AngularFactor = Vector3.One;
        _grabbedObject.GravityScale = 1;

        var playerVelocity = _velocity;
        var throwForce = playerVelocity * 2;
        _grabbedObject.ApplyCentralImpulse(throwForce);

        _grabbedObject = null;
    }

    private void UpdateVelocity(Vector3 direction)
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