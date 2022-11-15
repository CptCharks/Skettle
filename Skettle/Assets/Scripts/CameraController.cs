using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Temp before moving to cinemachine. May still use it just for player control though
public class CameraController : MonoBehaviour
{

    public Transform t_target;
    public Vector3 v3_target;
    public float f_cameraSpeed = 0.5f;
    public float distanceFromTarget = 5f;

    //TODO: use the offset to give the player a further view in the direction they aim
    public Vector4 cameraOffSets;

    [SerializeField] private bool b_freezeCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (t_target == null)
            b_freezeCamera = true;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (b_freezeCamera)
            return;

        v3_target = new Vector3(t_target.position.x, t_target.position.y, t_target.position.z - distanceFromTarget);

        transform.position = Vector3.Lerp(transform.position, v3_target, f_cameraSpeed);
    }

    public void SetCameraFrozen(bool freeze)
    {
        b_freezeCamera = freeze;

        if(!b_freezeCamera && transform == null)
        {
            b_freezeCamera = true;
            Debug.LogError("Camera doesn't have any valid target. Remaining frozen");
        }
    }
}
