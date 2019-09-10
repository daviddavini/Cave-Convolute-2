using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
     public static Transform FindParentWithTag(this Transform child, string tag)
     {
        while (child.parent != null)
        {
             if (child.parent.tag == tag)
             {
                return child.parent;
             }
             child = child.parent;
        }
        return null; // Could not find a parent with given tag.
     }

    public static float RandomFloat(System.Random random, float min, float max)
    {
        return (float) random.NextDouble() * (max-min) + min;
    }

    public static Vector3 RandomVector(System.Random random, Vector3 min, Vector3 max)
    {
        return new Vector3(RandomFloat(random, min.x, max.x),
          RandomFloat(random, min.y, max.y),
          RandomFloat(random, min.z, max.z));
    }

    public static Vector2 MultiplyComponents(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }

    public static float BoundedFloat(float number, float min, float max)
    {
        return Mathf.Min(Mathf.Max(number, min), max);
    }

    public static int Bounded(this int number, int min, int max)
    {
        return Mathf.Min(Mathf.Max(number, min), max);
    }

    public static Vector2 UnitVector(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static Vector2 RandomUnitVector(System.Random random)
    {
        float angle = RandomFloat(random, -Mathf.PI, Mathf.PI);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static T RandomItemFromList<T>(System.Random random, List<T> list)
    {
        return list[random.Next(0, list.Count)];
    }

    public static void ShuffleList<T>(System.Random random, List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = random.Next(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static GameObject FindClosest(Vector3 position, Predicate<GameObject> condition, float radius = Mathf.Infinity)
    {
        float distanceToClosestGO = radius;
        GameObject closestGO = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        foreach(Collider2D collider in colliders)
        {
            if (condition(collider.gameObject))
            {
                GameObject currentGO = collider.gameObject;
                float distanceToCurrentGO = (currentGO.transform.position - position).magnitude;
                if (distanceToCurrentGO < distanceToClosestGO)
                {
                    distanceToClosestGO = distanceToCurrentGO;
                    closestGO = currentGO;
                }
            }
        }
        return closestGO;
    }
}
