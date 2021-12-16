using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableAsset cutsceneToPlay;
    public PlayableDirector director;
    public GameManager gameManager;

    private void Awake()
    {
        director = FindObjectOfType<PlayableDirector>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        director.Play(cutsceneToPlay, DirectorWrapMode.None);
        gameManager.PauseGame(true);
    }
}
