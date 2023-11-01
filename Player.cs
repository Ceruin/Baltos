using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed { get; set; } = 14;
    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Vector3 _velocity = Vector3.Zero;

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

        if (!IsOnFloor())
        {
            _velocity.Y -= FallAcceleration * (float)delta;
        }

        Velocity = _velocity;
        MoveAndSlide();
    }
}
