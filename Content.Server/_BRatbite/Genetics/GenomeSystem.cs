using Content.Server.GameTicking.Events;
using Content.Shared._BRatbite.Genetics;
using Content.Shared.GameTicking;
using System.Linq;

namespace Content.Server._BRatbite.Genetics;

public sealed partial class GenomeSystem : EntitySystem
{
    [Dependency] private readonly GenomeManager _genomeManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundStartingEvent>(OnRoundStart);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundEnd);
        SubscribeLocalEvent<GenomeComponent, MapInitEvent>(OnMapInit);
    }

    private void OnRoundStart(RoundStartingEvent args)
    {
        _genomeManager.LoadGenes();
    }

    private void OnRoundEnd(RoundRestartCleanupEvent args)
    {
        _genomeManager.UnloadGenes();
    }

    private void OnMapInit(Entity<GenomeComponent> ent, ref MapInitEvent args)
    {
        var randomGenes = _genomeManager.GetRandomGenes(ent.Comp.NumberOfGenes);
        var genes = randomGenes.Select(r => CorruptGene(new Gene
        {
            Number = r.Number,
            Dna = [.. r.Dna],
        }, r.GenePrototype));
        ent.Comp.BaseGenome = genes.ToList();
        // TODO: need to deep clone
        ent.Comp.CurrentGenome = genes.ToList();

    }
    private Gene CorruptGene(Gene gene, GenePrototype genePrototype)
    {
        // TODO
        return gene;
    }
}
