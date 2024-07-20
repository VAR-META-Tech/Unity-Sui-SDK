using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTStartup : MonoBehaviour
{
    private NFTActions nftActions;
    void Start()
    {
        nftActions = FindObjectOfType<NFTActions>();
        nftActions.Get_wallet_objects();
    }
}
