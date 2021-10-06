using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = default;
    private int currentBalance;
    public int CurrentBalance => this.currentBalance;


    private void Awake()
    {
        this.currentBalance = this.startingBalance;
    }

    public void Deposit(int amount)
    {
        this.currentBalance += Mathf.Abs(amount);
    }

    public void Withdraw(int amount)
    {
        this.currentBalance -= Mathf.Abs(amount);
    }
}
