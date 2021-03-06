using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject towerMenu;
    [SerializeField] GameObject speedMenu;
    [SerializeField] GameObject annoymentSliders;
    [SerializeField] GameObject infoPopUp;
    [SerializeField] GameObject settingsPopUp;
    [SerializeField] GameObject[] instructionsWindows;

    private GameObject currentWindow;
    private int instructionsPageIndex = 0;

    private void Awake()
    {
        this.towerMenu.SetActive(false);
        this.speedMenu.SetActive(false);
        this.annoymentSliders.SetActive(false);

        Time.timeScale = 0;
    }

    public void OnReturnToMainMenuClick()
    {
        this.instructionsPageIndex = 0;

        this.currentWindow.SetActive(false);
        this.currentWindow = null;

        this.gameObject.SetActive(true);
    }

    public void OnStartGameClick()
    {
        this.towerMenu.SetActive(true);
        this.speedMenu.SetActive(true);
        this.annoymentSliders.SetActive(true);

        this.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    public void OnSettingsClick()
    {
        this.gameObject.SetActive(false);

        this.settingsPopUp.SetActive(true);
        Messenger<bool>.Broadcast("SettingsHasPoppedUp", true);
    }

    public void OnInfoPopUpClick()
    {
        this.gameObject.SetActive(false);

        this.infoPopUp.SetActive(true);
        this.currentWindow = this.infoPopUp;
    }

    public void OnInstructionsClick()
    {
        this.gameObject.SetActive(false);

        this.instructionsWindows[this.instructionsPageIndex].SetActive(true);
        this.currentWindow = this.instructionsWindows[this.instructionsPageIndex];
    }

    public void OnNextPageClick()
    {
        this.currentWindow.SetActive(false);

        this.instructionsPageIndex++;

        this.instructionsWindows[this.instructionsPageIndex].SetActive(true);
        this.currentWindow = this.instructionsWindows[this.instructionsPageIndex];        
    }

    public void OnPreviousPageClick()
    {
        this.currentWindow.SetActive(false);

        this.instructionsPageIndex--;

        this.instructionsWindows[this.instructionsPageIndex].SetActive(true);
        this.currentWindow = this.instructionsWindows[this.instructionsPageIndex];
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnQuiteGameClick()
    {
        Application.Quit();
    }
}
