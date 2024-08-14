using JadeLib.Features.Hints.Parsing.Records;

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
/// Provides a way to handle indent tags.
/// </summary>
[RichTextTag]
public class IndentTag : MeasurementTag
{
    /// <inheritdoc/>
    public override string[] Names { get; } = { "indent" };

    /// <inheritdoc/>
    public override bool HandleTag(ParserContext context, MeasurementInfo info)
    {
        context.ResultBuilder.Append($"<rotate={info}>");
        context.AddEndingTag<CloseIndentTag>();

        return true;
    }
}
