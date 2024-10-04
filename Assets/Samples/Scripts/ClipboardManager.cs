using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClipboardManager : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Dropdown dropdown;

    public void CopyToClipboard()
    {
        if (text != null)
        {
            GUIUtility.systemCopyBuffer = text.text;
        }
        else if (dropdown != null)
        {
            GUIUtility.systemCopyBuffer = dropdown.options[dropdown.value].text;
        }
    }
}
