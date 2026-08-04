using Robust.Shared.GameStates;

namespace Content.Shared._WL.Trigger.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DeadManComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public EntityUid? Trigger;
}
