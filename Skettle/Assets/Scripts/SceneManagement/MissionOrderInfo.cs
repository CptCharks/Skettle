using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make the editor inspector force object to be a scene asset field
[CreateAssetMenu(fileName = "MissionOrder", menuName = "MissionData/MissionOrder",order = 0)]
public class MissionOrderInfo : ScriptableObject
{
    public List<MissionLoadInfo> missions;
}

[System.Serializable]
public class MissionLoadInfo
{
    public SceneStartID missionStart;
    public int order;
}