using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetWallet : MonoBehaviour
{
    public TMP_Text sourceTMP;
    public TMP_Text destinationTMP;
    public void Use()
    {
        // Check if the references are assigned
        if (sourceTMP != null && destinationTMP != null)
        {
            // Get the text from the source TMP and set it to the destination TMP
            destinationTMP.text = sourceTMP.text;
        }
        else
        {
            Debug.LogError("Source or Destination TMP is not assigned.");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
