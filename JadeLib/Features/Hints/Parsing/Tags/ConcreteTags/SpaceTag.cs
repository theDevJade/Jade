// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Parsing.Enums;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle space tags.
/// </summary>
[RichTextTag]
public class SpaceTag : MeasurementTag
{
    /// <inheritdoc />
    public override string[] Names { get; } = { "space" };

    /// <inheritdoc />
    public override bool AllowPercentages { get; } = false;

    /// <inheritdoc />
    public override bool HandleTag(ParserContext context, MeasurementInfo info)
    {
        var (value, style) = info;

        var convertedValue = style switch
        {
            MeasurementUnit.Ems => value * Constants.EMSTOPIXELS,
            _ => value
        };

        if (context.WidthSinceSpace > 0.0001 && context.WidthSinceSpace + convertedValue > context.FunctionalWidth)
        {
            Parser.CreateLineBreak(context, true);
        }

        context.WidthSinceSpace += convertedValue;

        context.SkipOverflow = true;
        context.ResultBuilder.Append($"<space={convertedValue}>");

        return true;
    }
}