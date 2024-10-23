using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using static SuiApi;

public class BalanceActions : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private BalanceData[] balances;

    public TMP_Dropdown dropdown;
    public TMP_InputField tmp_amount;
    public TMP_InputField tmp_recepient_address;
    public TMP_InputField tmp_sponser_address;

    void Awake()
    {
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
        balances = SuiApi.LoadWallets(dropdown.options[dropdown.value].text);
        PopulateList();
    }

    public void ProgrammableTransaction()
    {
        SuiTransactionBuilder.CreateBuilder();
        SuiAgruments coin = SuiTransactionBuilder.CreateArguments();
        SuiTransactionBuilder.AddArgumentGasCoin(coin);
        SuiAgruments amountAg = SuiTransactionBuilder.CreateArguments();
        SuiPure amountData = SuiBCS.BscBasic(SuiBCS.SuiType.U64, tmp_amount.text);
        SuiTransactionBuilder.MakePure(amountAg, amountData);
        SuiTransactionBuilder.AddSplitCoinsCommand(coin, amountAg);

        SuiAgruments agruments = SuiTransactionBuilder.CreateArguments();
        SuiTransactionBuilder.AddArgumentResult(agruments, 0);
        SuiAgruments recipient = SuiTransactionBuilder.CreateArguments();
        SuiPure recipientData = SuiBCS.BscBasic(SuiBCS.SuiType.Address, tmp_recepient_address.text);
        SuiTransactionBuilder.MakePure(recipient, recipientData);
        SuiTransactionBuilder.AddTransferObjectCommand(agruments, recipient);

        String reponse = SuiTransactionBuilder.ExecuteTransaction(dropdown.options[dropdown.value].text, 5000000);
        Debug.Log(reponse);
    }

    public void ProgrammableTransactionAllowSponser()
    {
        SuiTransactionBuilder.CreateBuilder();
        SuiAgruments coin = SuiTransactionBuilder.CreateArguments();
        SuiTransactionBuilder.AddArgumentGasCoin(coin);
        SuiAgruments amountAg = SuiTransactionBuilder.CreateArguments();
        SuiPure amountData = SuiBCS.BscBasic(SuiBCS.SuiType.U64, tmp_amount.text);
        SuiTransactionBuilder.MakePure(amountAg, amountData);
        SuiTransactionBuilder.AddSplitCoinsCommand(coin, amountAg);

        SuiAgruments agruments = SuiTransactionBuilder.CreateArguments();
        SuiTransactionBuilder.AddArgumentResult(agruments, 0);
        SuiAgruments recipient = SuiTransactionBuilder.CreateArguments();
        SuiPure recipientData = SuiBCS.BscBasic(SuiBCS.SuiType.Address, tmp_recepient_address.text);
        SuiTransactionBuilder.MakePure(recipient, recipientData);
        SuiTransactionBuilder.AddTransferObjectCommand(agruments, recipient);

        String reponse = SuiTransactionBuilder.ExecuteTransactionAllowSponser(dropdown.options[dropdown.value].text, 5000000, tmp_sponser_address.text);
        Debug.Log(reponse);
    }

    public void RequestTokensFromFaucet()
    {
        SuiApi.RequestTokensFromFaucet(dropdown.options[dropdown.value].text);
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
