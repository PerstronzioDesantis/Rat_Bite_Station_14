using Content.Shared.Atmos.Components;
using Robust.Shared.GameStates;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

/// <summary>
/// Ignites the entity on fire (and can keep them on fire too)
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class IgniteOnFireStatusEffectComponent : Component
{
    /// <summary>
    /// How much fire stacks to add to the entity when they get the effect
    /// </summary>
    [DataField]
    public float FireStacksToAdd;

    /// <summary>
    /// The minimal fire stacks the entity can have while the effect is active
    /// </summary>
    /// <remarks>if null, it doesn't change the minimal fire stacks of the entity</remarks>
    [DataField]
    public float? MinFireStacksToSet;

    /// <summary>
    /// If the effect stops the entity from being extinguished.
    /// </summary>
    [DataField]
    public bool StopExtinguish;

    /// <summary>
    /// The state of <see cref="FlammableComponent.CanExtinguish"/> before the effect was applied.
    /// </summary>
    /// <remarks>used to return to the same state after the effect is gone</remarks>
    [ViewVariables]
    public bool? CouldExtinguish;

    /// <summary>
    /// The value of <see cref="FlammableComponent.MinimumFireStacks"/> before the effect was applied.
    /// </summary>
    /// <remarks>used to return to the same value after the effect is gone</remarks>
    [ViewVariables]
    public float? PreviousMinFireStacks;
}
