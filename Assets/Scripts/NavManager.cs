using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavManager : MonoBehaviour
{

    public GameObject walletScreen;
    public GameObject balanceScreen;

    public void ShowWalletScreen()
    {
        walletScreen.SetActive(true);
        balanceScreen.SetActive(false);
    }

    public void ShowBalanceScreen()
    {
        walletScreen.SetActive(false);
        balanceScreen.SetActive(true);
    }
}
