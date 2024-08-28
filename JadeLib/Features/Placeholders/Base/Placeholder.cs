#region

using JadeLib.Features.Abstract;

#endregion

namespace JadeLib.Features.Placeholders.Base;

/// <summary>
///     The base of a placeholder, without the generic.
/// </summary>
public abstract class PlaceholderBase : ModuleSystem<PlaceholderBase>
{
    /// <summary>
    ///     Gets the pattern for this placeholder;
    /// </summary>
    public abstract string Pattern { get; }

    /// <inheritdoc />
    protected override void Register()
    {
        if (!PlaceholderManager.Bases.ContainsKey(this.GetType()))
        {
            PlaceholderManager.Bases.Add(this.GetType(), this);
        }
    }
}

/// <summary>
///     A placeholder.
/// </summary>
/// <typeparam name="TPasser">The passer when processing this placeholder.</typeparam>
public abstract class Placeholder<TPasser> : PlaceholderBase
    where TPasser : class
{
    /// <summary>
    ///     Gets the passer type for this placeholder, null means no passer (static)
    /// </summary>
    public virtual TPasser PasserType { get; } = null;

    public abstract string Process(TPasser passer = default);
}