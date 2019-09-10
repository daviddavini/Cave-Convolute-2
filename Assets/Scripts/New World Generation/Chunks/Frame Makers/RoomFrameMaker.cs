using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFrameMaker : FrameMaker
{
    public Vector2Int entryLengthRange;
    public Vector2Int wallLengthRange;
    public bool isSquare;
    public Vector2Int roomCountRange = new Vector2Int(1,4);

    public override List<FrameRange> PreferredFrameRanges { get {
        roomCountRange = new Vector2Int(Mathf.Max(1, roomCountRange.x), Mathf.Min(4, roomCountRange.y));

        List<FrameRange> preferredFrameRanges = new List<FrameRange>();

        Vector2Int entryToWallLengthRange = new Vector2Int((wallLengthRange.x-entryLengthRange.x)/2,
          (wallLengthRange.y-entryLengthRange.y)/2);
        FrameWallRange entryWallRange = new FrameWallRange(
          new FrameWall(entryToWallLengthRange.x, entryLengthRange.x, entryToWallLengthRange.x),
          new FrameWall(entryToWallLengthRange.y, entryLengthRange.y, entryToWallLengthRange.y)
        );
        FrameWallRange wallRange = new FrameWallRange(
          new FrameWall(wallLengthRange.x), new FrameWall(wallLengthRange.y));

        // List<List<FrameWallRange>> wallPermutations = new List<List<FrameWallRange>>(){
        //   new List<FrameWallRange>(){entryWallRange, wallRange, wallRange, wallRange};
        //   new List<FrameWallRange>(){wallRange, entryWallRange, wallRange, wallRange};
        //   new List<FrameWallRange>(){wallRange, wallRange, entryWallRange, wallRange};
        //   new List<FrameWallRange>(){wallRange, wallRange, wallRange, entryWallRange};
        // };
        preferredFrameRanges.Add(new FrameRange(new Dictionary<Edge, FrameWallRange>(){
          {Edge.Left, entryWallRange}, {Edge.Right, entryWallRange},
          {Edge.Top, entryWallRange}, {Edge.Bottom, entryWallRange}
        }));

        return preferredFrameRanges;
    }}
}
