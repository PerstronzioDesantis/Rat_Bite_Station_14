using Robust.Shared.Prototypes;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

[RegisterComponent]
public sealed partial class TemporaryGrantRandomComponentStatusEffectComponent : Component
{
    [DataField(required: true)]
    public List<WrappedComponentRegistry> Possibilities = [];

    [ViewVariables(VVAccess.ReadOnly)]
    public ComponentRegistry? Picked;
}

[DataDefinition]
public sealed partial class WrappedComponentRegistry // For yaml syntax's sake
{
    [DataField(required: true)]
    public ComponentRegistry Components = new();
}
