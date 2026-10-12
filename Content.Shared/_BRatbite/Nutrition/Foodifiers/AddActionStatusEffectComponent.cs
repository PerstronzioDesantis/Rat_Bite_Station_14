using Content.Shared.Actions.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

/// <summary>
/// Adds a action when the effect is applied
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class AddActionStatusEffectComponent : Component
{
    /// <summary>
    /// The action proto to be added
    /// </summary>
    [DataField]
    public EntProtoId<ActionComponent> Action;

    /// <summary>
    /// Reference to the action entity
    /// </summary>
    [ViewVariables]
    public EntityUid? ActionEntity;
}
