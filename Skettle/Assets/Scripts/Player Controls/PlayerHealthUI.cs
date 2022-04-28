using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    Hittable playerHeatlh;

    private void Awake()
    {
        playerHeatlh = GetComponentInChildren<Hittable>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + playerHeatlh.tempHit.ToString("0");
    }
}
