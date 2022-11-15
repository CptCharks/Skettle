using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SceneStartPoint : MonoBehaviour
{
    public SceneStartID sceneStartID;
    public Transform playerExitPoint;
    public SceneStartID linkedPoint;

    GameManager gameManagerRef;

    public bool isEnabled = true;

    [SerializeField] public bool alreadyTriggered = false;

    public UnityEvent onUsedWhenDisabled;


    void Awake()
    {
        gameManagerRef = FindObjectOfType<GameManager>();
    }

    public virtual void LoadLinkedPoint()
    {
        if (!isEnabled)
        {
            onUsedWhenDisabled.Invoke();
        }
        else if (!alreadyTriggered)
        {
            alreadyTriggered = true;
            gameManagerRef.LoadLevel(linkedPoint);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (playerExitPoint != null)
            Gizmos.DrawWireSphere(playerExitPoint.position,0.2f);
    }
}
