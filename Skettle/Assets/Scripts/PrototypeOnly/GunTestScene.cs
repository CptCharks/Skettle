using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTestScene : MonoBehaviour
{
    [SerializeField] List<ShootingController> controllers;
    [SerializeField] ShootingController currentController;

    int length;
    int current;

    private void Awake()
    {
        currentController = controllers[0];
        length = controllers.Count;
        current = 0;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            currentController.gun_gun.Fire();
        }

        float scrollMove = Input.GetAxis("Mouse ScrollWheel");

        if (scrollMove > 0)
        {
            current++;
            if(current >= length)
            {
                current = 0;
            }
            currentController = controllers[current];
        } else if(scrollMove < 0)
        {
            current--;
            if (current < 0)
            {
                current = length-1;
            }
            currentController = controllers[current];
        }

    }
}
