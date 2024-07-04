using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static MultisigLib;
using static WalletLib;

public class MultisigActions : MonoBehaviour
{
    private WalletLib walletLib;
    private MultisigLib multisigLib;
    public TMP_Dropdown addressDropdown;
    public Transform multisigScrollViewContent;

    private WalletData[] wallets;
    // Start is called before the first frame update
    void Awake()
    {
        walletLib = FindObjectOfType<WalletLib>();
        if (walletLib == null)
        {
            Debug.LogError("WalletLib component not found in the scene. Ensure a GameObject has this component attached.");
            return; // Exit Start() if WalletLib is not found
        }
        multisigLib = FindObjectOfType<MultisigLib>();
        if (multisigLib == null)
        {
            Debug.LogError("WalletLib component not found in the scene. Ensure a GameObject has this component attached.");
            return; // Exit Start() if WalletLib is not found
        }
        addressDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(addressDropdown);
        });
    }
    public void GetOrCreateMultisig()
    {
        string[] addresses = GetTextValuesFromScrollView(multisigScrollViewContent);
        byte[] weights = { 1, 1, 1 };
        ushort threshold = 2;

        MultiSigData multiSigData = multisigLib.Get_or_create_multisig(addresses, weights, threshold);

        if (string.IsNullOrEmpty(multiSigData.Error))
        {
            Debug.Log($"MultiSigData: {multiSigData}");
            Debug.Log($"Bytes as Hex String: {multiSigData.BytesToHexString()}");

            // Example usage of HexStringToBytes
            string hexString = multiSigData.BytesToHexString();
            byte[] bytesFromHex = MultiSigData.HexStringToBytes(hexString);
            Debug.Log($"Bytes from Hex String: {BitConverter.ToString(bytesFromHex)}");
        }
        else
        {
            Debug.LogError($"Error: {multiSigData.Error}");
        }
    }

    string[] GetTextValuesFromScrollView(Transform content)
    {
        // Get the content Transform of the ScrollView

        // Create a list to store the text values
        List<string> textValues = new List<string>();

        // Loop through each child of the content Transform
        foreach (Transform child in content)
        {
            // Get the TMP_Text component of the child, if it exists
            TMP_Text tmpText = child.GetComponent<TMP_Text>();

            // If the TMP_Text component is found, add its text to the list
            if (tmpText != null)
            {
                textValues.Add(tmpText.text);
                Debug.Log("addd" + tmpText.text);
            }
        }

        // Convert the list to an array and return it
        return textValues.ToArray();
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Output the selected option
        Debug.Log("Selected: " + change.options[change.value].text);
    }

    public void SetDropdownValue(int index)
    {
        if (index >= 0 && index < addressDropdown.options.Count)
        {
            addressDropdown.value = index;
            addressDropdown.RefreshShownValue();
            DropdownValueChanged(addressDropdown); // Call the method to handle any logic needed for the new value
        }
        else
        {
            Debug.LogWarning("Dropdown value index is out of range.");
        }
    }
    public void SetDropdownValue(string option)
    {
        int index = addressDropdown.options.FindIndex(opt => opt.text == option);
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
        addressDropdown.ClearOptions();
        addressDropdown.options.Add(new TMP_Dropdown.OptionData("Choose the wallet that you want to add or remove"));

        foreach (var wallet in wallets)
        {
            addressDropdown.options.Add(new TMP_Dropdown.OptionData(wallet.Address));
        }
        SetDropdownValue(-1);
    }

}
