#region

using JadeLib.Features.Placeholders.Base;

#endregion

namespace JadeLib.Loader.LoaderSegments;

public class PlaceholderSegment : LoaderSegment
{
    public override string Name { get; } = "Placeholder Patch";

    public override bool Condition => true;

    protected override void Load()
    {
        PlaceholderBase.ReflectiveRegister();
        foreach (var usingAssembly in Jade.UsingAssemblies)
        {
            PlaceholderPatcher.ApplyPatches(usingAssembly, Jade._harmony);
        }
    }

    protected override void Unload()
    {
    }
}