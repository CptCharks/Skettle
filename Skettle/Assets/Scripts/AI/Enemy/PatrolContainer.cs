using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolContainer : MonoBehaviour
{
    public enum PatrolType
    {
        BackAndForth,
        Racetrack
    }

    public PatrolType type;

    public int size;

    public void Awake()
    {
        size = pathNodes.Count;
    }

    [SerializeField] public List<Transform> pathNodes;
}
