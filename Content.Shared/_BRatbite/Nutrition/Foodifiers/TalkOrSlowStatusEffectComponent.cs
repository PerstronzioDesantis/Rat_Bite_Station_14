using Robust.Shared.GameStates;

namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

[RegisterComponent, NetworkedComponent]
public sealed partial class TalkOrSlowStatusEffectComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid AppliedTo = EntityUid.Invalid;

    [DataField]
    public int DebuffDuration = 10;

    [DataField]
    public TimeSpan Interval = TimeSpan.FromSeconds(10);

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextCheck = TimeSpan.Zero;

    [ViewVariables(VVAccess.ReadOnly)]
    public bool Avoided;
}
