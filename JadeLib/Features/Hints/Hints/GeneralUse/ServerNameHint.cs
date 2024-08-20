// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Elements;
using JadeLib.Features.Hints.Hints.Base.CustomHints;

#endregion

namespace JadeLib.Features.Hints.Hints.GeneralUse;

public abstract class ServerNameHint : GlobalHint
{
    public override bool Debug { get; set; } = false;

    public abstract string Name { get; set; }

    public override int Position { get; set; } = 20;

    public override string GetContent(HintCtx context)
    {
        return this.Name;
    }
}