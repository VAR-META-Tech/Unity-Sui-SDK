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

    private SuiApi balanceManager;
    public TMP_Dropdown dropdown;
    public TMP_InputField tmp_amount;
    public TMP_InputField tmp_recepient_address;
    public TMP_InputField tmp_sponser_address;

    void Awake()
    {
        balanceManager = FindObjectOfType<SuiApi>();
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
        SuiTransactionBuilder transactionBuilder = new SuiTransactionBuilder();
        SuiAgruments coin = transactionBuilder.CreateArguments();
        transactionBuilder.AddArgumentGasCoin(coin);
        SuiAgruments amountAg = transactionBuilder.CreateArguments();
        SuiPure amountData = SuiBCS.BscBasic(SuiBCS.SuiType.U64,tmp_amount.text);
        transactionBuilder.MakePure(amountAg,amountData);
        transactionBuilder.AddSplitCoinsCommand(coin, amountAg);

        SuiAgruments agruments = transactionBuilder.CreateArguments();
        transactionBuilder.AddArgumentResult(agruments, 0);
        SuiAgruments recipient = transactionBuilder.CreateArguments();
        SuiPure recipientData = SuiBCS.BscBasic(SuiBCS.SuiType.Address,tmp_recepient_address.text);
        transactionBuilder.MakePure(recipient,recipientData);
        transactionBuilder.AddTransferObjectCommand(agruments, recipient);

        String reponse = transactionBuilder.ExecuteTransaction(dropdown.options[dropdown.value].text, 5000000);
        Debug.Log(reponse);
    }

    public void ProgrammableTransactionAllowSponser()
    {        
        SuiTransactionBuilder transactionBuilder = new SuiTransactionBuilder();
        SuiAgruments coin = transactionBuilder.CreateArguments();
        transactionBuilder.AddArgumentGasCoin(coin);
        SuiAgruments amountAg = transactionBuilder.CreateArguments();
        SuiPure amountData = SuiBCS.BscBasic(SuiBCS.SuiType.U64,tmp_amount.text);
        transactionBuilder.MakePure(amountAg,amountData);
        transactionBuilder.AddSplitCoinsCommand(coin, amountAg);

        SuiAgruments agruments = transactionBuilder.CreateArguments();
        transactionBuilder.AddArgumentResult(agruments, 0);
        SuiAgruments recipient = transactionBuilder.CreateArguments();
        SuiPure recipientData = SuiBCS.BscBasic(SuiBCS.SuiType.Address,tmp_recepient_address.text);
        transactionBuilder.MakePure(recipient,recipientData);
        transactionBuilder.AddTransferObjectCommand(agruments, recipient);

        String reponse = transactionBuilder.ExecuteTransactionAllowSponser(dropdown.options[dropdown.value].text, 5000000,tmp_sponser_address.text);
        Debug.Log(reponse);
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
