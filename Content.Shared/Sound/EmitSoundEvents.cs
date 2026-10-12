using Content.Shared.Actions;
using Robust.Shared.Audio;

namespace Content.Shared.Sound;

public sealed partial class EmitSoundActionEvent : InstantActionEvent
{
    [DataField]
    public SoundSpecifier Sound =
        new SoundCollectionSpecifier(collection: "BikeHorn");
}
