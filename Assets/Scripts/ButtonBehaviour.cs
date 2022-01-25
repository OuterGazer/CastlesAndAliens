using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    private float currentTimeScale;

    [SerializeField] GameObject pauseMenu;

    private Image currentImage;
    private Sprite standardSprite;

    private bool isPointerInButton = false;

    // Start is called before the first frame update
    void Awake()
    {
        this.currentImage = this.gameObject.GetComponent<Image>();

        if(this.pauseMenu != null)
            this.pauseMenu.SetActive(false);
    }

    private void Start()
    {
        this.standardSprite = this.currentImage.sprite;
    }

    public void OnPauseClick()
    {
        if(Time.timeScale > 0)
        {
            this.currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
            this.pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = this.currentTimeScale;
            this.pauseMenu.SetActive(false);
        }
        
    }


    // Button Animation Below

    public void OnPointerEnter(Sprite hoverOverSprite)
    {
        this.currentImage.sprite = hoverOverSprite;

        this.isPointerInButton = true;
    }

    public void OnPointerExit()
    {
        this.currentImage.sprite = this.standardSprite;

        this.isPointerInButton = false;
    }

    public void OnPointerDown(Sprite clickedSprite)
    {
        this.currentImage.sprite = clickedSprite;
    }

    public void OnPointerUp(Sprite hoverOverSprite)
    {
        if(this.isPointerInButton)
            this.currentImage.sprite = hoverOverSprite;
        else
            this.currentImage.sprite = this.standardSprite;
    }
}
