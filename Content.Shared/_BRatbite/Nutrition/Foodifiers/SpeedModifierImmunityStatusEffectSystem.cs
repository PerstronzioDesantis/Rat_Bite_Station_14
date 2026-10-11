using Content.Shared._BRatbite.Nutrition.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.StatusEffectNew;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

public sealed partial class SpeedModifierImmunityStatusEffectSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifierSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SpeedModifierImmunityStatusEffectComponent, StatusEffectRelayedEvent<RefreshMovementSpeedModifiersEvent>>(OnRefreshMovementSpeed);
        SubscribeLocalEvent<SpeedModifierImmunityStatusEffectComponent, StatusEffectAppliedEvent>((ent, ref args) => _movementSpeedModifierSystem.RefreshMovementSpeedModifiers(args.Target));
        SubscribeLocalEvent<SpeedModifierImmunityStatusEffectComponent, StatusEffectRemovedEvent>((ent, ref args) => _movementSpeedModifierSystem.RefreshMovementSpeedModifiers(args.Target));
        SubscribeLocalEvent<SpeedModifierImmunityStatusEffectComponent, StatusEffectScaleEvent>((ent, ref args) => _movementSpeedModifierSystem.RefreshMovementSpeedModifiers(args.Target));
        SubscribeLocalEvent<SpeedModifierImmunityStatusEffectComponent, EffectDescriptionEvent>(OnGetDescription);
    }

    private void OnRefreshMovementSpeed(Entity<SpeedModifierImmunityStatusEffectComponent> ent, ref StatusEffectRelayedEvent<RefreshMovementSpeedModifiersEvent> args)
    {
        var scale = CompOrNull<StatusEffectScaleComponent>(ent)?.Scale ?? 1f;
        if (args.Args.SprintSpeedModifier < 1)
            args.Args.SprintSpeedModifier = (1 - args.Args.SprintSpeedModifier) * ent.Comp.Multiplier * scale + 1;
        if (args.Args.WalkSpeedModifier < 1)
            args.Args.WalkSpeedModifier = (1 - args.Args.WalkSpeedModifier) * ent.Comp.Multiplier * scale + 1;
    }

    private void OnGetDescription(Entity<SpeedModifierImmunityStatusEffectComponent> ent, ref EffectDescriptionEvent args)
    {
        args.Message.AddMarkupOrThrow(Loc.GetString("guidebook-description-speed-immunity", ("multiplier", MathF.Round(100 - ent.Comp.Multiplier * 100))));
        args.Message.PushNewline();
    }
}
