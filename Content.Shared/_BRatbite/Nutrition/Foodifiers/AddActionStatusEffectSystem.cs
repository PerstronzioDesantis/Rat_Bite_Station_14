using Content.Shared.Actions;
using Content.Shared.StatusEffectNew;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

public sealed class AddActionStatusEffectSystem : EntitySystem
{
    [Dependency] private SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AddActionStatusEffectComponent, StatusEffectAppliedEvent>(OnStatusEffectApplied);
        SubscribeLocalEvent<AddActionStatusEffectComponent, StatusEffectRemovedEvent>(OnStatusEffectRemoved);
    }

    private void OnStatusEffectApplied(Entity<AddActionStatusEffectComponent> entity, ref StatusEffectAppliedEvent args)
    {
        _actions.AddAction(args.Target, ref entity.Comp.ActionEntity, entity.Comp.Action);
    }

    private void OnStatusEffectRemoved(Entity<AddActionStatusEffectComponent> entity, ref StatusEffectRemovedEvent args)
    {
        _actions.RemoveAction(args.Target, entity.Comp.ActionEntity);
    }
}
