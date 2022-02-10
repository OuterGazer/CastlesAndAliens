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
    [SerializeField] GameObject redNormalPopUp;
    [SerializeField] GameObject cannonPopUp;
    [SerializeField] GameObject enemyCannonPopUp;
    [SerializeField] GameObject redFastPopUp;
    [SerializeField] GameObject catapultPopUp;
    [SerializeField] GameObject whiteEnemiesPopUp;
    [SerializeField] GameObject enemyMissilePopUp;
    [SerializeField] GameObject lastRushPopUp;
    [SerializeField] GameObject victoryPopUp;

    [SerializeField] AudioClip enemyTutorialSFX;
    [SerializeField] AudioClip towerTutorialSFX;
    [SerializeField] AudioClip warningSFX;

    private GameObject currentPopUp;
    private float currentGameSpeed = 1;

    private AudioSource audioSource;

    private bool hasBalistaTutorialAppeared = false;

    private void Awake()
    {
        Messenger<float>.AddListener("Change Game Speed", OnGameSpeedChange);

        Messenger.AddListener("Slow Red", OnSlowRedAppearance);
        Messenger.AddListener("Enemy Balistas", OnEnemyBalistaAppearance);
        Messenger.AddListener("Normal Red", OnNormalRedAppearance);
        Messenger.AddListener("Cannon Tower", ShowCannonTutorial);
        Messenger.AddListener("Enemy Cannons", OnEnemyCannonAppearance);
        Messenger.AddListener("Fast Red", OnFastRedAppearance);
        Messenger.AddListener("Catapult Tower", ShowCatapultTutorial);
        Messenger.AddListener("White Enemies", OnWhiteEnemiesAppearance);
        Messenger.AddListener("Enemy Missiles", OnEnemyMissileAppearance);
        Messenger.AddListener("Last Rush", OnLastRushAppearance);
        Messenger.AddListener("Victory", OnVictoryAppearance);

        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener("Change Game Speed", OnGameSpeedChange);

        Messenger.RemoveListener("Slow Red", OnSlowRedAppearance);
        Messenger.RemoveListener("Enemy Balistas", OnEnemyBalistaAppearance);
        Messenger.RemoveListener("Normal Red", OnNormalRedAppearance);
        Messenger.RemoveListener("Cannon Tower", ShowCannonTutorial);
        Messenger.RemoveListener("Enemy Cannons", OnEnemyCannonAppearance);
        Messenger.RemoveListener("Fast Red", OnFastRedAppearance);
        Messenger.RemoveListener("Catapult Tower", ShowCatapultTutorial);
        Messenger.RemoveListener("White Enemies", OnWhiteEnemiesAppearance);
        Messenger.RemoveListener("Enemy Missiles", OnEnemyMissileAppearance);
        Messenger.RemoveListener("Last Rush", OnLastRushAppearance);
        Messenger.RemoveListener("Victory", OnVictoryAppearance);
    }

    private void OnGameSpeedChange(float inGameSpeed)
    {
        this.currentGameSpeed = inGameSpeed;
    }

    public void OnOkClicked()
    {
        Time.timeScale = this.currentGameSpeed;

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
        Time.timeScale = 0;

        window.SetActive(true);
        this.currentPopUp = window;

        PlayPopUpSFX(isEnemy);

        this.towerMenu.SetActive(false);
        this.gameSpeedMenu.SetActive(false);
        this.annoymentBars.SetActive(true);
    }

    private void PlayPopUpSFX(bool isEnemy)
    {
        if (isEnemy)
        {
            if (!this.gameObject.name.Contains("Rush"))
                this.audioSource.PlayOneShot(this.enemyTutorialSFX);
            else
                this.audioSource.PlayOneShot(this.warningSFX);
        }
        else
        {
            this.audioSource.PlayOneShot(this.towerTutorialSFX);
        }
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

    private void OnNormalRedAppearance()
    {
        PopUpWindow(this.redNormalPopUp, true);
    }

    private void ShowCannonTutorial()
    {
        PopUpWindow(this.cannonPopUp, false);
    }
    private void OnEnemyCannonAppearance()
    {
        PopUpWindow(this.enemyCannonPopUp, true);
    }

    private void OnFastRedAppearance()
    {
        PopUpWindow(this.redFastPopUp, true);
    }

    private void ShowCatapultTutorial()
    {
        PopUpWindow(this.catapultPopUp, false);
    }

    private void OnWhiteEnemiesAppearance()
    {
        PopUpWindow(this.whiteEnemiesPopUp, true);
    }

    private void OnEnemyMissileAppearance()
    {
        PopUpWindow(this.enemyMissilePopUp, true);
    }

    private void OnLastRushAppearance()
    {
        PopUpWindow(this.lastRushPopUp, true);
    }

    private void OnVictoryAppearance()
    {
        PopUpWindow(this.victoryPopUp, false);
    }
}
