﻿#region

using JadeLib.Features.Hints.Parsing.Enums;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle align tags.
/// </summary>
[RichTextTag]
public class AlignTag : RichTextTag
{
    /// <inheritdoc />
    public override string[] Names { get; } = { "align" };

    /// <inheritdoc />
    public override TagStyle TagStyle { get; } = TagStyle.ValueParam;

    /// <inheritdoc />
    public override bool HandleTag(ParserContext context, string content)
    {
        var alignment = TagHelpers.ExtractFromQuotations(content);

        if (alignment == null || !Constants.Alignments.Contains(alignment))
        {
            return false;
        }

        context.ResultBuilder.Append($"<align={alignment}>");
        context.AddEndingTag<CloseAlignTag>();

        return true;
    }
}