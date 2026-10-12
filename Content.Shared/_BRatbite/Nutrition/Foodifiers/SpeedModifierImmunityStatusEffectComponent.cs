namespace Content.Shared._BRatbite.Nutrition.Foodifiers;

[RegisterComponent]
// I have no idea on how to call this, but it reduces slow downs from
// sources, for example if the multiplier is 0.6 and you are 30% slown
// down, you would be slown down by 30% * 0.6 = 18%
public sealed partial class SpeedModifierImmunityStatusEffectComponent : Component
{
    [DataField(required: true)]
    public float Multiplier;
}
