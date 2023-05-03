using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunStoreManager : MonoBehaviour, I_ProgressConditional
{
    //Information
    ProgressContainer progress;
    public GunPrices gunPrices;
    public GunPrices ammoPrices;

    //Menu panels
    public GameObject menuPanel;
    List<GameObject> gunButtons = new List<GameObject>();
    public Button gunsTabButton;
    public Button ammoTabButton;
    public Button confirmPurchaseButton;


    public GameObject gunButton_pf;
    public GameObject ammoButton_pf;
    [SerializeField] Transform contentPanel;

    public TextMeshProUGUI playerMoney;
    public TextMeshProUGUI spendMoney;

    bool menuState = false;
    bool gunsTabOpen = true;

    GameManager gameManager;

    public UnityEvent onStoreExit = new UnityEvent();

    int readyToBuyGun = -1;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        spendMoney.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);

        gunsTabButton.onClick.AddListener(() => ShowGunOrAmmoPanel(true));
        ammoTabButton.onClick.AddListener(() => ShowGunOrAmmoPanel(false));
    }

    void Update()
    {
        //Add options to cancel purchase with right click, esc, or something
    }

    public void OpenCloseMenu(bool state)
    {
        menuState = state;
        menuPanel.SetActive(state);

        gameManager.SetEnabledPlayerControls(!state);

        RefreshPlayerMoney();

        if (state)
        {
            gunsTabOpen = true;
            RefreshGunPanels();
        }
        else
        {
            ShowGunOrAmmoPanel(true);

            foreach(GameObject go in gunButtons)
            {
                Destroy(go);
            }
            gunButtons.Clear();

            onStoreExit.Invoke();
        }
    }

    public void RefreshGunPanels()
    {
        foreach (GameObject go in gunButtons)
        {
            Destroy(go);
        }
        gunButtons.Clear();

        LoadGunOptions();
    }

    public void RefreshAmmoPanels()
    {
        foreach (GameObject go in gunButtons)
        {
            Destroy(go);
        }
        gunButtons.Clear();

        LoadAmmoOptions();
    }

    void LoadGunOptions()
    {
        for(int i = 0; i < 5; i++)
        {
            if (!progress.playerData.weaponOwnership[i])
            {
                var panel = Instantiate(gunButton_pf, contentPanel);
                panel.GetComponent<GunListElement>().SetupElement(this, gunPrices.gunPrices[i].gunName, gunPrices.gunPrices[i].gunPrice, i);
                gunButtons.Add(panel);
            }
        }
    }

    void LoadAmmoOptions()
    {
        for(int i = 1; i < 5; i++)
        {
            if(progress.playerData.weaponOwnership[i])
            {
                var panel = Instantiate(ammoButton_pf, contentPanel);
                panel.GetComponent<GunListElement>().SetupElement(this, ammoPrices.gunPrices[i].gunName, ammoPrices.gunPrices[i].gunPrice, i);
                gunButtons.Add(panel);
            }
        }
    }

    void RefreshPlayerMoney()
    {
        playerMoney.text = "$" + progress.playerData.playerMoney.ToString();
    }

    public void ShowGunOrAmmoPanel(bool showGunPanel)
    {
        spendMoney.gameObject.SetActive(false);
        gunsTabOpen = showGunPanel;

        if (showGunPanel)
        {
            RefreshGunPanels();
            gunsTabButton.interactable = false;
            ammoTabButton.interactable = true;
        }
        else
        {
            RefreshAmmoPanels();
            ammoTabButton.interactable = false;
            gunsTabButton.interactable = true;
        }
    }

    public void ShowOrHide(ProgressContainer data)
    {
        progress = data;
    }

    public void PurchaseWeapon()
    {
        if (gunsTabOpen)
        {
            if(gunPrices.gunPrices[readyToBuyGun].gunPrice > progress.playerData.playerMoney)
            {
                //Make error noises and stuff
                return;
            }
            gameManager.PurchaseWeapon(readyToBuyGun);
            UnselectOptions();
            RefreshGunPanels();
            RefreshPlayerMoney();
            spendMoney.gameObject.SetActive(false);
        }
        else
        {
            if(ammoPrices.gunPrices[readyToBuyGun].gunPrice > progress.playerData.playerMoney)
            {
                //Make error noises and stuff
                return;
            }
            gameManager.PurchaseAmmo(readyToBuyGun);
            RefreshPlayerMoney();
        }
    }

    //Probably didn't need seperate ones for guns and ammo. Only the purchaseWeapon funciton needs to check for differences
    public void ShowBuyButton(int gunEnum)
    {
        //Consider making this check the player's money against the weapon price
        if(gunEnum <= -1)
        {
            //Play error sound effect
            return;
        }

        readyToBuyGun = gunEnum;

        spendMoney.gameObject.SetActive(true);
        spendMoney.text = "-$" + gunPrices.gunPrices[readyToBuyGun].gunPrice;

        confirmPurchaseButton.interactable = true;
    }

    public void ShowBuyButtonAmmo(int gunEnum)
    {
        //Consider making this check the player's money against the weapon price
        if (gunEnum <= -1)
        {
            //Play error sound effect
            return;
        }

        readyToBuyGun = gunEnum;

        spendMoney.gameObject.SetActive(true);
        spendMoney.text = "-$" + ammoPrices.gunPrices[readyToBuyGun].gunPrice;

        confirmPurchaseButton.interactable = true;
    }

    public void UnselectOptions()
    {
        readyToBuyGun = -1;

        confirmPurchaseButton.interactable = false;

        foreach(GameObject go in gunButtons)
        {
            go.GetComponent<GunListElement>().ResetPanel();
        }
    }
}
