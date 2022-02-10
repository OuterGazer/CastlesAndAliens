using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnoymentSliderBehaviour : MonoBehaviour
{
    [SerializeField] Slider queenSlider;
    [SerializeField] Slider kingSlider;
    [SerializeField] GameObject youLostPopUp;
    [SerializeField] GameObject annoymentMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject towerMenu;

    private AudioSource audioSource;
    [SerializeField] AudioClip youLostSFX;
    [SerializeField] AudioClip doorKnockSFX;
    [SerializeField] AudioClip alienAskingSFX;
    [SerializeField] AudioClip queenAnnoyedSFX;
    [SerializeField] AudioClip kingAnnoyedSFX;

    [SerializeField] int queenMaxAnnoymentLevel;
    [SerializeField] int kingMaxAnnoymentLevel;


    private void Awake()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        Messenger<string>.AddListener("AnnoyRoyalty", OnRoyaltyAnnoyed);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.queenSlider.maxValue = this.queenMaxAnnoymentLevel;
        this.kingSlider.maxValue = this.kingMaxAnnoymentLevel;

        this.queenSlider.value = 0;
        this.kingSlider.value = 0;
    }

    public void OnRoyaltyAnnoyed(string enemyName)
    {
        if (enemyName.Contains("Red"))
        {
            this.queenSlider.value++;
            StartCoroutine(AnnoyQueen());
        }
        else
        {
            this.kingSlider.value++;
            StartCoroutine(AnnoyKing());
        }

        if((this.queenSlider.value >= this.queenMaxAnnoymentLevel) ||
           (this.kingSlider.value >= this.kingMaxAnnoymentLevel))
        {
            AudioSource.PlayClipAtPoint(this.youLostSFX, Camera.main.transform.position);

            Time.timeScale = 0;

            this.youLostPopUp.SetActive(true);
            
            this.pauseMenu.SetActive(false);
            this.towerMenu.SetActive(false);
        }
    }

    private IEnumerator AnnoyQueen()
    {
        this.audioSource.PlayOneShot(this.doorKnockSFX);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        this.audioSource.PlayOneShot(this.alienAskingSFX, AudioListener.volume * 3.0f);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        this.audioSource.PlayOneShot(this.queenAnnoyedSFX);
    }

    private IEnumerator AnnoyKing()
    {
        this.audioSource.PlayOneShot(this.doorKnockSFX);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        this.audioSource.PlayOneShot(this.alienAskingSFX, AudioListener.volume * 3.0f);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        this.audioSource.PlayOneShot(this.kingAnnoyedSFX);
    }

    private void OnDestroy()
    {
        Messenger<string>.RemoveListener("AnnoyRoyalty", OnRoyaltyAnnoyed);
    }
}
