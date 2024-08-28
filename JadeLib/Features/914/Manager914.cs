// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Scp914;
using JadeLib.Features.Abstract.FeatureGroups;
using JadeLib.Features.Abstract.FeatureGroups.Events;
using JadeLib.Features.Extensions;
using JadeLib.Features.Positioning;
using MEC;

#endregion

namespace JadeLib.Features._914;

public class Manager914
{
    private static Manager914 Instance;

    internal Manager914()
    {
        Instance ??= this;
        new FeatureGroup("jade914").Supply(this).Register();
        foreach (var assembly in Jade.UsingAssemblies)
        {
            foreach (var type in assembly.GetTypes().Where(e => e.IsSubclassOf(typeof(Recipes914))))
            {
                var instance = Activator.CreateInstance(type, JadeFeature.Flags, null) as Recipes914;
                type.GetMethod("RegisterRecipes", JadeFeature.Flags, null, [], null)?.Invoke(instance, []);
            }
        }
    }

    public static List<ItemRecipe914> ItemRecipes { get; } = [];

    public static List<PlayerRecipe914> PlayerRecipes { get; } = [];

    [Listener]
    internal void On914Item(UpgradingInventoryItemEventArgs args)
    {
        var recipes = ItemRecipes.Where(e => args.Item.Type == e.Input);
        var itemRecipe914s = recipes.ToList();
        if (!itemRecipe914s.Any())
        {
            return;
        }

        itemRecipe914s.Shuffle();

        args.IsAllowed = false;

        Timing.CallDelayed(
            0.3f,
            () =>
            {
                foreach (var (itemType, chance, itemTypes) in itemRecipe914s)
                {
                    switch (chance.Chance())
                    {
                        case true:
                            args.Item.Destroy();
                            foreach (var item in itemTypes.Select(type => Item.Create(type)))
                            {
                                switch (args.Player.IsInventoryFull)
                                {
                                    case true:
                                        item.CreatePickup(args.Player.Position);
                                        continue;
                                    case false:
                                        item.Give(args.Player);
                                        continue;
                                }
                            }

                            return;
                        case false:
                            continue;
                    }
                }
            });
    }

    [Listener]
    internal void On914Player(UpgradingPlayerEventArgs args)
    {
        var recipes = PlayerRecipes.Where(e => e.InputPredicate(args.Player));
        var playerRecipe914s = recipes.ToList();
        if (!playerRecipe914s.Any())
        {
            return;
        }

        playerRecipe914s.Shuffle();

        Timing.CallDelayed(
            0.3f,
            () =>
            {
                foreach (var (_, chance, action) in playerRecipe914s)
                {
                    switch (chance.Chance())
                    {
                        case true:
                            action(args.Player);
                            return;
                        case false:
                            continue;
                    }
                }
            });
    }
}