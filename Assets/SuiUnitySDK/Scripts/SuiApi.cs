using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SuiApi : MonoBehaviour
{
    const string LIB_NAME = SuiConst.MACOS_LIB_NAME;

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

    [DllImport(LIB_NAME)]
    private static extern string request_tokens_from_faucet(System.IntPtr address);

    [DllImport(LIB_NAME)]
    private static extern string programmable_transaction(System.IntPtr sender_address, System.IntPtr recipient_address, ulong amount);

    [DllImport(LIB_NAME)]
    private static extern string programmable_transaction_allow_sponser(System.IntPtr sender_address, System.IntPtr recipient_address, ulong amount, System.IntPtr sponser_address);

    private static BalanceArray balanceArray;


    public static void RequestTokensFromFaucet(string address)
    {
        System.IntPtr _address = Marshal.StringToHGlobalAnsi(address);
        try
        {
            Debug.Log(request_tokens_from_faucet(_address));
        }
        finally
        {
            // Free the unmanaged string memory
            Marshal.FreeHGlobal(_address);
        }
    }

    public static BalanceData[] LoadWallets(string address)
    {
        Debug.Log($"Load Balance Of {address}");
        free_balance_array(balanceArray);
        balanceArray = get_balances(address);
        return GetBalances();
    }

    public static BalanceData[] GetBalances()
    {
        int length = (int)balanceArray.length;

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
