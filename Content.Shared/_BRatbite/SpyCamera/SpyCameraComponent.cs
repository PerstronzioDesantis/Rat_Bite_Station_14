using Robust.Shared.GameStates;

namespace Content.Shared._BRatbite.SpyCamera;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SpyCameraComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public Enum? UiKey;

    [ViewVariables, AutoNetworkedField]
    public EntityUid? TargetEntity;
}
