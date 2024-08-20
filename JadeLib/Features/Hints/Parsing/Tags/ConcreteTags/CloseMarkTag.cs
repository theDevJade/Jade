// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle closing mark tags.
/// </summary>
[RichTextTag]
public class CloseMarkTag : ClosingTag<CloseMarkTag>
{
    /// <inheritdoc />
    public override string Name { get; } = "/mark";
}