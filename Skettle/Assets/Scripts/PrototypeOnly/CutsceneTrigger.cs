using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableAsset cutsceneToPlay;
    public PlayableDirector director;
    public CutsceneManager cutsceneManager;
    public GameManager gameManager;

    private void Awake()
    {
        director = FindObjectOfType<PlayableDirector>();
        cutsceneManager = FindObjectOfType<CutsceneManager>();
        gameManager = FindObjectOfType<GameManager>();
    }


    //TODO: Replace the direct use of the director with the cutscene manager
    public void OnTriggerEnter2D(Collider2D collider)
    {
		/*if(gameManager == null)
		{
			gameManager = FindObjectOfType<GameManager>();
		}*/
        director.Play(cutsceneToPlay, DirectorWrapMode.None);
        gameManager.PauseGame(true);
    }

    public void ForcePlayCutscene(PlayableAsset toPlay)
    {
        cutsceneManager.StartCutscene(toPlay);
    }
}
