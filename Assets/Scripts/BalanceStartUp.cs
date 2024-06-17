using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using static BalanceManager;

public class BalanceStartUp : MonoBehaviour
{
    // Reference to the list item prefab
    public GameObject listItemPrefab;

    // Reference to the content object of the Scroll View (optional)
    public Transform content;

    // Sample data
    private BalanceData[] balances;

    private BalanceManager balanceManager;
    public TMP_Text addressTMP;
    void Start()
    {
        balanceManager = FindObjectOfType<BalanceManager>();
        LoadBalances();
    }

    public void LoadBalances()
    {
        Debug.Log("Load Balance of " + addressTMP.text);
        balances = balanceManager.LoadWallets(addressTMP.text);
        PopulateList();
    }


    void PopulateList()
    {
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
