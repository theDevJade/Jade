// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Parsing.Enums;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle size tags.
/// </summary>
[RichTextTag]
public class SizeTag : MeasurementTag
{
    private const string TAGFORMAT = "<size={0}>";

    /// <inheritdoc />
    public override string[] Names { get; } = { "size" };

    /// <inheritdoc />
    public override bool HandleTag(ParserContext context, MeasurementInfo info)
    {
        context.SizeTags.Push(context.Size);
        var value = info.style switch
        {
            MeasurementUnit.Percentage => info.value / 100 * Constants.DEFAULTSIZE,
            MeasurementUnit.Ems => info.value * Constants.EMSTOPIXELS,
            _ => info.value
        };

        context.Size = value;
        context.ResultBuilder.AppendFormat(TAGFORMAT, value);

        return true;
    }
}