using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuInteraction : MonoBehaviour
{
    [SerializeField] Image[] towerButtons;
    [SerializeField] TextMeshProUGUI[] towerTexts;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Image item in this.towerButtons)
        {
            item.color = Color.grey;
        }

        SetTextsColorToGrey();
    }

    public void OnClick(Image towerType)
    {
        foreach (Image item in this.towerButtons)
        {
            item.color = Color.grey;
            ChangeButtonColor(item);
            SetTextsColorToGrey();
        }

        towerType.color = Color.white;
        ChangeButtonColor(towerType);
        ChangeTextColor(towerType);
    }

    private void SetTextsColorToGrey()
    {
        foreach (TextMeshProUGUI item in this.towerTexts)
        {
            item.color = Color.grey;
        }
    }

    private void ChangeTextColor(Image towerType)
    {
        for (int i = 0; i < this.towerButtons.Length; i++)
        {
            if (this.towerButtons[i] == towerType)
                this.towerTexts[i].color = Color.white;
        }
    }

    private void ChangeButtonColor(Image buttonImage)
    {
        ColorButtons[] curButton = buttonImage.gameObject.GetComponentsInChildren<ColorButtons>();
        foreach (ColorButtons part in curButton)
        {
            part.UpdateButtonColor();
        }
    }
}
