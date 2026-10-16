using Content.Shared.StatusEffectNew;
using Robust.Shared.Random;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

public sealed partial class TemporaryGrantRandomComponentStatusEffectSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TemporaryGrantRandomComponentStatusEffectComponent, StatusEffectAppliedEvent>(OnApply);
        SubscribeLocalEvent<TemporaryGrantRandomComponentStatusEffectComponent, StatusEffectRemovedEvent>(OnRemoved);
    }

    private void OnRemoved(Entity<TemporaryGrantRandomComponentStatusEffectComponent> ent, ref StatusEffectRemovedEvent args)
    {
        if (ent.Comp.Picked is not null)
            EntityManager.RemoveComponents(args.Target, ent.Comp.Picked);
    }

    private void OnApply(Entity<TemporaryGrantRandomComponentStatusEffectComponent> ent, ref StatusEffectAppliedEvent args)
    {
        var picked = _random.Pick(ent.Comp.Possibilities);
        EntityManager.AddComponents(args.Target, picked.Components);
        ent.Comp.Picked = picked.Components;
    }
}
