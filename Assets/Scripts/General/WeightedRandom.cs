using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class WeightedRandomElement<T>
{
    public T element;
    public float weight = 1;
}

[System.Serializable]
public class WeightedRandom<T>
{
    public List<WeightedRandomElement<T>> members;

    public WeightedRandom(List<WeightedRandomElement<T>> members)
    {
        this.members = members;
    }

    public T RandomElement(System.Random random)
    {
        float totalWeight = members.Sum(x => x.weight);
        float prob = (float)random.NextDouble();
        float currentWeight = 0;
        foreach (WeightedRandomElement<T> member in members)
        {
            currentWeight += member.weight;
            if (prob < currentWeight / totalWeight)
            {
                //Debug.Log("Weighted Random Chose Number: " + members.IndexOf(member));
                return member.element;
            }
                
        }
        Debug.Log("No Weighted Random Member was chosen");
        return default;
    }
}

[Serializable]
public class WeightedSprite : WeightedRandomElement<Sprite>
{
}