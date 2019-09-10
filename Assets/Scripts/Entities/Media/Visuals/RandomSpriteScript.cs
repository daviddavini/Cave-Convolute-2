using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomSpriteScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public static System.Random classwideRandom;

    public List<WeightedSprite> weightedSprites;
    public WeightedRandom<Sprite> weightedRandomSprites;

    void Awake()
    {
        if (classwideRandom == null)
            classwideRandom = new System.Random();

        weightedRandomSprites = new WeightedRandom<Sprite>(weightedSprites.Cast<WeightedRandomElement<Sprite>>().ToList());
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = weightedRandomSprites.RandomElement(new System.Random(classwideRandom.Next()));
    }
}
