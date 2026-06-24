using Robust.Shared.Serialization;

namespace Content.Shared._BRatbite.Genetics;

[Serializable, NetSerializable]
public enum NucleoBase : byte
{
    A = 0, T = 1, C = 2, G = 3, X = 4
}

public static class NucleoExtension
{
    public static NucleoBase GetOpposite(this NucleoBase n)
    {
        if (n == NucleoBase.X) return n;
        return (NucleoBase) ((byte) n ^ 1);
    }
}
