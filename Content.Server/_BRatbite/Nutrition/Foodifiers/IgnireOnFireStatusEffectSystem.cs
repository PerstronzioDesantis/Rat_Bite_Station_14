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

        entity.Comp.CouldExtinguish = flammableComponent.CanExtinguish;
        flammableComponent.CanExtinguish = !entity.Comp.StopExtinguish;

        flammableComponent.FireStacks += entity.Comp.FireStacksToAdd;
        _flammable.Ignite(args.Target);
    }

    private void OnEffectRemoved(Entity<IgniteOnFireStatusEffectComponent> entity, ref StatusEffectRemovedEvent args)
    {
        if(!TryComp<FlammableComponent>(args.Target, out var flammableComponent))
            return;

        flammableComponent.CanExtinguish = entity.Comp.CouldExtinguish;
    }
}
