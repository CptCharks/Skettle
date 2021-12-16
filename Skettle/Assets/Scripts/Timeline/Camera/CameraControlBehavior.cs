using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CameraControlBehavior : PlayableBehaviour
{
    [SerializeField]
    private Vector3 position;

    private bool firstFrameHappend;
    private Vector3 defaultPosition;

    private Camera camera;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        camera = playerData as Camera;

        if (camera == null)
            return;

        if(!firstFrameHappend)
        {
            defaultPosition = camera.transform.position;

            firstFrameHappend = true;
        }

        camera.transform.position = position;
    }


    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        firstFrameHappend = false;

        if (camera == null)
            return;

        camera.transform.position = defaultPosition;

        base.OnBehaviourPause(playable, info);
    }
}
