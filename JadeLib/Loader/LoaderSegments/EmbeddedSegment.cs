namespace JadeLib.Loader.LoaderSegments;

public class EmbeddedSegment : LoaderSegment
{
    public override string Name { get; } = "Embedded DLLs";

    public override bool Condition => true;

    protected override void Load()
    {
        CosturaUtility.Initialize();
    }

    protected override void Unload()
    {
    }
}