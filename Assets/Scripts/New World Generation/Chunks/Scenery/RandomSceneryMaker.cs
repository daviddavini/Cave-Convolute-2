using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSceneryMaker : SceneryMaker
{
    public Vector2Int sceneryCountRange = new Vector2Int(1, 2);
    public float sceneryDensity = 0.1f;
    public bool sceneryCountByDensity;
    public float sceneryProbability = 0.1f;
    public bool sceneryCountByProbability;

    protected override void Make(Chart chart)
    {
        base.Make(chart);
        int sceneryCount;
        if(sceneryCountByDensity)
        {
            sceneryCount = (int)(sceneryDensity*chart.Area);
        }else if(sceneryCountByProbability)
        {
            sceneryCount = 0;
            while(Random.Range(0f,1f) < sceneryProbability)
                sceneryCount++;
        } else
        {
            sceneryCount = random.Next(sceneryCountRange.x, sceneryCountRange.y);
        }
        for(int i = 0; i < sceneryCount; i++)
        {
            Vector2 percentagePos;
            int tries = 0;
            do{
                percentagePos = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                //Debug.Log(percentagePos);
            } while (!chart.HasGroundAt(PercentagePosToLocalPos(percentagePos)) && tries++ < 10);
            if(tries == 11) {Debug.Log("Failed to make scenery on ground position"); continue;}
            MakeScenery(PercentagePosToLocalPos(percentagePos));
        }
    }
}
