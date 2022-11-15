using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="SceneStartID",menuName = "SceneManagement/SceneStartID", order=0)]
public class SceneStartID : ScriptableObject
{
    public string sceneName;
    public string startID;
}
