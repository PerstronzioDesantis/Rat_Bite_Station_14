using Content.Shared.Alert;
using Robust.Shared.Prototypes;

namespace Content.Shared._BRatbite.Movement;

[RegisterComponent, AutoGenerateComponentState, Access(typeof(MagnetizedSystem))]
public sealed partial class MagnetizedComponent : Component
{
    [DataField]
    public ProtoId<AlertPrototype> MagnetizedAlert = "Magboots";

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public Dictionary<EntityUid, bool> Sources = new(); // <Source, requiresGrid> Avoid prematurely removing magnetism if there are multiple things controlling.
}
