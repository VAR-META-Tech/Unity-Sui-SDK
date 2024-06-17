using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WalletManager;

public class WalletActions : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private WalletData[] wallets;

    private WalletManager walletManager;

    public TMP_InputField inputField;

    public string GetText()
    {
        if (inputField != null)
        {
            return inputField.text;
        }
        return string.Empty;
    }

    void Start()
    {
        walletManager = FindObjectOfType<WalletManager>();
    }

    public void GetWallets()
    {
        wallets = walletManager.GetWallets();
    }
    public void LoadWallets()
    {
        wallets = walletManager.GetWallets();
        PopulateList();
    }
    public void GenerateAndAddNew()
    {

        walletManager.GenerateAndAddNew();
        wallets = walletManager.LoadWallets();
        PopulateList();
    }

    public void ImportFromPrivateKey()
    {
        walletManager.ImportFromPrivateKey(GetText());
        wallets = walletManager.LoadWallets();
        PopulateList();
    }

    public void ImportFromMnemonic()
    {
        walletManager.ImportFromMnemonic(GetText());
        wallets = walletManager.LoadWallets();
        PopulateList();
    }
    void PopulateList()
    {
        // Clear old list items
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

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
