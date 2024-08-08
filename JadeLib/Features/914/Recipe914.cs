// <copyright file="Recipe914.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Exiled.API.Features;

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