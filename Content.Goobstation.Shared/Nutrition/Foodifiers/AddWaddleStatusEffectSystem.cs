using Content.Shared.StatusEffectNew;
using Content.Shared._BRatbite.Nutrition;
using Content.Goobstation.Shared.Waddle;

namespace Content.Goobstation.Shared.Nutrition.Foodifiers;

public sealed class AddWaddleStatusEffectSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AddWaddleStatusEffectComponent, StatusEffectAppliedEvent>(OnStatusEffectApplied);
        SubscribeLocalEvent<AddWaddleStatusEffectComponent, StatusEffectRemovedEvent>(OnStatusEffectRemoved);
        SubscribeLocalEvent<AddWaddleStatusEffectComponent, EffectDescriptionEvent>(OnGetDescription);
    }

    private void OnStatusEffectApplied(Entity<AddWaddleStatusEffectComponent> entity, ref StatusEffectAppliedEvent args)
    {
        var user = args.Target;

        if (EnsureComp<WaddleAnimationComponent>(user, out var waddle))
            return;

        entity.Comp.AddedWaddle = true;
        Dirty(entity);

        var comp = entity.Comp;
        if (comp.AnimationLength is {} length)
            waddle.AnimationLength = length;
        if (comp.HopIntensity is {} hopIntensity)
            waddle.HopIntensity = hopIntensity;
        if (comp.TumbleIntensity is {} tumbleIntensity)
            waddle.TumbleIntensity = tumbleIntensity;
        if (comp.RunAnimationLengthMultiplier is {} multiplier)
            waddle.RunAnimationLengthMultiplier = multiplier;

        Dirty(user, waddle);
    }

    private void OnStatusEffectRemoved(Entity<AddWaddleStatusEffectComponent> entity, ref StatusEffectRemovedEvent args)
    {
        if (!entity.Comp.AddedWaddle)
            return;

        RemCompDeferred<WaddleAnimationComponent>(args.Target);
        entity.Comp.AddedWaddle = false;
        Dirty(entity);
    }

    private void OnGetDescription(Entity<AddWaddleStatusEffectComponent> entity, ref EffectDescriptionEvent args)
    {
        args.Message.AddMarkupOrThrow(Loc.GetString("guidebook-description-add-waddle"));
        args.Message.PushNewline();
    }
}
