using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtons : MonoBehaviour
{
    [SerializeField] Image sourceImage;
    private Color sourceColor;
    private Image image;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.image = this.gameObject.GetComponent<Image>();                

        if (this.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
            this.sprite = sprite;
        
        this.sourceColor = this.sourceImage.color;

        if (this.sprite == null)
            this.image.color = this.sourceColor;
        else
            this.sprite.color = this.sourceColor;

    }

    public void UpdateButtonColor()
    {
        this.sourceColor = this.sourceImage.color;

        if (this.sprite == null)
            this.image.color = this.sourceColor;
        else
            this.sprite.color = this.sourceColor;
    }
}
