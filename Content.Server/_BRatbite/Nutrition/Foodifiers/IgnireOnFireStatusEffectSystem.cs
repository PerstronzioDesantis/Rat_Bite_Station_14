using Content.Server.Atmos.EntitySystems;
using Content.Shared._BRatbite.Nutrition.Foodifiers;
using Content.Shared.Atmos.Components;
using Content.Shared.StatusEffectNew;

namespace Content.Server._BRatbite.Nutrition.Foodifiers;

public sealed class IgniteOnFireStatusEffectSystem : SharedIgniteOnFireStatusEffectSystem
{
    [Dependency] private readonly FlammableSystem _flammable = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<IgniteOnFireStatusEffectComponent, StatusEffectAppliedEvent>(OnEffectApplied);
        SubscribeLocalEvent<IgniteOnFireStatusEffectComponent, StatusEffectRemovedEvent>(OnEffectRemoved);
    }

    private void OnEffectApplied(Entity<IgniteOnFireStatusEffectComponent> entity, ref StatusEffectAppliedEvent args)
    {
        if(!TryComp<FlammableComponent>(args.Target, out var flammableComponent))
            return;

        if (entity.Comp.StopExtinguish)
        {
            entity.Comp.CouldExtinguish = flammableComponent.CanExtinguish;
            flammableComponent.CanExtinguish = false;
        }

        if (entity.Comp.MinFireStacksToSet != null)
        {
            entity.Comp.PreviousMinFireStacks = flammableComponent.MinimumFireStacks;
            flammableComponent.MinimumFireStacks = entity.Comp.MinFireStacksToSet.Value;
        }

        _flammable.AdjustFireStacks(args.Target, entity.Comp.FireStacksToAdd, null, true);
    }

    private void OnEffectRemoved(Entity<IgniteOnFireStatusEffectComponent> entity, ref StatusEffectRemovedEvent args)
    {
        if(!TryComp<FlammableComponent>(args.Target, out var flammableComponent))
            return;

        if (entity.Comp.CouldExtinguish != null)
            flammableComponent.CanExtinguish = entity.Comp.CouldExtinguish.Value;

        if (entity.Comp.PreviousMinFireStacks != null)
            flammableComponent.MinimumFireStacks = entity.Comp.PreviousMinFireStacks.Value;
    }
}
