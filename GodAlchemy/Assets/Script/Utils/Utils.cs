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

    public static Vector3Int ToVector3Int(this Vector3 vector3)
    {
        return Vector3Int.RoundToInt(vector3);
    }

    public static GameObject Instantiate(this GameObject gameObject, Vector3 position)
    {
        var go = GameObject.Instantiate(gameObject);
        go.transform.position = position;
        return go;
    }


}

