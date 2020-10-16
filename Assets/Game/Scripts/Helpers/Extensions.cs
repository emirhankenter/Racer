using Game.Scripts.Behaviours;
using Game.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
