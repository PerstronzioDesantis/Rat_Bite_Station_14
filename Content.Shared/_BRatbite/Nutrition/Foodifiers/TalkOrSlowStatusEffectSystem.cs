using Content.Shared.Chat;
using Content.Shared.CrewManifest;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

public sealed class TalkOrSlowStatusEffectSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly StatusEffectNew.StatusEffectsSystem _statusEffects = default!;

    private static readonly EntProtoId DebuffProtoId = "TalkOrSlowDebuff";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TalkOrSlowStatusEffectComponent, StatusEffectAppliedEvent>(OnInit);
        SubscribeLocalEvent<TalkOrSlowStatusEffectComponent, StatusEffectRelayedEvent<EntitySpokeEvent>>(OnEntitySpoke);
    }

    private void OnEntitySpoke(Entity<TalkOrSlowStatusEffectComponent> ent, ref StatusEffectRelayedEvent<EntitySpokeEvent> args)
    {
        ent.Comp.Avoided = true;
        Dirty(ent);
    }

    private void OnInit(Entity<TalkOrSlowStatusEffectComponent> ent, ref StatusEffectAppliedEvent args)
    {
        ent.Comp.AppliedTo = args.Target;
        ent.Comp.NextCheck = _timing.CurTime + ent.Comp.Interval;
        Dirty(ent);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<TalkOrSlowStatusEffectComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (_timing.CurTime - comp.NextCheck < TimeSpan.FromSeconds(0))
                continue;
            if (!comp.Avoided)
                _statusEffects.TryAddStatusEffectDuration(comp.AppliedTo, DebuffProtoId, TimeSpan.FromSeconds(comp.DebuffDuration));
            comp.Avoided = false;
            comp.NextCheck = _timing.CurTime + comp.Interval;
            Dirty(uid, comp);
        }
    }
}
