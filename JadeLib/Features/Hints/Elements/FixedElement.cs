namespace JadeLib.Features.Hints.Elements;

public sealed class FixedElement(string text, int position) : Element
{
    private readonly string text = text;

    public override float Position { get; set; } = position;

    public override string GetText(HintCtx ctx)
    {
        return this.text;
    }
}