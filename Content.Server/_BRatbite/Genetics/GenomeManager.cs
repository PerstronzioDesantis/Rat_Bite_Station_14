using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared._BRatbite.Genetics;
using System.Diagnostics;

namespace Content.Server._BRatbite.Genetics;

public sealed partial class GenomeManager
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    private CompleteGene[] _genes = new CompleteGene[0];
    public readonly static int DnaLength = 16;


    public void Initialize()
    {
    }

    // Call this at the start of the round, before any genes are initialized
    public void LoadGenes()
    {
        Debug.Assert(_genes.Length == 0, "LoadGenes was called without being uninitialized first");
        var genes = _proto.EnumeratePrototypes<GenePrototype>().ToList();
        _random.Shuffle(genes);
        _genes = Enumerable.Range(0, genes.Count).Select((i) =>
        {
            return new CompleteGene
            {
                Dna = GenerateRandomDNA(),
                GenePrototype = genes[i],
                Number = i,
            };
        }).ToArray();
    }

    public void UnloadGenes()
    {
        _genes = new CompleteGene[0];
    }

    public CompleteGene? GetCompleteGene(int number)
    {
        Debug.Assert(_genes.Length != 0, "GetCompleteGene was called without being initialized");
        return _genes.ElementAtOrDefault(number);
    }

    public CompleteGene[] GetRandomGenes(int count)
    {
        Debug.Assert(count <= _genes.Length, "Trying to get more random genes than available");
        return _random.GetItems(_genes, count, allowDuplicates: false);
    }

    private (NucleoBase, NucleoBase)[] GenerateRandomDNA()
    {
        return Enumerable.Range(0, DnaLength).Select((i) =>
        {
            var nucleoBase = (NucleoBase) _random.Next(0, 4);
            return (nucleoBase, nucleoBase.GetOpposite());
        }).ToArray();
    }
}

public sealed class CompleteGene
{
    // This is the actual DNA of the complete gene
    // This is not allowed to contain any X's
    public required (NucleoBase, NucleoBase)[] Dna { get; init; }
    public required int Number;

    public required GenePrototype GenePrototype { get; init; }
}
