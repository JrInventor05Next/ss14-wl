using Content.Shared.Trigger.Components.Triggers;
using Robust.Shared.GameStates;

namespace Content.Shared._WL.Trigger.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TriggerOnDeactivateComponent : BaseTriggerOnXComponent
{
    [ViewVariables, AutoNetworkedField]
    public bool IsTransferring = false;

    [ViewVariables, AutoNetworkedField]
    public bool IsActivated = false;

    [ViewVariables, AutoNetworkedField]
    public EntityUid? User;
}
