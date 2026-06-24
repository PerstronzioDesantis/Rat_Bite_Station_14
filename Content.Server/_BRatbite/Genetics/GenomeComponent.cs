using Content.Shared._BRatbite.Genetics;

namespace Content.Server._BRatbite.Genetics;

[RegisterComponent]
public sealed partial class GenomeComponent : Component
{
    [DataField]
    // This is the base genome of when the entity starts, we need a reference to it
    // So we can reset it when injecting Mutadone
    public List<Gene> BaseGenome = new();

    [DataField]
    public List<Gene> CurrentGenome = new();

    [DataField]
    public int NumberOfGenes = 6;
}

[Serializable]
public sealed class Gene
{
    // This is the gene number. It is generated at the start of the
    // round and is shared with every entity (i.e., if the Monkified gene is
    // 5 for an entity, it will be 5 for every entity).
    // Use GenomeManager to fetch info about this gene, like the name or effects
    [ViewVariables]
    public int Number;
    [ViewVariables]
    public required (NucleoBase, NucleoBase)[] Dna { get; init; }
    [ViewVariables]
    public bool Activated = false;
}
