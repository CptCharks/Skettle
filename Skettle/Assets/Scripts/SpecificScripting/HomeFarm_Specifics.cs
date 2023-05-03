using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HomeFarm_Specifics : SceneSpecificsBase
{
    CutsceneManager cutsceneManager;

    public PlayableAsset introCutscene;

    void Awake()
    {
        cutsceneManager = FindObjectOfType<CutsceneManager>();
    }

    public override void SceneStart()
    {
        cutsceneManager.StartCutscene(introCutscene);
    }

    public override void SceneEnd()
    {

    }
}
