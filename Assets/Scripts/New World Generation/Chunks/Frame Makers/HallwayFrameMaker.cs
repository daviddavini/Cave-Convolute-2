using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayFrameMaker : FrameMaker
{
    public Vector2Int entryLengthRange;
    public Vector2Int entryToWallLengthRange;
    public Vector2Int sideWallLengthRange;

    public override List<FrameRange> PreferredFrameRanges { get {
        List<FrameRange> preferredFrameRanges = new List<FrameRange>();

        // Vector2Int entryToWallLengthRange = new Vector2Int((entryWallLengthRange.x-entryLengthRange.x)/2,
        //   (entryWallLengthRange.y-entryLengthRange.y)/2);
        Debug.Log(entryToWallLengthRange);
        FrameWallRange entryWallRange = new FrameWallRange(
          new FrameWall(entryToWallLengthRange.x, entryLengthRange.x, entryToWallLengthRange.x),
          new FrameWall(entryToWallLengthRange.y, entryLengthRange.y, entryToWallLengthRange.y)
        );
        FrameWallRange sideWallRange = new FrameWallRange(
          new FrameWall(sideWallLengthRange.x),
          new FrameWall(sideWallLengthRange.y)
        );

        preferredFrameRanges.Add(new FrameRange(new Dictionary<Edge, FrameWallRange>(){
          {Edge.Left, entryWallRange}, {Edge.Right, entryWallRange},
          {Edge.Top, sideWallRange}, {Edge.Bottom, sideWallRange}
        }));
        preferredFrameRanges.Add(new FrameRange(new Dictionary<Edge, FrameWallRange>(){
          {Edge.Left, sideWallRange}, {Edge.Right, sideWallRange},
          {Edge.Top, entryWallRange}, {Edge.Bottom, entryWallRange}
        }));

        return preferredFrameRanges;
    }}
}
