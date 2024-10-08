using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultisigStartUp : MonoBehaviour
{
    private MultisigActions multisigActions;
    void Start()
    {
        multisigActions = FindObjectOfType<MultisigActions>();
        multisigActions.LoadWallets();
        // multisigActions.GetOrCreateMultisig();
    }
}
