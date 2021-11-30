using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuInteraction : MonoBehaviour
{
    [SerializeField] Image[] towerButtons;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Image item in this.towerButtons)
        {
            item.color = Color.grey;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(Image towerType)
    {
        foreach (Image item in this.towerButtons)
        {
            item.color = Color.grey;
        }

        towerType.color = Color.white;
    }
}
