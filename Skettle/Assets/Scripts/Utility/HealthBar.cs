using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RectTransform healthBar;
    Image healthBarImage;

    [SerializeField] Color flashColor;
    Color startingColor;

    [SerializeField] float startingWidth;

    public float percent;

    public float startingValue;

    private void Awake()
    {
        startingWidth = healthBar.rect.width;

        healthBarImage = healthBar.GetComponent<Image>();
        startingColor = healthBarImage.color;
    }

    public void UpdateHealthBarPercent(float percent)
    {
        this.percent = percent;
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (startingWidth / 100) * percent);
        StartCoroutine(Flash());
    }

    public void UpdateHealthValue(float value)
    {
        this.percent = (value / startingValue)*100;

        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (startingWidth / 100) * percent);
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        healthBarImage.color = flashColor;

        yield return new WaitForSeconds(0.1f);

        healthBarImage.color = startingColor;
    }
}
