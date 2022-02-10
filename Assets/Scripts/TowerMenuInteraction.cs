using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuInteraction : MonoBehaviour
{
    [SerializeField] Image[] towerButtons;
    [SerializeField] TextMeshProUGUI[] towerTexts;
    [SerializeField] Image[] coinImages;
    [SerializeField] GameObject[] towerPrefabs;
    [SerializeField] GameObject[] previewPrefabs;
    private Color selectedColor = Color.white;
    private Color unselectedColor = Color.grey;
    private Color hoverOverColor = new Color32(192, 192, 192, 255);

    [SerializeField] AudioClip hoverOverSFX;
    [SerializeField] AudioClip clickSFX;

    private AudioSource audioSource;

    private bool[] isButtonActivated = new bool[3];

    // Start is called before the first frame update
    void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        Messenger.AddListener("UnlockCannonTower", OnCannonTowerUnlocked);
        Messenger.AddListener("UnlockCatapultTower", OnCatapultTowerUnlocked);

        foreach (Image item in this.towerButtons)
        {
            if(item.color != Color.black)
                item.color = this.unselectedColor;
        }

        SetTextsColorToGrey();
        SetCoinsColorToGrey();
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
            if (item.color != Color.black)
            {
                item.color = this.unselectedColor;
                ChangeButtonColor(item);
                SetTextsColorToGrey();
                SetCoinsColorToGrey();
            }                
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
            if(item.gameObject.activeSelf)
                item.color = this.unselectedColor;
        }
    }

    private void SetCoinsColorToGrey()
    {
        foreach (Image item in this.coinImages)
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
                this.coinImages[i].color = color;
               
                if(color == this.selectedColor)
                {
                    this.isButtonActivated[i] = true;
                    this.audioSource.PlayOneShot(this.clickSFX);

                    Messenger<GameObject>.Broadcast("SetTowerType", this.towerPrefabs[i]);
                    Messenger<GameObject>.Broadcast("SetPreviewType", this.previewPrefabs[i]);
                }
                    
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
            if (!this.towerTexts[i].gameObject.activeSelf) { return; }

            if (this.towerButtons[i] == towerType)
            {
                if (!this.isButtonActivated[i])
                {
                    this.audioSource.PlayOneShot(this.hoverOverSFX);
                    SetColor(towerType, color, true);
                }                    
            }
        }
    }

    public void OnClick(Image towerType)
    {
        if(!Mathf.Approximately(Time.timeScale, 0))
            SetColor(towerType, this.selectedColor, false);
    }

    public void OnPointerEnter(Image towerType)
    {
        if (!Mathf.Approximately(Time.timeScale, 0))
            SetHoverOverColor(towerType, this.hoverOverColor);
    }    

    public void OnPointerExit(Image towerType)
    {
        if (!Mathf.Approximately(Time.timeScale, 0))
            SetHoverOverColor(towerType, this.unselectedColor);
    }

    private void OnCannonTowerUnlocked()
    {
        this.towerTexts[1].gameObject.SetActive(true);
        this.towerButtons[1].GetComponent<Button>().interactable = true;
        SetColor(this.towerButtons[1], this.unselectedColor, false);
    }

    private void OnCatapultTowerUnlocked()
    {
        this.towerTexts[2].gameObject.SetActive(true);
        this.towerButtons[2].GetComponent<Button>().interactable = true;
        SetColor(this.towerButtons[2], this.unselectedColor, false);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener("UnlockCannonTower", OnCannonTowerUnlocked);
        Messenger.RemoveListener("UnlockCatapultTower", OnCatapultTowerUnlocked);
    }
}
