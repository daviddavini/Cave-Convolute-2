using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntryCountPreference {None, Min, Max};
public abstract class FrameMaker : MonoBehaviour
{
    public virtual List<Frame> PreferredFrames(System.Random random)
    {
        return new List<Frame>();
    }

    public Frame TryGetFrame(FrameRange frameRangeConstraints, EntryCountPreference ecp, System.Random random = null)
    {
        random = random ?? new System.Random();

        List<Frame> preferredFrames = PreferredFrames(random);
        preferredFrames.Shuffle(random);
        if (ecp == EntryCountPreference.Min)
            preferredFrames.Sort((f, g) => f.EntryCount - g.EntryCount);
        else if (ecp == EntryCountPreference.Max)
            preferredFrames.Sort((f, g) => g.EntryCount - f.EntryCount);

        //Debug.Log(preferredFrames[0].EntryCount);

        // Debug.Log(preferredFrames[0]);
        // Debug.Log(frameRangeConstraints);
        // Debug.Log("TryGetFrame: " + frameRangeConstraints.Contains(preferredFrames[0]));

        foreach(Frame preferredFrame in preferredFrames)
        {
            if(frameRangeConstraints.Contains(preferredFrame))
                return preferredFrame;
        }
        return null;
    }

    public virtual List<FrameRange> PreferredFrameRanges {get;}

    public Frame TryMakeFrame(FrameRange frameRangeConstraints, System.Random random = null)
    {
        random = random ?? new System.Random();

        List<FrameRange> preferredFrameRanges = PreferredFrameRanges;
        preferredFrameRanges.Shuffle(random);

        //Debug.Log("Constraints: " + frameRangeConstraints);

        foreach(FrameRange preferredFrameRange in preferredFrameRanges)
        {
            FrameRange possibleFrameRange = frameRangeConstraints.Intersection(preferredFrameRange);

            //Debug.Log("Possible FR: " + possibleFrameRange);
            if (!possibleFrameRange.IsEmpty){
                Frame chosenFrame = possibleFrameRange.RandomFrame(random);
                //Debug.Log("Chosen Frame: " + chosenFrame);
                return chosenFrame;
            }
        }
        return null;
    }
}
