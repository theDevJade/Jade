// # --------------------------------------
// # Made by theDevJade with <3
// # --------------------------------------

#region

using System;
using System.Collections.Generic;
using Exiled.API.Features;

#endregion

namespace JadeLib.Features._914;

public abstract class Recipes914
{
    public abstract void RegisterRecipes();

    protected void Add(params ItemRecipe914[] recipes)
    {
        Manager914.ItemRecipes.AddRange(recipes);
    }

    protected void Add(params PlayerRecipe914[] recipes)
    {
        Manager914.PlayerRecipes.AddRange(recipes);
    }
}

public record ItemRecipe914(ItemType Input, int Chance, List<ItemType> Output);

public record PlayerRecipe914(Func<Player, bool> InputPredicate, int Chance, Action<Player> Output);