using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnoymentSliderBehaviour : MonoBehaviour
{
    [SerializeField] Slider queenSlider;
    [SerializeField] Slider kingSlider;

    [SerializeField] int queenMaxAnnoymentLevel;
    [SerializeField] int kingMaxAnnoymentLevel;


    private void Awake()
    {
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
        }
        else
        {
            this.kingSlider.value++;
        }

        if((this.queenSlider.value >= this.queenMaxAnnoymentLevel) ||
           (this.kingSlider.value >= this.kingMaxAnnoymentLevel))
        {
            // TODO: implement losing logic here
        }
    }

    private void OnDestroy()
    {
        Messenger<string>.RemoveListener("AnnoyRoyalty", OnRoyaltyAnnoyed);
    }
}
