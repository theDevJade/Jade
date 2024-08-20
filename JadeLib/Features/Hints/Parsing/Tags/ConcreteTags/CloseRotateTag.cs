// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle closing rotate tags.
/// </summary>
[RichTextTag]
public class CloseRotateTag : ClosingTag<CloseRotateTag>
{
    /// <inheritdoc />
    public override string Name { get; } = "/rotate";
}