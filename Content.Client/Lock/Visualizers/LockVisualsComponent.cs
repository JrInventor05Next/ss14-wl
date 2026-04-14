namespace Content.Client.Lock.Visualizers;

[RegisterComponent]
[Access(typeof(LockVisualizerSystem))]
public sealed partial class LockVisualsComponent : Component
{
    /// <summary>
    /// The RSI state used for the lock indicator while the entity is locked.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public string? StateLocked = "locked";

    /// <summary>
    /// The RSI state used for the lock indicator entity is unlocked.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public string? StateUnlocked = "unlocked";
}
