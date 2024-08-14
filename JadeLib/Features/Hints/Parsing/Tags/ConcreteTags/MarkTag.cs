using JadeLib.Features.Hints.Parsing.Enums;

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
/// Provides a way to handle mark tags.
/// </summary>
[RichTextTag]
public class MarkTag : RichTextTag
{
    /// <inheritdoc/>
    public override string[] Names { get; } = { "mark" };

    /// <inheritdoc/>
    public override TagStyle TagStyle { get; } = TagStyle.ValueParam;

    /// <inheritdoc/>
    public override bool HandleTag(ParserContext context, string content)
    {
        context.ResultBuilder.Append($"<mark={content}>");
        context.AddEndingTag<CloseMarkTag>();
        return true;
    }
}
