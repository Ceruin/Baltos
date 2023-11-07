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

	private TimeSpan coyoteTime = new TimeSpan(0, 0, 0, 0, 500);

	// Coyote time duration in seconds
	private Stopwatch coyoteTimer = new Stopwatch();

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
			return JumpHeight;
		}
	}

	[Export]
	public float JumpHeight { get; set; } = 20f;

	[Export]
	public int JumpImpulse { get; set; } = 20;

	[Export]
	public int Speed { get; set; } = 14;

	#endregion Properties

	#region Methods

	private void ApplyGravity(double delta)
	{
		_velocity.Y += Gravity * (float)delta;
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

		// Implement Coyote Time
		if (IsOnFloor())
		{
			coyoteTimer.Reset();
			canJump = true;
		}
		else if (canJump && coyoteTimer.Elapsed != TimeSpan.Zero && coyoteTimer.Elapsed < coyoteTime)
		{
			coyoteTimer.Start();
			canJump = true;
		}
		else
		{
			coyoteTimer.Stop();
			canJump = false;
		}

		// Implement Jump Buffering
		if (Input.IsActionPressed("jump"))
		{
			Jump();
		}

		MoveTowards(direction);
	}

	private void MoveTowards(Vector3 direction)
	{
		// Apply slowdown when the player stops moving
		if (direction == Vector3.Zero)
		{
			_velocity.X = Mathf.Lerp(_velocity.X, 0f, 0.1f);
			_velocity.Z = Mathf.Lerp(_velocity.Z, 0f, 0.1f);
		}
		else
		{
			_velocity.X = direction.X * Speed;
			_velocity.Z = direction.Z * Speed;
		}
	}

	private void Jump()
	{
		if (canJump)
		{
			_velocity.Y = JumpImpulse;
			canJump = false;
			coyoteTimer.Reset();
		}
	}

	private void Move()
	{
		Velocity = _velocity;
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
