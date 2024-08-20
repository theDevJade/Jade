// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

namespace JadeLib.Features.Hints.Parsing.Tags.ConcreteTags;

/// <summary>
///     Provides a way to handle closing color tags.
/// </summary>
[RichTextTag]
public class CloseColorTag : ClosingTag<CloseColorTag>
{
    /// <inheritdoc />
    public override string Name { get; } = "/color";
}