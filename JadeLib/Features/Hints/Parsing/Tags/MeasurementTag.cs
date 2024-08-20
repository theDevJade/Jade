// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using JadeLib.Features.Hints.Parsing.Enums;
using JadeLib.Features.Hints.Parsing.Records;

#endregion

namespace JadeLib.Features.Hints.Parsing.Tags;

/// <summary>
///     Defines a <see cref="RichTextTag" /> that only takes in a measurement.
/// </summary>
public abstract class MeasurementTag : RichTextTag
{
    /// <inheritdoc />
    public sealed override TagStyle TagStyle { get; } = TagStyle.ValueParam;

    /// <summary>
    ///     Gets a value indicating whether or not this tag allows parentheses.
    /// </summary>
    public virtual bool AllowPercentages { get; } = true;

    /// <inheritdoc />
    public sealed override bool HandleTag(ParserContext context, string content)
    {
        if (MeasurementInfo.TryParse(content, out var info) &&
            (info.style != MeasurementUnit.Percentage || this.AllowPercentages))
        {
            return this.HandleTag(context, info);
        }

        return false;
    }

    /// <summary>
    ///     Handles an instance of this tag with <see cref="MeasurementInfo" />.
    /// </summary>
    /// <param name="context">The context of the parser.</param>
    /// <param name="info">The information about the measurement.</param>
    /// <returns>true if the tag is valid, otherwise false.</returns>
    public abstract bool HandleTag(ParserContext context, MeasurementInfo info);
}