using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour
{
    PlayableDirector director;
    GameManager manager;

    UnityEvent onCutsceneEnd = new UnityEvent();

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();

        director = GetComponent<PlayableDirector>();
        director.stopped += EndCutscene;
    }

    public void StartCutscene(PlayableAsset timeline, UnityAction callback = null)
    {
        if(callback != null)
        {
            onCutsceneEnd.AddListener(callback);
        }

        manager.PauseGame(true);
        director.Play(timeline, DirectorWrapMode.None);
    }

    public void EarlyCutsceneEnd()
    {
        manager.PauseGame(false);
        onCutsceneEnd.Invoke();
        onCutsceneEnd.RemoveAllListeners();
    }

    public void EndCutscene(PlayableDirector callingDirector)
    {
        manager.PauseGame(false);
        onCutsceneEnd.Invoke();
        onCutsceneEnd.RemoveAllListeners();
    }
}
