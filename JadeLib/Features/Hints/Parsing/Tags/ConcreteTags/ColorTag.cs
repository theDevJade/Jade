﻿// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Parsing.Enums;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle color tags.
/// </summary>
[RichTextTag]
public class ColorTag : RichTextTag
{
    /// <inheritdoc />
    public override string[] Names { get; } = { "color" };

    /// <inheritdoc />
    public override TagStyle TagStyle { get; } = TagStyle.ValueParam;

    /// <inheritdoc />
    public override bool HandleTag(ParserContext context, string content)
    {
        if (content.StartsWith("#"))
        {
            if (!Constants.ValidColorSizes.Contains(content.Length - 1))
            {
                return false;
            }
        }
        else
        {
            var unquoted = TagHelpers.ExtractFromQuotations(content);
            if (unquoted == null || !Constants.Colors.Contains(unquoted))
            {
                return false;
            }
        }

        context.ResultBuilder.Append($"<color={content}>");
        context.AddEndingTag<CloseColorTag>(true);
        return true;
    }
}