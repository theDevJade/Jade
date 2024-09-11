#region

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JadeLib.Features.AutoUpdater;
using JetBrains.Annotations;

#endregion

namespace JadeLib.Loader.LoaderSegments;

public class AutoUpdater : LoaderSegment
{
    public override string Name { get; } = "Initialize AutoUpdater";

    public override bool Condition => Jade.Settings.AutoUpdate;

    [CanBeNull] public static Task AutoUpdaterTask { get; set; }

    public static Timer AutoUpdaterTimer { get; private set; }

    protected override void Load()
    {
        AutoUpdaterCallback(null);
        AutoUpdaterTimer = new Timer(AutoUpdaterCallback, null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
    }

    private static void AutoUpdaterCallback(object state)
    {
        if (AutoUpdaterTask is null || !AutoUpdaterTask.IsCompleted)
        {
            return;
        }

        AutoUpdaterTask = Task.Run(
            async () =>
            {
                await JadeAutoUpdate.CheckAndUpdateDllAsync(
                    "theDevJade",
                    "Jade",
                    "1.0.0",
                    Assembly.GetExecutingAssembly().Location);
            });
    }

    protected override void Unload()
    {
    }
}