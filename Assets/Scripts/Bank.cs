using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = default;
    private int currentBalance;
    public int CurrentBalance => this.currentBalance;
    [SerializeField] TextMeshProUGUI balanceText;


    private void Awake()
    {
        this.currentBalance = this.startingBalance;
        this.balanceText.text = $"Gold: {this.currentBalance}";
    }

    public void Deposit(int amount)
    {
        this.currentBalance += Mathf.Abs(amount);
        this.balanceText.text = $"Gold: {this.currentBalance}";
    }

    public void Withdraw(int amount)
    {
        this.currentBalance -= Mathf.Abs(amount);
        this.balanceText.text = $"Gold: {this.currentBalance}";

        if (this.currentBalance < 0)
        {
            this.balanceText.text = "Gold: 0";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
