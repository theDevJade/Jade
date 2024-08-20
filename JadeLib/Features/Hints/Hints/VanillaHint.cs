// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Hints;
using JadeLib.Features.Hints.Display;
using JadeLib.Features.Hints.Elements;
using MEC;

#endregion

namespace JadeLib.Features.Hints.Hints;

public sealed class VanillaHint
{
    public readonly DynamicElement Element;
    public readonly Screen Owner;

    internal VanillaHint(Screen owner)
    {
        this.Owner = owner;
        this.Element = new DynamicElement(400, this.Get);
    }

    public string CurrentText { get; private set; } = string.Empty;

    private string Get(HintCtx ctx)
    {
        return this.CurrentText;
    }

    public void Add(TextHint hint)
    {
        this.CurrentText = hint.Text;
        Timing.CallDelayed(
            hint.DurationScalar,
            () =>
            {
                if (this.CurrentText == hint.Text)
                {
                    this.CurrentText = string.Empty;
                }
            });
    }
}