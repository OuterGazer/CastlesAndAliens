using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuInteraction : MonoBehaviour
{
    [SerializeField] Image[] towerButtons;
    [SerializeField] TextMeshProUGUI[] towerTexts;
    private Color selectedColor = Color.white;
    private Color unselectedColor = Color.grey;
    private Color hoverOverColor = new Color32(192, 192, 192, 255);

    private bool[] isButtonActivated = new bool[3];

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Image item in this.towerButtons)
        {
            item.color = this.unselectedColor;
        }

        SetTextsColorToGrey();
    }

    private void SetColor(Image towerType, Color color, bool isHoverOver)
    {
        if(!isHoverOver)
            SetAllToGrey();

        towerType.color = color;
        ChangeButtonColor(towerType);
        ChangeTextColor(towerType, color);
    }
    private void SetAllToGrey()
    {
        foreach (Image item in this.towerButtons)
        {
            item.color = this.unselectedColor;
            ChangeButtonColor(item);
            SetTextsColorToGrey();
        }

        for(int i = 0; i < this.isButtonActivated.Length; i++)
        {
            this.isButtonActivated[i] = false;
        }
    }

    private void SetTextsColorToGrey()
    {
        foreach (TextMeshProUGUI item in this.towerTexts)
        {
            item.color = this.unselectedColor;
        }
    }

    private void ChangeTextColor(Image towerType, Color color)
    {
        for (int i = 0; i < this.towerButtons.Length; i++)
        {
            if (this.towerButtons[i] == towerType)
            {
                this.towerTexts[i].color = color;
               
                if(color == this.selectedColor)
                    this.isButtonActivated[i] = true;
            }
                
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
    private void SetHoverOverColor(Image towerType, Color color)
    {
        for (int i = 0; i < this.towerButtons.Length; i++)
        {
            if (this.towerButtons[i] == towerType)
            {
                if (!this.isButtonActivated[i])
                    SetColor(towerType, color, true);
            }
        }
    }

    public void OnClick(Image towerType)
    {
        SetColor(towerType, this.selectedColor, false);
    }

    public void OnPointerEnter(Image towerType)
    {
        SetHoverOverColor(towerType, this.hoverOverColor);
    }    

    public void OnPointerExit(Image towerType)
    {
        SetHoverOverColor(towerType, this.unselectedColor);
    }
}
