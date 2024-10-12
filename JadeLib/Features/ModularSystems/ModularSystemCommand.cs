using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace JadeLib.Features.ModularSystems;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ModularSystemCommand : ICommand
{
    public string Command => "modular";

    public string[] Aliases => new[] { "modularsystem", "modsystem", "modularplugins" };

    public string Description => "Allows modular enabling and disabling of modules and their respective components.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response =
                "Usage:\nmodular show\nmodular enable <module> <component>\nmodular disable <module> <component>";
            return false;
        }

        var action = arguments.At(0).ToLower();

        switch (action)
        {
            case "enable":
            case "disable":
                if (arguments.Count < 3)
                {
                    response = $"Usage: modular {action} <module> <component>";
                    return false;
                }

                var moduleName = arguments.At(1);
                var componentName = arguments.At(2);
                var enable = action == "enable";

                var module = ModularManager.Systems.FirstOrDefault(
                    s => s.SystemName.Equals(moduleName, StringComparison.OrdinalIgnoreCase));

                if (module == null)
                {
                    response = $"Module '{moduleName}' not found.";
                    return false;
                }

                if (!string.IsNullOrEmpty(module.Permission) && !sender.CheckPermission(module.Permission))
                {
                    response = $"You do not have permission to manage the '{module.SystemName}' module.";
                    return false;
                }

                if (!ModularManager.SystemComponents.TryGetValue(module, out var components))
                {
                    response = $"No components found for module '{moduleName}'.";
                    return false;
                }

                var component = components.FirstOrDefault(
                    c => c.Id.Equals(componentName, StringComparison.OrdinalIgnoreCase));

                if (component == null)
                {
                    response = $"Component '{componentName}' not found in module '{moduleName}'.";
                    return false;
                }

                if (!string.IsNullOrEmpty(component.Permission) && !sender.CheckPermission(component.Permission))
                {
                    response = $"You do not have permission to manage the '{component.Name}' component.";
                    return false;
                }

                component.SetEnabled(enable, out var setResponse);
                response = setResponse;
                return true;

            case "show":
                response = ModularManager.GetModuleStatus();
                return true;

            default:
                response =
                    "Usage:\nmodular show\nmodular enable <module> <componentID>\nmodular disable <module> <componentID>";
                return false;
        }
    }
}