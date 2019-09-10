using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootPrefab
{
    public GameObject prefab;
    public float chance = 1;
}

public class LootEffect : EventEffect
{
    public List<LootPrefab> lootPrefabs;
    public float maxOffset = 0.3f;

    private System.Random random;

    protected override void Awake()
    {
        random = new System.Random();
        base.Awake();
    }

    protected override void DoEffect()
    {
        foreach(LootPrefab lootPrefab in lootPrefabs)
        {
            while(random.NextDouble() < lootPrefab.chance)
            {
                Vector3 offset = Tools.RandomVector(random, new Vector3(-maxOffset, -maxOffset, 0),
                  new Vector3(maxOffset, maxOffset, 0));
                Instantiate(lootPrefab.prefab, transform.position + offset,
                  Quaternion.identity, transform.root);
            }
        }
    }
    // protected override void UndoEffect()
    // {
    //
    // }
}
