using Content.Shared.Alert;
using Content.Shared.Atmos.Components;
using Content.Shared.CrewManifest;
using Content.Shared.Gravity;

namespace Content.Shared._BRatbite.Movement;

public sealed partial class MagnetizedSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly SharedGravitySystem _gravity = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MagnetizedComponent, IsWeightlessEvent>(OnIsWeightless);
        SubscribeLocalEvent<MagnetizedComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<MagnetizedEntityComponent, ComponentStartup>(OnMagnetizedEntityStartup);
        SubscribeLocalEvent<MagnetizedEntityComponent, ComponentRemove>(OnMagnetizedEntityRemoved);
    }

    private void OnMagnetizedEntityRemoved(Entity<MagnetizedEntityComponent> ent, ref ComponentRemove args)
    {
        RemoveSource(ent, ent);
    }

    private void OnMagnetizedEntityStartup(Entity<MagnetizedEntityComponent> ent, ref ComponentStartup args)
    {
        AddSource(ent, ent, false);
    }

    private void OnShutdown(Entity<MagnetizedComponent> ent, ref ComponentShutdown args)
    {
        ent.Comp.Sources.Clear();
        UpdateEffects(ent.Owner, ent.Comp, false);
    }


    public void AddSource(EntityUid source, EntityUid target, bool requiresGrid = true)
    {
        var comp = EnsureComp<MagnetizedComponent>(target);
        var first = comp.Sources.Count == 0;

        comp.Sources.Add(source, requiresGrid);

        if (first)
            UpdateEffects(target, comp, true);
        else
            _gravity.RefreshWeightless(target);
    }

    public void RemoveSource(EntityUid source, EntityUid target)
    {
        if (!TryComp<MagnetizedComponent>(source, out var comp) || !comp.Sources.Remove(source))
            return;

        if (comp.Sources.Count == 0)
        {
            RemCompDeferred<MagnetizedComponent>(target);
            return;
        }

        Dirty(target, comp);
        _gravity.RefreshWeightless(target);
    }

    private void UpdateEffects(EntityUid user, MagnetizedComponent comp, bool state)
    {
        // TODO: public api for this and add access
        if (TryComp<MovedByPressureComponent>(user, out var moved))
            moved.Enabled = !state;

        _gravity.RefreshWeightless(user);

        if (state)
            _alerts.ShowAlert(user, comp.MagnetizedAlert);
        else
            _alerts.ClearAlert(user, comp.MagnetizedAlert);
    }

    private void OnIsWeightless(Entity<MagnetizedComponent> ent, ref IsWeightlessEvent args)
    {
        if (args.Handled || ent.Comp.Sources.Count == 0)
            return;

        var requiresGrid = ent.Comp.Sources.ContainsValue(false);
        if (requiresGrid && !_gravity.EntityOnGravitySupportingGridOrMap(ent.Owner))
            return;

        args.IsWeightless = false;
        args.Handled = true;
    }
}
