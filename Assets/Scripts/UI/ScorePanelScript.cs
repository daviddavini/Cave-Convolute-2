using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePanelScript : EventEffect
{
    private int score = 0;
    private TextMeshProUGUI textMeshPro;

    protected override void Awake()
    {
        base.Awake();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    protected override void DoEffect()
    {
        score += 1;
        textMeshPro.text = "SCORE   " + score;
    }
}
