using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEffect : EventEffect
{
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
    }

    protected override void DoEffect()
    {
        gameManager.EndGame();
    }
}
