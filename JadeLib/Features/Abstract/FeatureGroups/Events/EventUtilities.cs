#region

using System.Globalization;
using System.Linq;
using System.Reflection;

#endregion

namespace JadeLib.Features.Abstract.FeatureGroups.Events;

public static class EventUtilities
{
    public static void RegisterInNamepsace(string @namespace)
    {
        Assembly.GetExecutingAssembly().GetTypes().Where(
                e => e.Namespace!.StartsWith(@namespace.ToLower(), true, CultureInfo.CurrentCulture)).ToList()
            .ForEach(EventGroup.RegisterAllEventsForMethods);
    }
}