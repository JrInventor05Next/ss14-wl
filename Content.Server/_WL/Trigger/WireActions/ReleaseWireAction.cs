using Content.Server.Defusable.Components;
using Content.Server.Defusable.Systems;
using Content.Server.Wires;
using Content.Shared._WL.Trigger.Components;
using Content.Shared._WL.Trigger.Systems;
using Content.Shared.Wires;

namespace Content.Server._WL.Trigger.WireActions;

public sealed partial class ReleaseWireAction : ComponentWireAction<TriggerOnDeactivateComponent>
{
    public override Color Color { get; set; } = Color.Yellow;
    public override string Name { get; set; } = "wire-name-bomb-delay";

    public override bool Cut(EntityUid user, Wire wire, TriggerOnDeactivateComponent comp)
    {
        return true;
    }

    public override bool Mend(EntityUid user, Wire wire, TriggerOnDeactivateComponent comp)
    {
        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, TriggerOnDeactivateComponent comp)
    {
        EntityManager.System<TriggerOnActionSystem>().DelayWirePulse(user, wire, comp);
    }
}
