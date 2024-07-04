using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static WalletLib;

public class WalletActions : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;
    public GameObject walletDetail;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private WalletData[] wallets;

    private WalletLib walletLib;

    public TMP_InputField inputField;

    public TMP_Dropdown dropdown;
    public TMP_Dropdown schemeDropdown;
    public TMP_Dropdown wordLengthDropdown;
    public TMP_Text tmp_address;
    public TMP_Text tmp_public_key;
    public TMP_Text tmp_private_key;
    public TMP_Text tmp_scheme;
    public TMP_Text tmp_mnemonic;

    public string GetText()
    {
        if (inputField != null)
        {
            return inputField.text;
        }
        return string.Empty;
    }

    void Awake()
    {
        walletLib = FindObjectOfType<WalletLib>();
        if (walletLib == null)
        {
            Debug.LogError("WalletLib component not found in the scene. Ensure a GameObject has this component attached.");
            return; // Exit Start() if WalletLib is not found
        }
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
    }
    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Output the selected option
        Debug.Log("Selected: " + change.options[change.value].text);
    }
    void SetDefaultValue()
    {
        // Set the default value
        SetDropdownValue(0);
    }

    public void SetDropdownValue(int index)
    {
        if (index >= 0 && index < dropdown.options.Count)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
            DropdownValueChanged(dropdown); // Call the method to handle any logic needed for the new value
        }
        else
        {
            Debug.LogWarning("Dropdown value index is out of range.");
        }
    }
    public void SetDropdownValue(string option)
    {
        int index = dropdown.options.FindIndex(opt => opt.text == option);
        if (index != -1)
        {
            SetDropdownValue(index);
        }
        else
        {
            Debug.LogWarning("Dropdown option not found: " + option);
        }
    }
    public void LoadWallets()
    {
        wallets = walletLib.LoadWallets();
        PopulateList();
    }
    public void GenerateAndAddNew()
    {
        walletLib.GenerateAndAddNew();
        wallets = walletLib.LoadWallets();
        PopulateList();
    }
    public void CreateWallet()
    {
        WalletData wallet = walletLib.GenerateWallet(schemeDropdown.options[schemeDropdown.value].text, wordLengthDropdown.options[wordLengthDropdown.value].text);
        wallet.Show();
        tmp_scheme.text = wallet.KeyScheme;
        tmp_address.text = wallet.Address;
        tmp_public_key.text = wallet.PublicBase64Key;
        tmp_private_key.text = wallet.PrivateKey;
        tmp_mnemonic.text = wallet.Mnemonic;
    }


    public void ImportFromPrivateKey()
    {
        walletLib.ImportFromPrivateKey(GetText());
        wallets = walletLib.LoadWallets();
        PopulateList();
    }

    public void ImportFromMnemonic()
    {
        walletLib.ImportFromMnemonic(GetText());
        wallets = walletLib.LoadWallets();
        PopulateList();
    }
    public void PopulateList()
    {
        // Clear old list items
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        dropdown.ClearOptions();

        foreach (var wallet in wallets)
        {

            // Create a new list item from the prefab
            GameObject newItem = Instantiate(listItemPrefab, content);
            dropdown.options.Add(new TMP_Dropdown.OptionData(wallet.Address));
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
        SetDefaultValue();
    }
}
