#region

using JadeLib.Features.Hints.Parsing.Enums;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle pos tags.
/// </summary>
[RichTextTag]
public class PosTag : MeasurementTag
{
    /// <inheritdoc />
    public override string[] Names { get; } = { "pos" };

    /// <inheritdoc />
    public override bool HandleTag(ParserContext context, MeasurementInfo info)
    {
        var (value, style) = info;

        var convertedValue = style switch
        {
            MeasurementUnit.Ems => value * Constants.EMSTOPIXELS,
            MeasurementUnit.Percentage => info.value / 100 * Constants.DISPLAYAREAWIDTH,
            _ => value,
        };

        context.CurrentLineWidth = convertedValue;
        context.SkipOverflow = true;
        context.WidthSinceSpace = 0;
        context.ResultBuilder.Append($"<pos={convertedValue}>");

        return true;
    }
}