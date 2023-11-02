using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleration { get; set; } = 75;
	[Export]
	public int JumpImpulse { get; set; } = 20;
	[Export]
	public float JumpTime { get; set; } = 0.5f;

	private Vector3 _velocity = Vector3.Zero;
	private float _jumpProgress = 0f;
	private float _jumpGoal = 0f;

	public override void _PhysicsProcess(double delta)
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

		_velocity.X = direction.X * Speed;
		_velocity.Z = direction.Z * Speed;

		// Gravity needs to be applied on jump, lerp may be not needed? 
		

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_jumpProgress = 0f;
			_jumpGoal = _velocity.Y + JumpImpulse;
			GD.Print("Jump Prog: " + _jumpProgress);
			GD.Print("Jump Goal: " + _jumpGoal);
		}

		
		if (_jumpProgress < _jumpGoal)
		{
			float jumpHeight = Mathf.Lerp(0, JumpImpulse, JumpTime);
			_velocity.Y = jumpHeight;
			_jumpProgress += _velocity.Y;
			GD.Print("Jump Prog: " + _jumpProgress);
			GD.Print("Jump Goal: " + _jumpGoal);
		}
		else if (!IsOnFloor())
		{
			_velocity.Y -= FallAcceleration * (float)delta;
		}

		Velocity = _velocity;
		MoveAndSlide();
	}
	
	// Method for throwing object
	// Object needs to be thrown in an arc to a location.
	
	
	// Components needed:
	// Health
	// Movespeed
	
	
	// Class Friend:
	// 
	
}
