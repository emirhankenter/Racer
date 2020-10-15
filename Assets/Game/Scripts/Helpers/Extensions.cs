using Game.Scripts.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Direction? GetDirectionTo(this Transform transform, Transform target)
    {
        var perp = Vector3.Cross(transform.position, target.position);
        var direction = Vector3.Dot(perp, Vector3.up);

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
}
