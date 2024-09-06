// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Enums;
using JadeLib.Features.Hints.Parsing;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Elements;

public abstract class Element
{
    public delegate string HintContent(HintCtx ctx);

    /// <summary>
    ///     Gets or sets the options for this element.
    /// </summary>
    public virtual ElementOptions Options { get; protected set; } = ElementOptions.PreserveSpacing;

    /// <summary>
    ///     Gets or sets a value indicating whether or not this element is enabled and will show.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     Gets or sets the position of the element on a scale from 0-1000, where 0 represents the bottom of the screen and
    ///     1000 represents the top.
    /// </summary>
    public abstract float Position { get; set; }

    /// <summary>
    ///     Gets or sets the priority of the hint (determining if it shows above another hint).
    /// </summary>
    public virtual int ZIndex { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the <see cref="Parser" /> currently in use by this <see cref="Element" />.
    /// </summary>
    /// <remarks>
    ///     Implementations should default this to <see cref="Parsing.Parser.DefaultParser" />.
    /// </remarks>
    public Parser Parser { get; set; } = Parser.DefaultParser;

    public abstract string GetText(HintCtx ctx);

    public virtual ParsedData GetParsedData(ReferenceHub player)
    {
        return Parser.DefaultParser.Parse(this.GetText(new HintCtx(this, player)));
    }
}

public record HintCtx(Element element, ReferenceHub Player);