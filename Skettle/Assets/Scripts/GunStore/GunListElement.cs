using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunListElement : MonoBehaviour
{
    GunStoreManager parent;

    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI gunCostText;

    public GunEnum gun;

    public void SetupElement(GunStoreManager manager, string gunName, int cost, int gunEnum)
    {
        parent = manager;
        gunNameText.text = gunName;
        gunCostText.text = "$ " + cost;
        gun = (GunEnum)gunEnum;
    }

    public void OnPress()
    {
        parent.UnselectOptions();
        parent.ShowBuyButton((int)gun);

        //Or change the color
        GetComponent<Button>().interactable = false;
    }

    public void OnPressAmmo()
    {
        parent.UnselectOptions();
        parent.ShowBuyButtonAmmo((int)gun);

        //Or change the color
        GetComponent<Button>().interactable = false;
    }

    public void ResetPanel()
    {


        GetComponent<Button>().interactable = true;
    }

    public void RemoveGunPanel()
    {
        Destroy(gameObject);
    }
}
