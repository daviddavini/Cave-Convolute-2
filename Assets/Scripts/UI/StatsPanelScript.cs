using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsPanelScript : MonoBehaviour
{
    public StatsScript statsScript;
    private TextMeshProUGUI textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        statsScript.RefreshStatsEvent += UpdateStatsText;
    }

    // Start is called before the first frame update
    void UpdateStatsText()
    {
        string text = "";
        text += "Max HP:   " + statsScript.GetMaxHP().ToString();
        text += "\nSpeed:   " + statsScript.GetSpeed().ToString();
        text += "\nLight:   " + statsScript.GetLight().ToString();
        textMeshPro.text = text;
    }
}
