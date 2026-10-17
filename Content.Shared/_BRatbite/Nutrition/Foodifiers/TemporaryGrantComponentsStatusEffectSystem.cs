using Content.Shared.StatusEffectNew;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

public sealed partial class TemporaryGrantComponentsStatusEffectSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TemporaryGrantComponentsStatusEffectComponent, StatusEffectAppliedEvent>(OnApply);
        SubscribeLocalEvent<TemporaryGrantComponentsStatusEffectComponent, StatusEffectRemovedEvent>(OnRemove);
    }

    private void OnRemove(Entity<TemporaryGrantComponentsStatusEffectComponent> ent, ref StatusEffectRemovedEvent args)
    {
        EntityManager.RemoveComponents(args.Target, ent.Comp.Components);
    }

    private void OnApply(Entity<TemporaryGrantComponentsStatusEffectComponent> ent, ref StatusEffectAppliedEvent args)
    {
        EntityManager.AddComponents(args.Target, ent.Comp.Components);
    }
}
