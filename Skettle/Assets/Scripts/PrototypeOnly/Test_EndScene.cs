using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_EndScene : MonoBehaviour
{
    bool levelEnd = false;

    CameraController camera;
    [SerializeField] Image blackEnd;

    public void Start()
    {
        camera = FindObjectOfType<CameraController>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!levelEnd)
        {
            levelEnd = true;
            camera.SetCameraFrozen(true);

            StartCoroutine(FadeOutUIStuff());
        }
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

        Quit();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
