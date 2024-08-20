// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Parsing.Enums;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle line-height tags.
/// </summary>
[RichTextTag]
public class LineHeightTag : MeasurementTag
{
    private const string TAGFORMAT = "<line-height={0}p>";

    /// <inheritdoc />
    public override string[] Names { get; } = { "line-height" };

    /// <inheritdoc />
    public override bool HandleTag(ParserContext context, MeasurementInfo info)
    {
        var (value, style) = info;

        // the line height of ems and percentages changes based on the current size
        var convertedValue = style switch
        {
            MeasurementUnit.Percentage => value / 100 * Constants.DEFAULTHEIGHT *
                                          (context.Size / Constants.DEFAULTSIZE),
            MeasurementUnit.Ems => value * Constants.EMSTOPIXELS * (context.Size / Constants.DEFAULTSIZE),
            _ => value
        };

        context.CurrentLineHeight = convertedValue;
        context.ResultBuilder.AppendFormat(TAGFORMAT, convertedValue);

        context.AddEndingTag<CloseLineHeightTag>();

        return true;
    }
}