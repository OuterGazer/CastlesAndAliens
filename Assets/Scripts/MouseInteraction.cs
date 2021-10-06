using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] GameObject basicTowerPrefab;

    [SerializeField] bool isPlaceable = default;
    public bool IsPlaceable => this.isPlaceable;

    private Bank bank;

    private void Start()
    {
        this.bank = GameObject.FindObjectOfType<Bank>();
    }

    private void OnMouseDown()
    {
        if (this.isPlaceable && this.bank.CurrentBalance > 0)
        {
            //Debug.Log(this.gameObject.transform.position);
            Vector3 placementPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.2f, this.gameObject.transform.position.z);
            GameObject tower = Instantiate<GameObject>(this.basicTowerPrefab, placementPos, Quaternion.identity);
            this.isPlaceable = false;

            this.bank.Withdraw(tower.GetComponent<DefenseTower>().GoldCost);
        }        
    }
}
