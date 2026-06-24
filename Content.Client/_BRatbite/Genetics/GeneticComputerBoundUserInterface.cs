using Content.Shared._BRatbite.Genetics;
using Robust.Client.UserInterface;

namespace Content.Client._BRatbite.Genetics;

public sealed partial class GeneticComputerBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private GeneticComputerWindow? _window;

    public GeneticComputerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<GeneticComputerWindow>();
        _window.OnClose += () => _window = null;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (_window == null || state is not GeneticComputerScanInfoInterface gasMinerState)
            return;
        _window.Populate(gasMinerState);
    }
}
