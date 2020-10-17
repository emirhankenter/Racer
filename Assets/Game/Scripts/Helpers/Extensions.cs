using Game.Scripts.Behaviours;
using Game.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Extensions
{
    public static Direction? GetDirectionTo(this Transform transform, Transform target)
    {
        var right = Vector3.Cross(transform.up, transform.forward);
        var direction = Vector3.Dot(right, target.position - transform.position);

        if (direction > 0.0f)
        {
            return Direction.Right;
        }
        else if (direction < 0.0f)
        {
            return Direction.Left;
        }

        return null;
    }

    public static T Spawn<T>(this T target)
    {
        return ObjectPoolingManager.Instance.Spawn(target);
    }


    private static System.Random Random = new System.Random();
    public static T GetRandomElement<T>(this T[] array)
    {
        var index = Random.Next(0, array.Length);
        return array[index];
    }
    public static T GetRandomElement<T>(this List<T> list)
    {
        var index = Random.Next(0, list.Count);
        return list[index];
    }
}
