using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelperFunctions : object
{
    public static GameObject FindObject(this GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindUIObject(this GameObject parent, string name)
    {
        RectTransform[] trs = parent.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
