using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] Slider soundSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioClip testSFX;

    private AudioSource gameMusicTrack;

    private bool cameFromMainMenu = false;

    private void Awake()
    {
        Messenger<bool>.AddListener("SettingsHasPoppedUp", OnSettingsEnabled);
        
        this.gameMusicTrack = this.gameObject.GetComponent<AudioSource>();

        //PlayerPrefs.SetFloat("Music", 0.25f);
        //PlayerPrefs.SetFloat("SFX", 0.65f);

        this.gameMusicTrack.volume = PlayerPrefs.GetFloat("Music", 0.250f);
        AudioListener.volume = PlayerPrefs.GetFloat("SFX", 0.65f);
    }

    private void OnDestroy()
    {
        Messenger<bool>.RemoveListener("SettingsHasPoppedUp", OnSettingsEnabled);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameMusicTrack.ignoreListenerPause = true;
        this.gameMusicTrack.ignoreListenerVolume = true;
    }

    private void OnSettingsEnabled(bool cameFromMainMenu)
    {
        this.soundSlider.value = PlayerPrefs.GetFloat("Music", 0.250f);
        this.sfxSlider.value = PlayerPrefs.GetFloat("SFX", 0.65f);

        this.cameFromMainMenu = cameFromMainMenu;
    }

    public void OnSoundChanged()
    {
        this.gameMusicTrack.volume = this.soundSlider.value;
    }

    public void OnSFXChanged()
    {
        AudioListener.volume = this.sfxSlider.value;
        AudioSource.PlayClipAtPoint(this.testSFX, Camera.main.transform.position);
    }

    public void OnOkClick()
    {
        PlayerPrefs.SetFloat("Music", this.soundSlider.value);
        PlayerPrefs.SetFloat("SFX", this.sfxSlider.value);

        if (this.cameFromMainMenu)
            this.mainMenu.SetActive(true);
        else
            this.pauseMenu.SetActive(true);

        this.settingsMenu.SetActive(false);
    }
}
