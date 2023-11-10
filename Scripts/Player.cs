using Godot;
using System;
using System.Diagnostics;

// useful: https://stackoverflow.com/questions/73161622/how-to-make-godot-character-jump
public partial class Player : CharacterBody3D
{
    #region Fields

    private float _halfJumpTime = 0.5f;
    private Vector3 _velocity = Vector3.Zero;
    private bool canJump = true;
    private TimeSpan coyoteTime = new TimeSpan(0, 0, 0, 0, 300); 
    private Stopwatch coyoteTimer = new Stopwatch(); // Coyote time duration in seconds

    #endregion Fields

    #region Properties

    private float Gravity
    {
        get
        {
            return -InitialSpeed / _halfJumpTime;
        }
    }

    private float InitialSpeed
    {
        get
        {
            return 2.0f * MaxHeight / _halfJumpTime;
        }
    }

    private float MaxHeight
    {
        get
        {
            return JumpForce;
        }
    }

    [Export]
    public float JumpForce { get; set; } = 5;

    [Export]
    public int Speed { get; set; } = 14;

    #endregion Properties

    #region Methods

    private void ApplyGravity(double delta)
    {
        if (!IsOnFloor())
            _velocity.Y += Gravity * (float)delta;
        else if (_velocity.Y <= 0)
            _velocity.Y = 0;
    }

    private void HandleInput(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_up"))
        {
            direction.Z -= 1;
        }
        if (Input.IsActionPressed("move_down"))
        {
            direction.Z += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1;
        }
        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position - direction, Vector3.Up);
        }
        //GD.Print($@"Can: {canJump} + Coyote: {coyoteTimer.Elapsed} + {coyoteTimer.Elapsed != TimeSpan.Zero} + {coyoteTimer.Elapsed < coyoteTime}");
        // Implement Coyote Time
        if (IsOnFloor())
        {
            coyoteTimer.Reset();
            canJump = true;
            //GD.Print($@"Coyote Started");
        }
        else if (canJump && coyoteTimer.Elapsed == TimeSpan.Zero)
        {
            coyoteTimer.Start();
            canJump = true;
            //GD.Print($@"Coyote Jump");
            //GD.Print($@"Coyote: {coyoteTimer.Elapsed}");
        }
        else if (canJump && coyoteTimer.Elapsed != TimeSpan.Zero && coyoteTimer.Elapsed <= coyoteTime)
        {
            // do nothing
        } 
        else
        {
            coyoteTimer.Reset();
            canJump = false;
            //GD.Print($@"Coyote Ended");
        }
        //GD.Print($@"Coyote: {coyoteTimer.Elapsed}");
        // Implement Jump Buffering
        if (Input.IsActionJustPressed("jump"))
        {
            Jump();
        }

        MoveTowards(direction);
    }

    private void MoveTowards(Vector3 direction)
    {
        float targetSpeed = direction.Length() > 0 ? Speed : 0f;
        _velocity.X = Mathf.Lerp(_velocity.X, direction.X * targetSpeed, 0.1f);
        _velocity.Z = Mathf.Lerp(_velocity.Z, direction.Z * targetSpeed, 0.1f);
    }

    private void Jump()
    {
        if (canJump)
        {
            _velocity.Y = JumpForce;
            canJump = false;
            coyoteTimer.Reset();
        }
    }

    private void Move()
    {
        Velocity = _velocity;
        //GD.Print("Velocity: " + Velocity);
        MoveAndSlide();
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleInput(delta);
        ApplyGravity(delta);
        Move();
    }

    #endregion Methods

    // Method for throwing object
    // Object needs to be thrown in an arc to a location.

    // Components needed:
    // Health
    // Movespeed

    // Class Friend:
    //
}