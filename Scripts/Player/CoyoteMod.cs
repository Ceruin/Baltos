using Godot;
using System;
using System.Diagnostics;

public class CoyoteMod
{
    #region Fields
    protected bool canJump = true;
    protected TimeSpan coyoteTime = new TimeSpan(0, 0, 0, 0, 300);
    protected Stopwatch coyoteTimer = new Stopwatch();
    #endregion Fields
    // Coyote time duration in seconds

    #region Properties
    public bool CanJump => canJump;
    #endregion Properties

    #region Methods

    public void Process(CharacterBody3D character)
    {
        //GD.Print($@"Can: {canJump} + Coyote: {coyoteTimer.Elapsed} + {coyoteTimer.Elapsed != TimeSpan.Zero} + {coyoteTimer.Elapsed < coyoteTime}");
        // Implement Coyote Time
        if (character.IsOnFloor())
        {
            Start();
        }
        else if (canJump && coyoteTimer.Elapsed == TimeSpan.Zero)
        {
            Start2();
        }
        else if (canJump && coyoteTimer.Elapsed != TimeSpan.Zero && coyoteTimer.Elapsed <= coyoteTime)
        {
            // do nothing
        }
        else
        {
            Reset();
        }
        //GD.Print($@"Coyote: {coyoteTimer.Elapsed}");
    }

    public void Reset()
    {
        canJump = false;
        coyoteTimer.Reset();
    }

    public void Start()
    {
        coyoteTimer.Reset();
        canJump = true;
        //GD.Print($@"Coyote Started");
    }

    public void Start2()
    {
        coyoteTimer.Start();
        canJump = true;
        //GD.Print($@"Coyote Jump");
        //GD.Print($@"Coyote: {coyoteTimer.Elapsed}");
    }

    #endregion Methods
}