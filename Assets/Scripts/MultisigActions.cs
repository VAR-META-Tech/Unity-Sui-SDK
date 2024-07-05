using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static BalanceLib;
using static MultisigLib;
using static WalletLib;

public class MultisigActions : MonoBehaviour
{
    private WalletLib walletLib;
    private BalanceLib balanceLib;
    private MultisigLib multisigLib;
    private BalanceData[] balances;
    public TMP_Dropdown addressDropdown;
    public TMP_Dropdown addressDropdown2;
    public Transform multisigScrollViewContent;

    public TMP_InputField threadHoldInputField;

    public TMP_Text multisigAddress;
    public TMP_Text multisigBytes;
    public TMP_Text multisigBalance;

    private WalletData[] wallets;
    // Start is called before the first frame update
    void Awake()
    {
        walletLib = FindObjectOfType<WalletLib>();
        balanceLib = FindObjectOfType<BalanceLib>();
        multisigLib = FindObjectOfType<MultisigLib>();
        addressDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(addressDropdown);
        });

        addressDropdown2.onValueChanged.AddListener(delegate
       {
           DropdownValueChanged(addressDropdown2);
       });
        if (walletLib == null || balanceLib)
        {
            Debug.LogError("WalletLib or BalanceLib component not found in the scene. Ensure a GameObject has this component attached.");
            return; // Exit Start() if WalletLib is not found
        }
        if (multisigLib == null)
        {
            Debug.LogError("WalletLib component not found in the scene. Ensure a GameObject has this component attached.");
            return; // Exit Start() if WalletLib is not found
        }
    }
    public void RequestTokensFromFaucet()
    {
        if (!string.IsNullOrEmpty(multisigAddress.text))
            balanceLib.RequestTokensFromFaucet(multisigAddress.text);
    }
    public void LoadSuiBalance()
    {
        Debug.Log($"Load Balance Of: {multisigAddress.text}");

        multisigBalance.text = string.Empty;
        if (!string.IsNullOrEmpty(multisigAddress.text))
        {
            balances = balanceLib.LoadWallets(multisigAddress.text);
            Debug.Log($"Balance: {balances}");
            foreach (BalanceData balance in balances)
            {
                multisigBalance.text = balance.TotalBalance[0].ToString();
            }
        }
    }
    public void GetOrCreateMultisig()
    {
        string[] addresses = GetAddressValuesFromScrollView(multisigScrollViewContent);
        byte[] weights = GetWeightValuesFromScrollView(multisigScrollViewContent);
        ushort threshold = GetThreadHold();
        MultiSigData multiSigData = multisigLib.Get_or_create_multisig(addresses, weights, threshold);

        if (string.IsNullOrEmpty(multiSigData.Error))
        {
            Debug.Log($"MultiSigData: {multiSigData}");
            Debug.Log($"Bytes as Hex String: {multiSigData.BytesToHexString()}");

            // Example usage of HexStringToBytes
            string hexString = multiSigData.BytesToHexString();
            byte[] bytesFromHex = MultiSigData.HexStringToBytes(hexString);
            Debug.Log($"Bytes from Hex String: {BitConverter.ToString(bytesFromHex)}");
            multisigAddress.text = multiSigData.Address;
            multisigBytes.text = hexString;
            LoadSuiBalance();
        }
        else
        {
            Debug.LogError($"Error: {multiSigData.Error}");
        }
    }

    string[] GetAddressValuesFromScrollView(Transform content)
    {
        // Get the content Transform of the ScrollView

        // Create a list to store the text values
        List<string> textValues = new List<string>();

        // Loop through each child of the content Transform
        foreach (Transform child in content)
        {
            // Get the TMP_Text component of the child, if it exists
            TMP_Text tmpText = child.GetComponentInChildren<TMP_Text>();

            // If the TMP_Text component is found, add its text to the list
            if (tmpText != null)
            {
                textValues.Add(tmpText.text);
                Debug.Log("add " + tmpText.text);
            }
        }

        // Convert the list to an array and return it
        return textValues.ToArray();
    }
    ushort GetThreadHold()
    {
        if (ushort.TryParse(threadHoldInputField.text, out ushort result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning("Invalid byte value entered. Please enter a value between 0 and 255.");
            threadHoldInputField.text = string.Empty; // Clear the input field if the value is invalid
        }
        return 0;
    }
    byte[] GetWeightValuesFromScrollView(Transform content)
    {
        // Get the content Transform of the ScrollView

        // Create a list to store the text values
        List<byte> weights = new List<byte>();

        // Loop through each child of the content Transform
        foreach (Transform child in content)
        {
            // Get the TMP_Text component of the child, if it exists
            TMP_InputField tmpInput = child.GetComponentInChildren<TMP_InputField>();

            // If the TMP_Text component is found, add its text to the list
            if (tmpInput != null)
            {
                if (byte.TryParse(tmpInput.text, out byte result))
                {
                    weights.Add(result);
                    Debug.Log("add weight" + result);
                }
                else
                {
                    weights.Add(0);
                    Debug.LogWarning("Invalid byte value entered. Please enter a value between 0 and 255.");
                    tmpInput.text = string.Empty; // Clear the input field if the value is invalid
                }
            }
        }

        // Convert the list to an array and return it
        return weights.ToArray();
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Output the selected option
        Debug.Log("Selected: " + change.options[change.value].text);
    }

    public void SetDropdownValue(TMP_Dropdown dropdown, int index)
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
    public void SetDropdownValue(TMP_Dropdown dropdown, string option)
    {
        int index = dropdown.options.FindIndex(opt => opt.text == option);
        if (index != -1)
        {
            SetDropdownValue(dropdown, index);
        }
        else
        {
            Debug.LogWarning("Dropdown option not found: " + option);
        }
    }
    public void LoadWallets()
    {
        wallets = walletLib.LoadWallets();
        UpdateWallets(addressDropdown);
        UpdateWallets(addressDropdown2);
    }

    public void UpdateWallets(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.options.Add(new TMP_Dropdown.OptionData("Choose the wallet that you want to add or remove"));

        foreach (var wallet in wallets)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(wallet.Address));
        }
        SetDropdownValue(dropdown, -1);
    }

}
