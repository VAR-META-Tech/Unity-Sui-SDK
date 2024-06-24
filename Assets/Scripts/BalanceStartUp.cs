using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using static BalanceManager;

public class BalanceStartUp : MonoBehaviour
{
    private BalanceActions balanceActions;
    void Start()
    {
        balanceActions = FindObjectOfType<BalanceActions>();
        balanceActions.LoadBalances();
    }

}
