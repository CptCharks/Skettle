using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoEnd : MonoBehaviour
{
    [SerializeField] int deathCount = 0;

    public GameObject endScreenUI;
    public TextMeshProUGUI deathCountText;

    GameManager gm;

    public void AddDeath()
    {
        deathCount++;
    }

    public void DemoFinished()
    {
        gm = FindObjectOfType<GameManager>();
        StartCoroutine(FinishSequence());
    }

    IEnumerator FinishSequence()
    {
        yield return new WaitForSeconds(1f);
        gm.SetEnabledPlayerControls(false);


        yield return new WaitForSeconds(1f);

        endScreenUI.SetActive(true);
        deathCountText.text = deathCount.ToString();

        yield return new WaitForSeconds(8f);

        gm.FadeOutUI(Application.Quit);
    }

}
