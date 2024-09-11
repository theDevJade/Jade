namespace JadeLib.Loader.LoaderSegments;

public class HarmonySegment : LoaderSegment
{
    public override string Name { get; } = "Harmony Patches";

    public override bool Condition => true;

    protected override void Load()
    {
        Jade._harmony.PatchAll();
    }

    protected override void Unload()
    {
        Jade._harmony.UnpatchAll(Jade._harmony.Id);
    }
}