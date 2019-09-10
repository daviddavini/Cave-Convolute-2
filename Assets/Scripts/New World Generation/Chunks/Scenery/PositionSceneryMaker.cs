using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSceneryMaker : SceneryMaker
{
    public List<Vector2> percentagePositions = new List<Vector2>(){
      new Vector2(0.5f, 0.5f)
    };

    public Orientation orientationRequirement = Orientation.None;

    protected override void Make(Chart chart)
    {
        if (orientationRequirement != Orientation.None && orientationRequirement != chart.GetOrientation())
        {
            return;
        }

        base.Make(chart);
        for(int i = 0; i < percentagePositions.Count; i++)
        {
            MakeScenery(PercentagePosToLocalPos(percentagePositions[i]));
        }
    }
}
