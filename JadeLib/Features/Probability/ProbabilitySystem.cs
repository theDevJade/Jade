#region

using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

#endregion

namespace JadeLib.Features.Probability;

/// <summary>
///     A system that manages probabilities, ensuring they sum to 100%.
/// </summary>
/// <typeparam name="T">The type of the items in the probability system.</typeparam>
public class ProbabilitySystem<T>
{
    private readonly List<ProbabilityItem<T>> items = [];

    /// <summary>
    ///     Adds an item to the probability system with the specified weight.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="weight">The weight or probability of the item occurring.</param>
    /// <exception cref="System.ArgumentException">Thrown when the weight is out of the range [0, 100].</exception>
    public void AddItem(T item, float weight)
    {
        if (weight is < 0 or > 100)
        {
            throw new ArgumentException("Weight must be between 0 and 100.");
        }

        this.items.Add(new ProbabilityItem<T>(item, weight));
    }

    /// <summary>
    ///     Retrieves a random item based on the assigned probabilities.
    /// </summary>
    /// <returns>The randomly selected item.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the system is improperly configured.</exception>
    public T GetRandomItem()
    {
        this.NormalizeWeights(); // Ensure weights sum to 100% before getting an item.

        var totalWeight = this.items.Sum(i => i.Weight);
        var randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0;
        foreach (var item in this.items)
        {
            cumulativeWeight += item.Weight;
            if (randomValue <= cumulativeWeight)
            {
                return item.Item;
            }
        }

        throw new InvalidOperationException("Should not reach this point.");
    }

    /// <summary>
    ///     Normalizes the weights so that they sum up to 100%.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the total weight is zero.</exception>
    public void NormalizeWeights()
    {
        var totalWeight = this.items.Sum(i => i.Weight);

        if (totalWeight == 0)
        {
            throw new InvalidOperationException("Total weight must be greater than 0.");
        }

        foreach (var item in this.items)
        {
            item.Weight = item.Weight / totalWeight * 100.0f;
        }
    }

    /// <summary>
    ///     Represents an item in the probability system with its associated weight.
    /// </summary>
    /// <typeparam name="TU">The type of the item.</typeparam>
    private class ProbabilityItem<TU>(TU item, float weight)
    {
        public TU Item { get; } = item;

        public float Weight { get; set; } = weight;
    }
}