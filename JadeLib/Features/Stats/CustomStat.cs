// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using Exiled.API.Features;

#endregion

namespace JadeLib.Features.Stats;

/// <inheritdoc />
public abstract class CustomStat<TSelf>(ReferenceHub owner) : Stat<TSelf>(owner)
    where TSelf : Stat<TSelf>
{
    /// <inheritdoc />
    protected override void RegisterStat()
    {
        if (!((this.Owner != null) & !Player.Get(this.Owner).DoNotTrack))
        {
            return;
        }

        var pool = this.Owner.GetStatisticsHub();
        pool.CustomStats.Add(this);
        this.RegisterCustomStat();
    }

    /// <summary>
    ///     Registers the custom stat, for events and such.
    /// </summary>
    protected abstract void RegisterCustomStat();
}