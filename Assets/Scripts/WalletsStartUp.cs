using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WalletLib;

public class WalletsStartUp : MonoBehaviour
{
    private WalletActions walletActions;
    void Start()
    {
        walletActions = FindObjectOfType<WalletActions>();
        walletActions.LoadWallets();
    }

}
