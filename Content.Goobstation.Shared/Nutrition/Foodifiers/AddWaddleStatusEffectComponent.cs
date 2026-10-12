using System.Numerics;
using Robust.Shared.GameStates;
using Content.Goobstation.Shared.Waddle;

namespace Content.Goobstation.Shared.Nutrition.Foodifiers;

/// <summary>
/// Makes the entity waddle when the effect is applied
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class AddWaddleStatusEffectComponent : Component
{
    /// <summary>
    /// <see cref="WaddleAnimationComponent.HopIntensity"/>
    /// </summary>
    [DataField]
    public Vector2? HopIntensity;

    /// <summary>
    /// <see cref="WaddleAnimationComponent.TumbleIntensity"/>
    /// </summary>
    [DataField]
    public float? TumbleIntensity;

    /// <summary>
    /// <see cref="WaddleAnimationComponent.AnimationLength"/>
    /// </summary>
    [DataField]
    public TimeSpan? AnimationLength;

    /// <summary>
    /// <see cref="WaddleAnimationComponent.RunAnimationLengthMultiplier"/>
    /// </summary>
    [DataField]
    public float? RunAnimationLengthMultiplier;

    /// <summary>
    /// Used to prevent double-removing WaddleAnimation, e.g. if you have a species that waddles.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool AddedWaddle;
}
