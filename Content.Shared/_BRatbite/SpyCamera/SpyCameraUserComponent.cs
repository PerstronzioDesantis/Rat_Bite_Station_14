using Robust.Shared.GameStates;

namespace Content.Shared._BRatbite.SpyCamera;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SpyCameraUserComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public List<EntityUid> OpenCameras = new();
}
