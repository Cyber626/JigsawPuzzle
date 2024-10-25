using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class ProgressBarCircle : MonoBehaviour {
    [Header("Bar Setting")]
    public Color BarColor;
    public Color BarBackGroundColor;
    public Color MaskColor;
    public Sprite BarBackGroundSprite;
    public bool doAlert = false;
    public int Alert = 20;
    public Color BarAlertColor;

    private Image bar, barBackground,Mask;
    private TextMeshProUGUI txtTitle;
    private float barValue;
    public float BarValue
    {
        get { return barValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValue = value;
            UpdateValue(barValue);

        }
    }

    private void Awake()
    {
        txtTitle = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        barBackground = transform.Find("BarBackgroundCircle").GetComponent<Image>();
        bar = transform.Find("BarCircle").GetComponent<Image>();
        Mask= transform.Find("Mask").GetComponent<Image>();
    }

    private void Start()
    {
        bar.color = BarColor;
        Mask.color = MaskColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(barValue);
    }

    void UpdateValue(float val)
    {
       
        bar.fillAmount = -(val / 100) + 1f;

        txtTitle.text = val + "%";

        if (doAlert && Alert >= val)
        {
            barBackground.color = BarAlertColor;
        }
        else
        {
            barBackground.color = BarBackGroundColor;
        }
    }
}
