using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    private float currentTimeScale;

    Image currentImage;
    [SerializeField] Sprite standardSprite;
    [SerializeField] Sprite hoverOverSprite;
    [SerializeField] Sprite clickedSprite;

    private bool isPointerInButton = false;

    // Start is called before the first frame update
    void Awake()
    {
        this.currentImage = this.gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        this.currentImage.sprite = this.standardSprite;
    }

    public void OnPauseClick()
    {
        if(Time.timeScale > 0)
        {
            this.currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = this.currentTimeScale;
        }
        
    }


    // Button Animation Below

    public void OnPointerEnter()
    {
        this.currentImage.sprite = this.hoverOverSprite;

        this.isPointerInButton = true;
    }

    public void OnPointerExit()
    {
        this.currentImage.sprite = this.standardSprite;

        this.isPointerInButton = false;
    }

    public void OnPointerDown()
    {
        this.currentImage.sprite = this.clickedSprite;
    }

    public void OnPointerUp()
    {
        if(this.isPointerInButton)
            this.currentImage.sprite = this.hoverOverSprite;
        else
            this.currentImage.sprite = this.standardSprite;
    }
}
