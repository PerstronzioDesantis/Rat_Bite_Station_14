using Content.Shared.UserInterface;
using Robust.Shared.Timing;

namespace Content.Shared._BRatbite.Genetics;

public sealed partial class GeneticComputerSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _userInterfaceSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GeneticComputerComponent, BeforeActivatableUIOpenEvent>(OnBeforeOpened);
    }

    private void OnBeforeOpened(Entity<GeneticComputerComponent> ent, ref BeforeActivatableUIOpenEvent args)
    {
        DirtyUI(ent);
    }

    private void DirtyUI(Entity<GeneticComputerComponent> ent)
    {
        if (_timing.IsFirstTimePredicted) return;
        var dummyState = new GeneticComputerScanInfoInterface(
            subjectName: "John Doe",
            subjectStatus: "Stable",
            geneticDamage: 0.123f,
            printCooldown: TimeSpan.FromSeconds(15),
            sequencerStatus: new SequencerStatus(
                new List<Sequence>
                {
            new(
                number: 1,
                info: "Hulk",
                activated: true,
                dna: new (NucleoBase, NucleoBase)[]
                {
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.X, NucleoBase.X),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.X, NucleoBase.G),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.X, NucleoBase.C),
                }),

            new(
                number: 2,
                info: "Fire Resistance",
                activated: false,
                dna: new (NucleoBase, NucleoBase)[]
                {
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.X, NucleoBase.T),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.X, NucleoBase.X),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.X, NucleoBase.A),
                    (NucleoBase.G, NucleoBase.C),
                }),

            new(
                number: 3,
                info: "Telekinesis",
                activated: false,
                dna: new (NucleoBase, NucleoBase)[]
                {
                    (NucleoBase.X, NucleoBase.G),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.X, NucleoBase.X),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.X, NucleoBase.C),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.X, NucleoBase.A),
                }),

            new(
                number: 4,
                info: "Regeneration",
                activated: true,
                dna: new (NucleoBase, NucleoBase)[]
                {
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.X, NucleoBase.X),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.X, NucleoBase.G),
                    (NucleoBase.C, NucleoBase.G),
                    (NucleoBase.G, NucleoBase.C),
                    (NucleoBase.A, NucleoBase.T),
                    (NucleoBase.T, NucleoBase.A),
                    (NucleoBase.X, NucleoBase.C),
                    (NucleoBase.C, NucleoBase.G),
                })
                },
                selectedSequenceId: 1
            )
        );
        _userInterfaceSystem.SetUiState(ent.Owner, GeneticComputerUiKey.Key, dummyState);
    }
}
