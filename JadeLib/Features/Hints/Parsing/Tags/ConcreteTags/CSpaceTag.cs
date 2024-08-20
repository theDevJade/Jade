// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Parsing.Enums;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle cspace tags.
/// </summary>
[RichTextTag]
public class CSpaceTag : MeasurementTag
{
    /// <inheritdoc />
    public override string[] Names { get; } = { "cspace" };

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

        context.CurrentCSpace = convertedValue;
        context.ResultBuilder.Append($"<cspace={convertedValue}>");

        context.AddEndingTag<CloseCSpaceTag>();

        return true;
    }
}