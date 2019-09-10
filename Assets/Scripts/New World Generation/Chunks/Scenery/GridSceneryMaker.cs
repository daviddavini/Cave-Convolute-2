using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSceneryMaker : SceneryMaker
{
    public Vector2Int gridCount;
    [Range(0,1)]
    public float gridDensity;
    public bool gridCountXByDensity = false;
    public bool gridCountYByDensity = false;
    public Rect gridPercentageRect = new Rect(new Vector2(0,0), new Vector2(1,1));
    //public bool gridRectByEdgeBuffer;

    // public Rect GridRect { get{
    //     if(gridRectByEdgeBuffer){
    //         return new Rect(gridRect, gridSize);
    //     }
    // }}

    protected override void Make(Chart chart)
    {
        base.Make(chart);
        Rect gridRect = PercentageRectToLocalRect(gridPercentageRect);

        Vector2Int gridCount = new Vector2Int(
            gridCountXByDensity ? (int)(gridRect.size.x*gridDensity) : this.gridCount.x,
            gridCountYByDensity ? (int)(gridRect.size.y*gridDensity) : this.gridCount.y
        );
        for (int i = 0; i < gridCount.x; i++) {
            for (int j = 0; j < gridCount.y; j++) {
                Vector3 localPos = (Vector3) gridRect.position +
                  new Vector3(i*gridRect.size.x/(gridCount.x-1), j*gridRect.size.y/(gridCount.y-1), 0);
                if(gridCount.x == 1)
                    localPos = new Vector3(gridRect.size.x/2.0f, localPos.y, localPos.z);
                if(gridCount.y == 1)
                    localPos = new Vector3(localPos.x, gridRect.size.y/2.0f, localPos.z);
                MakeScenery(localPos);
            }
        }
    }
}
