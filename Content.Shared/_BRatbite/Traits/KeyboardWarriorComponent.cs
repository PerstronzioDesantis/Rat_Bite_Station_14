using Content.Shared.Dataset;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._BRatbite.Traits;

[RegisterComponent, NetworkedComponent]
public sealed partial class KeyboardWarriorComponent : Component
{
    public override bool SendOnlyToOwner => true;

    [DataField]
    public TimeSpan BaseTimeBetweenMessages = TimeSpan.FromSeconds(8);

    [DataField]
    public ProtoId<LocalizedDatasetPrototype> Dataset = "MaldDataset";

    [DataField]
    public float MessageStandardDeviation = 2;
}
