using Robust.Shared.Prototypes;

namespace Content.Shared._BRatbite.Genetics;

[Prototype]
public sealed partial class GenePrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    // The components added or removed when the gene is activated
    [DataField]
    public ComponentRegistry Components { get; private set; } = default!;
}
