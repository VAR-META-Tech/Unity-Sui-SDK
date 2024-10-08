using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavManager : MonoBehaviour
{
    private GameObject walletScreen;
    private GameObject balanceScreen;
    private GameObject nftScreen;
    private GameObject multisigScreen;

    void Start()
    {
        // Find the GameObjects by name
        walletScreen = GameObject.FindWithTag("WalletScreenTag");
        balanceScreen = GameObject.FindWithTag("BalanceScreenTag");
        nftScreen = GameObject.FindWithTag("NftScreenTag");
        multisigScreen = GameObject.FindWithTag("MultisigScreenTag");
        if (walletScreen == null || balanceScreen == null || multisigScreen == null || nftScreen == null)
        {
            Debug.LogError("One or more screen GameObjects not found. Please ensure they are named correctly.");
        }
        else
        {
            SetActiveScreen(walletScreen);
        }
    }

    public void ShowWalletScreen()
    {
        if (walletScreen != null && balanceScreen != null && multisigScreen != null)
        {
            walletScreen.SetActive(true);
            balanceScreen.SetActive(false);
            multisigScreen.SetActive(false);
            Debug.Log("Wallet screen is now active.");
        }
        else
        {
            Debug.LogError("One or more screen GameObjects are not assigned.");
        }
    }

    public void ShowBalanceScreen()
    {
        if (walletScreen != null && balanceScreen != null && multisigScreen != null)
        {
            walletScreen.SetActive(false);
            balanceScreen.SetActive(true);
            multisigScreen.SetActive(false);
            nftScreen.SetActive(false);
            Debug.Log("Balance screen is now active.");
        }
        else
        {
            Debug.LogError("One or more screen GameObjects are not assigned.");
        }
    }

    public void ShowNftScreen()
    {
        if (walletScreen != null && balanceScreen != null && multisigScreen != null)
        {
            walletScreen.SetActive(false);
            balanceScreen.SetActive(false);
            nftScreen.SetActive(true);
            multisigScreen.SetActive(false);
            Debug.Log("NFTs screen is now active.");
        }
        else
        {
            Debug.LogError("One or more screen GameObjects are not assigned.");
        }
    }

    public void ShowMultisigScreen()
    {
        if (walletScreen != null && balanceScreen != null && multisigScreen != null)
        {
            walletScreen.SetActive(false);
            balanceScreen.SetActive(false);
            nftScreen.SetActive(false);
            multisigScreen.SetActive(true);
            Debug.Log("Multisig screen is now active.");
        }
        else
        {
            Debug.LogError("One or more screen GameObjects are not assigned.");
        }
    }

    private void SetActiveScreen(GameObject activeScreen)
    {
        walletScreen.SetActive(false);
        balanceScreen.SetActive(false);
        multisigScreen.SetActive(false);
        nftScreen.SetActive(false);
        activeScreen.SetActive(true);
    }
}
