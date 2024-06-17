using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WalletManager;

public class WalletsStartUp : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private WalletData[] wallets;

    private WalletManager walletManager;
    void Start()
    {
        walletManager = FindObjectOfType<WalletManager>();
        GetWallets();
        PopulateList();
    }

    public void GenerateAndAddWallet()
    {
        walletManager.GenerateWallet();
    }

    public void GetWallets()
    {
        wallets = walletManager.GetWallets();

    }

    void PopulateList()
    {

        foreach (var wallet in wallets)
        {
            // Create a new list item from the prefab
            GameObject newItem = Instantiate(listItemPrefab, content);

            // Find and set the text components of the list item
            TextMeshProUGUI[] textComponents = newItem.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textComponent in textComponents)
            {
                switch (textComponent.name)
                {
                    case "Address":
                        textComponent.text = wallet.Address;
                        break;
                    case "MnemonicText":
                        textComponent.text = wallet.Mnemonic;
                        break;
                    case "PublicKey":
                        textComponent.text = wallet.PublicBase64Key;
                        break;
                    case "PrivateKey":
                        textComponent.text = wallet.PrivateKey;
                        break;
                    case "KeySchemeText":
                        textComponent.text = wallet.KeyScheme;
                        break;
                }
            }
        }
    }
}
