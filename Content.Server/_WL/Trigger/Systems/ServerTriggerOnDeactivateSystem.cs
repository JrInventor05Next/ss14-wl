using Content.Server.Defusable.Systems;
using Content.Shared._WL.Trigger.Components;
using Content.Shared._WL.Trigger.Systems;

namespace Content.Server._WL.Trigger.Systems;

public sealed class ServerTriggerOnDeactivateSystem : SharedTriggerOnDeactivateSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TriggerOnDeactivateComponent, BombDetonatedEvent>(OnDetonate);
        SubscribeLocalEvent<TriggerOnDeactivateComponent, BombDefusedEvent>(OnDefuse);
    }

    private void OnDetonate(Entity<TriggerOnDeactivateComponent> ent, ref BombDetonatedEvent args)
    {
        if (!ent.Comp.IsActivated) return;

        if (Trigger.Trigger(ent, ent.Comp.User))
        {
            if (ent.Comp.User is { } owner)
                RemCompDeferred<DeadManComponent>(owner);
            ent.Comp.IsActivated = false;
            ent.Comp.User = null;
            Dirty(ent);
        }
    }

    private void OnDefuse(Entity<TriggerOnDeactivateComponent> ent, ref BombDefusedEvent args)
    {
        if (!ent.Comp.IsActivated) return;

        ent.Comp.IsActivated = false;
        if (ent.Comp.User is { } owner)
            RemCompDeferred<DeadManComponent>(owner);
        ent.Comp.User = null;
        Dirty(ent);
    }
}
