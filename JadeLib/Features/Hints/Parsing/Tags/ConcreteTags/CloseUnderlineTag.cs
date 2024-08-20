// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle closing underline tags.
/// </summary>
[RichTextTag]
public class CloseUnderlineTag : ClosingTag<CloseUnderlineTag>
{
    /// <inheritdoc />
    public override string Name { get; } = "/u";
}