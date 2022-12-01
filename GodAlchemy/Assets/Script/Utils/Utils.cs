using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        if (source.Count() == 0) return default;
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static int RandomIndex(this ICollection source)
    {
        return UnityEngine.Random.Range(0, source.Count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static void AddRange<T>(this ICollection<T> set, T[] items)
    {
        foreach (var item in items)
        {
            set.Add(item);
        }
    }

}

