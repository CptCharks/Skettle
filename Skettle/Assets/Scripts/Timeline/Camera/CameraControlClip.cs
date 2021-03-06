using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CameraControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private CameraControlBehavior template = new CameraControlBehavior();

    public ClipCaps clipCaps
    { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<CameraControlBehavior>.Create(graph, template);
    }
}
