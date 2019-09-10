using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using System.Linq;
using System;

public class StatsScript : MonoBehaviour
{
    public Stats startingStats;
    public Stats statsCap;

    public List<Stats> statsChanges;

    private Health health;
    private Movement movement;
    private FlickerLight flickerLight;
    new private Light2D light;

    public event Action RefreshStatsEvent = delegate{};

    void Awake()
    {
        statsChanges = statsChanges ?? new List<Stats>();
        health = GetComponentInChildren<Health>();
        movement = GetComponentInChildren<Movement>();
        flickerLight = GetComponentInChildren<FlickerLight>();
        light = GetComponentInChildren<Light2D>();
    }

    void Start()
    {
        RefreshApplyStats();
    }

    public float GetMaxHP()
    {
        return Mathf.Min(statsCap.maxHP, startingStats.maxHP + statsChanges.Select(sc => sc.maxHP).ToList().Sum());
    }

    public float GetSpeed()
    {
        return Mathf.Min(statsCap.speed, startingStats.speed + statsChanges.Select(sc => sc.speed).ToList().Sum());
    }

    public float GetLight()
    {
        return Mathf.Min(statsCap.light, startingStats.light + statsChanges.Select(sc => sc.light).ToList().Sum());
    }

    void RefreshApplyStats()
    {
        if(health != null)
            health.maxHealth = GetMaxHP();
        if(movement != null)
            movement.speed = GetSpeed();
        if (flickerLight != null)
            light.pointLightOuterRadius = GetLight()/2.0f;

        RefreshStatsEvent();
    }

    public void AddStatsChange(Stats statsChange)
    {
        Debug.Log("Adding Stat Change" + statsChange);
        if(statsChange == null) return;

        statsChanges.Add(statsChange);
        RefreshApplyStats();
    }

    public void RemoveStatsChange(Stats statsChange)
    {
        Debug.Log("Removing Stat Change" + statsChange);
        if(statsChange == null) return;

        statsChanges.Remove(statsChange);
        RefreshApplyStats();
    }
}
