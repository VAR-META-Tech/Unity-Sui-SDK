using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BalanceLib;

public class BalanceActions : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private BalanceData[] balances;

    private BalanceLib balanceManager;
    public TMP_Dropdown dropdown;
    public TMP_InputField tmp_amount;
    public TMP_InputField tmp_recepient_address;
    public TMP_InputField tmp_sponser_address;

    void Awake()
    {
        balanceManager = FindObjectOfType<BalanceLib>();
        dropdown.onValueChanged.AddListener(delegate
     {
         DropdownValueChanged(dropdown);
     });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Output the selected option
        LoadBalances();
    }

    public void LoadBalances()
    {
        Debug.Log("Load balance of: " + dropdown.options[dropdown.value].text);
        if (balanceManager == null) return;
        balances = balanceManager.LoadWallets(dropdown.options[dropdown.value].text);
        PopulateList();
    }

    public void ProgrammableTransaction()
    {
        if (ulong.TryParse(tmp_amount.text, out ulong amount))
        {
            Debug.Log("Input converted to ulong: " + amount);
        }
        else
        {
            Debug.LogError("Invalid input, unable to convert to ulong.");
        }
        balanceManager.ProgrammableTransaction(dropdown.options[dropdown.value].text, tmp_recepient_address.text, amount);
    }

    public void ProgrammableTransactionAllowSponser()
    {
        if (ulong.TryParse(tmp_amount.text, out ulong amount))
        {
            Debug.Log("Input converted to ulong: " + amount);
        }
        else
        {
            Debug.LogError("Invalid input, unable to convert to ulong.");
        }
        balanceManager.ProgrammableTransactionAllowSponser(dropdown.options[dropdown.value].text, tmp_recepient_address.text, amount, tmp_sponser_address.text);
    }

    public void RequestTokensFromFaucet()
    {
        balanceManager.RequestTokensFromFaucet(dropdown.options[dropdown.value].text);
    }

    void PopulateList()
    {
        if (!content) return;
        // Clear old list items
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var balance in balances)
        {
            // Create a new list item from the prefab
            GameObject newItem = Instantiate(listItemPrefab, content);

            // Find and set the text components of the list item
            TextMeshProUGUI[] textComponents = newItem.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textComponent in textComponents)
            {
                switch (textComponent.name)
                {
                    case "CoinType":
                        textComponent.text = balance.CoinType;
                        break;
                    case "CoinObjectCount":
                        textComponent.text = balance.CoinObjectCount.ToString();
                        break;
                    case "TotalBalance":
                        textComponent.text = balance.TotalBalance[0].ToString();
                        break;

                }
            }
        }
    }
}
