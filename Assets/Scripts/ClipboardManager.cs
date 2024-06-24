using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClipboardManager : MonoBehaviour
{
    public TMP_Text text;

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = text.text;
    }
}
