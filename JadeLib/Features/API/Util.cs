using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace JadeLib.API
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Exiled.API.Features;

    /// <summary>
    /// A set of various extension functions for utility purposes.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Retrieves or initializes a <see cref="KeyValuePair{TKey,TValue}"/> for a dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value in case it isn't found.</param>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The found/initilized value.</returns>
        public static TValue GetOrInit<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            var b = dict.TryGetValue(key, out var dictValue);
            switch (b)
            {
                case true:
                    return dictValue;
                case false:
                    dict.Add(key, value);
                    return value;
            }
        }

        public static Vector3 CenterRoom(GameObject gameObject, Vector3 localPos)
        {
            return gameObject.transform.TransformPoint(localPos);
        }

        /// <summary>
        /// Using Unity's Random generates a random number between 1 and 100.
        /// </summary>
        /// <param name="number">The chance to return true.</param>
        /// <returns>If it was true or not.</returns>
        public static bool Chance(this int number)
        {
            var random = UnityEngine.Random.Range(1, 100);
            return random <= number;
        }

        public static void CopyPublicFields(object source, object destination)
        {
            if (source == null || destination == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            var sourceFields = sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var destinationFields = destinationType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceField in sourceFields)
            {
                var destinationField = Array.Find(destinationFields, f => f.Name == sourceField.Name && f.FieldType == sourceField.FieldType);
                if (destinationField != null)
                {
                    destinationField.SetValue(destination, sourceField.GetValue(source));
                }
            }
        }

        public static void GetOrNothing<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Action<TValue> func)
        {
            var b = dict.TryGetValue(key, out var value);
            switch (b)
            {
                case true:
                    func.Invoke(value);
                    break;
                case false:
                    break;
            }
        }
        
        public static MethodBase GetCallingMethod()
        {
            var stackTrace = new StackTrace();
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var method = frame.GetMethod();
                // Skip until we find the method that called this Harmony-patched method
                if (method.DeclaringType != typeof(Player) && method.DeclaringType != typeof(HarmonyMethod))
                {
                    return method;
                }
            }
            return null;
        }

        public static bool ContainsTuple<TKey, TValue>(this IEnumerable<Tuple<TKey, TValue>> enumerable, TKey check)
        {
            var comparer = EqualityComparer<TKey>.Default;
            return enumerable.Any(e => comparer.Equals(e.Item1, check));
        }

        public static bool ContainsTuple<TKey, TValue>(this IEnumerable<Tuple<TKey, TValue>> enumerable, TValue check)
        {
            var comparer = EqualityComparer<TValue>.Default;
            return enumerable.Any(e => comparer.Equals(e.Item2, check));
        }
    }
}