using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class OnLevelEnter : MonoBehaviour
{
    [SerializeField] public bool levelHasBeenStarted;

    public UnityEvent onLevelLoaded;

    public void Start()
    {
        if(!levelHasBeenStarted)
        {
            onLevelLoaded.Invoke();
            levelHasBeenStarted = true;
        }
    }



}
