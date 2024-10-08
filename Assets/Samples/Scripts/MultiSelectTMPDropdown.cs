using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiSelectTMPDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Transform content;
    public GameObject selectedItemPrefab;

    private List<int> selectedIndices = new List<int>();
    public string defaultText = "Select wallet";
    void Start()
    {
        dropdown.captionText.text = defaultText;
        dropdown.value = 0;
        // Add a listener to handle selection changes
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        // Ignore the default option
        if (index == 0)
        {
            return;
        }
        // Toggle selection
        if (selectedIndices.Contains(index))
        {
            selectedIndices.Remove(index);
        }
        else
        {
            selectedIndices.Add(index);
        }
        dropdown.value = 0;
        dropdown.captionText.text = defaultText;
        // Update the scroll view display to show the selected items
        UpdateSelectedItemsDisplay();
    }

    void UpdateSelectedItemsDisplay()
    {
        // // Clear current selected items display
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Create and display selected items
        foreach (int index in selectedIndices)
        {
            GameObject selectedItem = Instantiate(selectedItemPrefab, content);
            TMP_Text selectedItemText = selectedItem.GetComponentInChildren<TMP_Text>();
            Debug.Log("set" + dropdown.options[index].text);
            selectedItemText.text = dropdown.options[index].text;
        }
    }
}
