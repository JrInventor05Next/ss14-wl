using Content.Shared._WL.Trigger.Components;
using Content.Shared.Examine;
using Content.Shared.Hands;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Trigger;
using Content.Shared.Verbs;
using Content.Shared.Wires;

namespace Content.Shared._WL.Trigger.Systems;

public sealed partial class SharedTriggerOnDeactivateSystem : TriggerOnXSystem
{
    [Dependency] private MobStateSystem _state = default!;
    [Dependency] private SharedHandsSystem _hands = default!;
    [Dependency] private SharedWiresSystem _wiresSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TriggerOnDeactivateComponent, ComponentShutdown>(OnShutdownTrigger);
        SubscribeLocalEvent<DeadManComponent, ComponentShutdown>(OnShutdownMan);
        SubscribeLocalEvent<TriggerOnDeactivateComponent, GetVerbsEvent<AlternativeVerb>>(OnGetAltVerbs);
        SubscribeLocalEvent<TriggerOnDeactivateComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<TriggerOnDeactivateComponent, GotUnequippedHandEvent>(OnGotUnequippedHandEvent);
        SubscribeLocalEvent<TriggerOnDeactivateComponent, GotEquippedHandEvent>(OnGotEquippedHandEvent);
        SubscribeLocalEvent<DeadManComponent, MobStateChangedEvent>(OnStateChanged);
    }

    private void OnShutdownTrigger(Entity<TriggerOnDeactivateComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.User is { } user)
            RemComp<DeadManComponent>(user);
    }

    private void OnShutdownMan(Entity<DeadManComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.Trigger is { } trigger && TryComp<TriggerOnDeactivateComponent>(trigger, out var triggerComp))
        {
            triggerComp.User = null;
            Dirty(trigger, triggerComp);
        }
    }

    private void OnGetAltVerbs(Entity<TriggerOnDeactivateComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Hands == null)
            return;

        var user = args.User;

        if (_hands.GetActiveItem(user) != ent)
            return;

        if (!ent.Comp.IsActivated)
            args.Verbs.Add(new AlternativeVerb
            {
                Text = Loc.GetString("trigger-on-deactivate-trigger-activate"),
                Act = () =>
                {
                    ent.Comp.IsActivated = true;
                    var deadMan = EnsureComp<DeadManComponent>(user);
                    deadMan.Trigger = ent;
                    ent.Comp.User = user;
                    Dirty(ent);
                    Dirty(user, deadMan);
                },
                Priority = 2
            });
        else
        {
            args.Verbs.Add(new AlternativeVerb
            {
                Text = Loc.GetString("trigger-on-deactivate-trigger-release"),
                Act = () => Release(ent, true),
                Priority = 2
            });
        }
    }

    private void OnExamine(Entity<TriggerOnDeactivateComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("trigger-on-deactivate-examine", ("activated", ent.Comp.IsActivated)));
    }

    private void OnGotUnequippedHandEvent(Entity<TriggerOnDeactivateComponent> ent, ref GotUnequippedHandEvent args) => Release(ent);

    private void OnGotEquippedHandEvent(Entity<TriggerOnDeactivateComponent> ent, ref GotEquippedHandEvent args) => Release(ent);

    private void OnStateChanged(Entity<DeadManComponent> ent, ref MobStateChangedEvent args)
    {
        if (_state.IsIncapacitated(ent) && ent.Comp.Trigger is { } trigger && HasComp<TriggerOnDeactivateComponent>(trigger))
            Trigger.Trigger(trigger, ent);
    }

    public void Release(Entity<TriggerOnDeactivateComponent> ent, bool skipTransfer = false)
    {
        if (!ent.Comp.IsActivated || ent.Comp.IsTransferring && !skipTransfer) return;

        if (Trigger.Trigger(ent))
        {
            if (ent.Comp.User is { } user && Exists(user))
                RemComp<DeadManComponent>(user);
            ent.Comp.IsActivated = false;
            ent.Comp.User = null;
            Dirty(ent);
            if (TryComp<WiresPanelComponent>(ent, out var wiresPanelComponent))
                _wiresSystem.TogglePanel(ent, wiresPanelComponent, false);
        }
    }

    public void Activate(Entity<TriggerOnDeactivateComponent> ent, EntityUid? user = null)
    {
        if (ent.Comp.IsActivated) return;

        ent.Comp.IsActivated = true;
        ent.Comp.User = user;
        Dirty(ent);
        if (user is { } target && Exists(target))
        {
            var comp = EnsureComp<DeadManComponent>(target);
            comp.Trigger = ent;
            Dirty(target, comp);
        }
    }

    public void Deactivate(Entity<TriggerOnDeactivateComponent> ent)
    {
        if (!ent.Comp.IsActivated) return;

        ent.Comp.IsActivated = false;
        if (ent.Comp.User is { } user && Exists(user))
            RemComp<DeadManComponent>(user);
        ent.Comp.User = null;
        Dirty(ent);
    }

    public void Transfer(Entity<TriggerOnDeactivateComponent?> ent, bool close = false)
    {
        if (!Resolve(ent, ref ent.Comp, false)) return;
        ent.Comp.IsTransferring = !close;
        Dirty(ent);
    }
    public void EndTransfer(Entity<TriggerOnDeactivateComponent?> ent, EntityUid target)
    {
        if (!Resolve(ent, ref ent.Comp, false)) return;

        if (ent.Comp.User is { } user && Exists(user))
            RemComp<DeadManComponent>(user);
        var dmComponent = EnsureComp<DeadManComponent>(target);
        dmComponent.Trigger = ent;
        Dirty(target, dmComponent);
        ent.Comp.User = target;
        ent.Comp.IsTransferring = false;
        Dirty(ent);
    }
}
