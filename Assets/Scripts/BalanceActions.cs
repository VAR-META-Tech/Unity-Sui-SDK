using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BalanceManager;

public class BalanceActions : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private BalanceData[] balances;

    private BalanceManager balanceManager;
    public TMP_Dropdown dropdown;
    public TMP_InputField tmp_amount;
    public TMP_InputField tmp_recepient_address;

    void Awake()
    {
        balanceManager = FindObjectOfType<BalanceManager>();
        dropdown.onValueChanged.AddListener(delegate
     {
         DropdownValueChanged(dropdown);
     });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Output the selected option
        Debug.Log("Selected:2 " + change.options[change.value].text);
        LoadBalances();
    }

    public void LoadBalances()
    {
        Debug.Log("Start Loading Balance ");
        if (balanceManager == null) return;
        Debug.Log("Load Balance of " + dropdown.options[dropdown.value].text);
        Debug.Log("Balance manager " + balanceManager.ToString());
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
            Debug.Log("CoinType: " + balance.CoinType);
            Debug.Log("CoinObjectCount: " + balance.CoinObjectCount);
            Debug.Log("TotalBalance: " + balance.TotalBalance[0]);
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
