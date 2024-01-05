using Godot;

public static class InputMod
{
    #region Properties

    public static bool Down =>
         Input.IsActionPressed(PlayerStrings.move_down);

    public static bool Jump =>
        Input.IsActionJustPressed(PlayerStrings.jump);

    public static bool Left =>
         Input.IsActionPressed(PlayerStrings.move_left);

    public static bool Right =>
        Input.IsActionPressed(PlayerStrings.move_right);

    public static bool Up =>
                         Input.IsActionPressed(PlayerStrings.move_up);

    #endregion Properties
}