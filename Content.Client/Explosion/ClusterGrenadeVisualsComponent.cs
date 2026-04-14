namespace Content.Client.Explosion;

[RegisterComponent]
[Access(typeof(ClusterGrenadeVisualizerSystem))]
public sealed partial class ClusterGrenadeVisualsComponent : Component
{
    [DataField]
    public string? State;
}
