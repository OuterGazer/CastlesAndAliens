using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonBehaviour : MonoBehaviour
{
    private float currentTimeScale;
    public float CurrentTimeScale => this.currentTimeScale;

    [SerializeField] Sprite blockedButton;
    [SerializeField] GameObject speedButton;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject restartMenu;

    private Image currentImage;
    private Sprite standardSprite;
    

    private bool isPointerInButton = false;
    private bool isButtonBlocked = false;
    public bool IsButtonBlocked
    {
        get { return this.IsButtonBlocked; }
        set { this.isButtonBlocked = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        this.currentImage = this.gameObject.GetComponent<Image>();

        if(this.pauseMenu != null)
        {
            this.pauseMenu.SetActive(false);
        }
            

        if (this.restartMenu != null)
            this.restartMenu.SetActive(false);
    }

    private void OnEnable()
    {
        if(this.standardSprite != null)
            this.currentImage.sprite = this.standardSprite;
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

            this.gameObject.GetComponent<Button>().enabled = false;
            this.speedButton.GetComponent<Button>().enabled = false;
            
            this.currentImage.sprite = this.blockedButton;
            this.speedButton.GetComponent<Image>().sprite = this.blockedButton;
            
            this.isButtonBlocked = true;
            this.speedButton.GetComponent<ButtonBehaviour>().IsButtonBlocked = true;
        }
        else
        {
            Time.timeScale = this.currentTimeScale;
            this.pauseMenu.SetActive(false);

            this.gameObject.GetComponent<Button>().enabled = true;
            this.speedButton.GetComponent<Button>().enabled = true;

            this.currentImage.sprite = this.standardSprite;
            this.speedButton.GetComponent<Image>().sprite = this.standardSprite;
            
            this.isButtonBlocked = false;
            this.speedButton.GetComponent<ButtonBehaviour>().IsButtonBlocked = false;
        }
        
    }

    public void OnRestartClick()
    {
        if (!this.restartMenu.activeInHierarchy)
        {
            this.pauseMenu.SetActive(false);
            this.restartMenu.SetActive(true);
        }
        else
        {
            this.pauseMenu.SetActive(true);
            this.restartMenu.SetActive(false);
        }            
    }

    public void OnConfirmRestartClick()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnSpeedChangeClick()
    {
        if(Mathf.Approximately(Time.timeScale, 1))
        {
            Time.timeScale = 2;
            this.speedText.text = "Speed x4";
        }
        else if (Mathf.Approximately(Time.timeScale, 2))
        {
            Time.timeScale = 4;
            this.speedText.text = "Speed x1";
        }
        else
        {
            Time.timeScale = 1;
            this.speedText.text = "Speed x2";
        }

        Messenger<float>.Broadcast("Change Game Speed", Time.timeScale);
            
    }


    // Button Animation Below

    public void OnPointerEnter(Sprite hoverOverSprite)
    {
        if(this.isButtonBlocked) { return; }

        this.currentImage.sprite = hoverOverSprite;

        this.isPointerInButton = true;
    }

    public void OnPointerExit()
    {
        if (this.isButtonBlocked) { return; }

        this.currentImage.sprite = this.standardSprite;

        this.isPointerInButton = false;
    }

    public void OnPointerDown(Sprite clickedSprite)
    {
        if (this.isButtonBlocked) { return; }

        this.currentImage.sprite = clickedSprite;
    }

    public void OnPointerUp(Sprite hoverOverSprite)
    {
        if (this.isButtonBlocked) { return; }

        if (this.isPointerInButton)
            this.currentImage.sprite = hoverOverSprite;
        else
            this.currentImage.sprite = this.standardSprite;
    }
}
