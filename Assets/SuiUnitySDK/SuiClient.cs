using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class SuiClient : MonoBehaviour
{
    const string LIB_NAME = SuiConst.MACOS_LIB_NAME;

    [DllImport(LIB_NAME)]
    private static extern int build_testnet();

    [DllImport(LIB_NAME)]
    private static extern int build_devnet();

    [DllImport(LIB_NAME)]
    private static extern int build_mainnet();

    private static SuiClient _instance;

    public static SuiClient Instance
    {
        get
        {
            // If the instance is null, find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<SuiClient>();

                // If still null, create a new GameObject and attach this script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SuiClient).ToString());
                    _instance = singletonObject.AddComponent<SuiClient>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
    }

    public static bool BuildTestnet()
    {
        return build_testnet() == 0;
    }

    public static bool BuildDevnet()
    {
        return build_devnet() == 0;
    }

    public static bool BuildMainnet()
    {
        return build_mainnet() == 0;
    }
}