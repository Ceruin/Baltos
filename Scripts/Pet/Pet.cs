using Godot;

public partial class Pet : CharacterBody3D
{
    #region Fields
    private GrabbableMod Grab = new GrabbableMod();
    private HealthMod HP = new HealthMod();
    private HappinessMod Joy = new HappinessMod();
    #endregion Fields
}