using Robust.Shared.Serialization;

namespace Content.Shared._BRatbite.Genetics;

[RegisterComponent]
public sealed partial class GeneticComputerComponent : Component
{

}


[Serializable, NetSerializable]
public enum GeneticComputerUiKey
{
    Key
}

[Serializable, NetSerializable]
public sealed class Sequence
{
    public int Number;
    public string Info;
    public bool Activated;
    public (NucleoBase, NucleoBase)[] Dna;

    public Sequence(int number, string info, bool activated, (NucleoBase, NucleoBase)[] dna)
    {
        Number = number;
        Info = info;
        Activated = activated;
        Dna = dna;
    }
}

[Serializable, NetSerializable]
public sealed class SequencerStatus
{

    public int SelectedSequenceId;
    public IList<Sequence> Sequences;

    public SequencerStatus(IList<Sequence> sequences, int selectedSequenceId)
    {
        SelectedSequenceId = selectedSequenceId;
        Sequences = sequences;
    }
}

[Serializable, NetSerializable]
public sealed class GeneticComputerScanInfoInterface : BoundUserInterfaceState
{
    public string SubjectName;
    public string SubjectStatus;
    public float GeneticDamage;
    public TimeSpan PrintCooldown;
    public SequencerStatus SequencerStatus;

    public GeneticComputerScanInfoInterface(string subjectName, string subjectStatus, float geneticDamage, TimeSpan printCooldown, SequencerStatus sequencerStatus)
    {
        SubjectName = subjectName;
        SubjectStatus = subjectStatus;
        GeneticDamage = geneticDamage;
        PrintCooldown = printCooldown;
        SequencerStatus = sequencerStatus;
    }
}
