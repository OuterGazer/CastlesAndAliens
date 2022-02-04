using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBehaviour : MonoBehaviour
{
    [SerializeField] GameObject towerMenu;
    [SerializeField] GameObject gameSpeedMenu;
    [SerializeField] GameObject annoymentBars;

    [SerializeField] GameObject redSlowPopUp;
    [SerializeField] GameObject balistaPopUp;
    [SerializeField] GameObject enemyBalistaPopUp;

    private GameObject currentPopUp;


    private bool hasBalistaTutorialAppeared = false;

    private void Awake()
    {
        Messenger.AddListener("Slow Red", OnSlowRedAppearance);
        Messenger.AddListener("Enemy Balistas", OnEnemyBalistaAppearance);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener("Slow Red", OnSlowRedAppearance);
        Messenger.RemoveListener("Enemy Balistas", OnEnemyBalistaAppearance);
    }

    public void OnOkClicked()
    {
        Time.timeScale = 1;

        this.currentPopUp.SetActive(false);
        this.currentPopUp = null;

        this.towerMenu.SetActive(true);
        this.gameSpeedMenu.SetActive(true);
        this.annoymentBars.SetActive(true);

        if (!this.hasBalistaTutorialAppeared)
        {
            ShowBalistaTutorial();
            return;
        }
    }

    private void PopUpWindow(GameObject window, bool isEnemy)
    {
        if (isEnemy)
        {
            //play boo SFX
        }
        else
        {
            //play horaay SFX
        }

        Time.timeScale = 0;

        window.SetActive(true);
        this.currentPopUp = window;

        this.towerMenu.SetActive(false);
        this.gameSpeedMenu.SetActive(false);
        this.annoymentBars.SetActive(true);
    }

    private void OnSlowRedAppearance()
    {
        PopUpWindow(this.redSlowPopUp, true);
    }

    private void ShowBalistaTutorial()
    {
        PopUpWindow(this.balistaPopUp, false);
        this.hasBalistaTutorialAppeared = true;
    }

    private void OnEnemyBalistaAppearance()
    {
        PopUpWindow(this.enemyBalistaPopUp, true);
    }
}
