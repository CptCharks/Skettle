using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Transform exitLocation;
    public Door linkedDoor;

    [SerializeField] Image blackEnd;
    CameraController camera;
    PlayerController player;


    private void Awake()
    {
        camera = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        camera.SetCameraFrozen(true);
        player.b_controlsDisabled = true;

        StopCoroutine(FadeOutUIStuff());
        StartCoroutine(FadeOutUIStuff());
    }

    IEnumerator FadeOutUIStuff()
    {
        Color current = blackEnd.color;

        while (blackEnd.color.a < 1)
        {
            current.a += 0.02f; //= new Color(current.r, current.g, current.b, op);

            blackEnd.color = current;

            yield return new WaitForFixedUpdate();
        }

        //Bit slow since it's waiting for the camera to automatically move
        player.transform.SetPositionAndRotation(linkedDoor.exitLocation.position, player.transform.rotation);
        camera.SetCameraFrozen(false);
        yield return new WaitForSeconds(1f);

        while (blackEnd.color.a > 0)
        {
            current.a -= 0.02f; //= new Color(current.r, current.g, current.b, op);

            blackEnd.color = current;

            yield return new WaitForFixedUpdate();
        }

        player.b_controlsDisabled = false;

    }


}
