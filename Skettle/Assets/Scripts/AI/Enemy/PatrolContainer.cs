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

    public Transform GetClosestNode(Vector3 compare)
    {
        float distance = 10000f;

        Transform toReturn = null;

        foreach(Transform trans in pathNodes)
        {

            float thisDist = Mathf.Abs((trans.position - compare).magnitude);

            if (thisDist < distance)
            {
                distance = thisDist;
                toReturn = trans;
            }
        }

        return toReturn;
    }

    //NOT USABLE YET
    public Vector3 GetClosestPointOnPath(Vector3 compare)
    {

        return Vector3.zero;
    }

    [SerializeField] public List<Transform> pathNodes;
}
