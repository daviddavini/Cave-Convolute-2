using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct FrameWall
{
    public int Length {get;}
    public bool HasEntry {get;}
    public int PreWall {get;}
    public int Entry {get;}
    public int PostWall {get;}

    public FrameWall(int length)
    {
        Length = length;
        HasEntry = false;
        Entry = 0;
        PreWall = Length / 2;
        PostWall = Length / 2 + Length % 2;
    }

    //gotta check for overflow man!
    public FrameWall(int preWall, int entry, int postWall)
    {
        if(entry == 0) {
            Length = preWall + entry + postWall;
            HasEntry = false;
            Entry = 0;
            PreWall = Length / 2;
            PostWall = Length / 2 + Length % 2;
        } else {
            try{
                Length = checked(preWall + entry + postWall);
            }catch(OverflowException){
                Length = int.MaxValue;
                //Debug.Log("overflow caught");
            }
            HasEntry = true;
            PreWall = preWall;
            Entry = entry;
            PostWall = postWall;
        }
    }

    public int GetWallBySign(Sign sign)
    {
        switch(sign)
        {
            case Sign.Negative: return PreWall;
            case Sign.Positive: return PostWall;
            default: return -1;
        }
    }

    public FrameWall WithEntryRemoved()
    {
        return new FrameWall(Length);
    }

    public bool LessThanOrEqualTo(FrameWall frameWall)
    {
        return (Length <= frameWall.Length
          && (!HasEntry || frameWall.HasEntry)
          && PreWall <= frameWall.PreWall
          && Entry <= frameWall.Entry
          && PostWall <= frameWall.PostWall);
    }

    public static FrameWall Min(FrameWall wall1, FrameWall wall2)
    {
        //WARNING LIKELY WONT WORK
        int entry = Mathf.Min(wall1.Entry, wall2.Entry);
        int preWall = Mathf.Min(wall1.PreWall, wall2.PreWall);
        int postWall = Mathf.Min(wall1.PostWall, wall2.PostWall);
        return new FrameWall(preWall, entry, postWall);
    }

    public static FrameWall Max(FrameWall wall1, FrameWall wall2)
    {
        // TODO Might need to remove entry from length
        int entry = Mathf.Max(wall1.Entry, wall2.Entry);
        int preWall = Mathf.Max(wall1.PreWall, wall2.PreWall);
        int postWall = Mathf.Max(wall1.PostWall, wall2.PostWall);
        return new FrameWall(preWall, entry, postWall);
    }

    public override string ToString()
    {
        if(HasEntry){
            string preWall = PreWall < int.MaxValue ? PreWall.ToString() : "∞";
            string entry = Entry < int.MaxValue ? Entry.ToString() : "∞";
            string postWall = PostWall < int.MaxValue ? PostWall.ToString() : "∞";
            return string.Format("| - {0} - [{1}] - {2} - |", preWall, entry, postWall);
        }
        else {
            string length = Length < int.MaxValue ? Length.ToString() : "∞";
            return string.Format("| - {0} - |", length);
        }
    }

    public static FrameWall MaxValue = new FrameWall(int.MaxValue, int.MaxValue, int.MaxValue);
    public static FrameWall MinValue = new FrameWall(0);
}
