using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    const string LIB_NAME = "libsui_rust_sdk";

    [StructLayout(LayoutKind.Sequential)]
    public struct Balance
    {
        public System.IntPtr coin_type;
        public System.UIntPtr coin_object_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ulong[] total_balance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BalanceArray
    {
        public System.IntPtr balances;
        public System.UIntPtr length;
    }

    [DllImport(LIB_NAME)]
    private static extern BalanceArray get_balances(string address);


    [DllImport(LIB_NAME)]
    private static extern void free_balance_array(BalanceArray balanceArray);


    private BalanceArray balanceArray;

    // The static instance of the singleton
    private static BalanceManager _instance;

    // Property to access the instance
    public static BalanceManager Instance
    {
        get
        {
            // If the instance is null, find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<BalanceManager>();

                // If still null, create a new GameObject and attach this script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(BalanceManager).ToString());
                    _instance = singletonObject.AddComponent<BalanceManager>();
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
        Debug.Log("Destroy balance Array");
        free_balance_array(balanceArray);
    }

    public BalanceData[] LoadWallets(string address)
    {
        Debug.Log("Load " + address);
        Debug.Log("balanceArray " + balanceArray);
        free_balance_array(balanceArray);
        balanceArray = get_balances(address);
        return GetBalances();
    }

    public BalanceData[] GetBalances()
    {
        int length = (int)balanceArray.length;
        Debug.Log("length " + length);

        BalanceData[] balances = new BalanceData[length];
        System.IntPtr currentPtr = balanceArray.balances;

        for (int i = 0; i < length; i++)
        {
            Balance balance = Marshal.PtrToStructure<Balance>(currentPtr);
            balances[i] = new BalanceData
            {
                CoinType = Marshal.PtrToStringAnsi(balance.coin_type),
                CoinObjectCount = (ulong)balance.coin_object_count,
                TotalBalance = balance.total_balance

            };


            currentPtr = System.IntPtr.Add(currentPtr, Marshal.SizeOf<Balance>());
        }
        Debug.Log("balances " + balances.ToString());

        return balances;
    }

    public struct BalanceData
    {
        public string CoinType;
        public ulong CoinObjectCount;
        public ulong[] TotalBalance;

        public BalanceData(string coinType, ulong coinObjectCount, ulong[] totalBalance)
        {
            CoinType = coinType;
            CoinObjectCount = coinObjectCount;
            TotalBalance = totalBalance;
        }
    }
}
