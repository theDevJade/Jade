// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System.Collections.Generic;
using JadeLib.Features.Hints.Elements;
using JadeLib.Features.Hints.Hints.Base.CustomHints;

#endregion

namespace JadeLib.Features.Hints.Hints.GeneralUse;

public abstract class ListBasedHint : ConditionalHint
{
    public override bool Debug { get; set; } = false;

    /// <summary>
    ///     Gets or sets the list wrapper, should include a {SPLIT} to indicate where to insert the list.
    /// </summary>
    public virtual string ListWrapper { get; set; } = "{SPLIT}";

    public abstract List<string> List { get; set; }

    public virtual char Seperator { get; } = ',';

    protected override string Content(HintCtx context)
    {
        return this.ListWrapper.Replace("{SPLIT}", $"{string.Join(this.Seperator.ToString(), this.List)}");
    }
}