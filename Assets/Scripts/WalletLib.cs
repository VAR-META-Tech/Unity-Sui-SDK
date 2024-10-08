using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WalletLib : MonoBehaviour
{
    const string LIB_NAME = "libsui_rust_sdk";
    [StructLayout(LayoutKind.Sequential)]
    public struct Wallet
    {
        public System.IntPtr address;
        public System.IntPtr mnemonic;
        public System.IntPtr public_base64_key;
        public System.IntPtr private_key;
        public System.IntPtr key_scheme;
        public void Show()
        {
            Debug.Log($"Address: {PtrToStringAnsiSafe(address, "Address")}");
            Debug.Log($"Mnemonic: {PtrToStringAnsiSafe(mnemonic, "Mnemonic")}");
            Debug.Log($"Public Base64 Key: {PtrToStringAnsiSafe(public_base64_key, "Public Base64 Key")}");
            Debug.Log($"Private Key: {PtrToStringAnsiSafe(private_key, "Private Key")}");
            Debug.Log($"Key Scheme: {PtrToStringAnsiSafe(key_scheme, "Key Scheme")}");
        }

        // Helper method to safely convert IntPtr to string with debugging
        private static string PtrToStringAnsiSafe(IntPtr ptr, string fieldName)
        {
            if (ptr == IntPtr.Zero)
            {
                Debug.LogError($"{fieldName} pointer is zero");
                return string.Empty;
            }
            try
            {
                return Marshal.PtrToStringAnsi(ptr);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error converting {fieldName}: {ex.Message}");
                return string.Empty;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WalletList
    {
        public System.IntPtr wallets;
        public ulong length;
    }

    [DllImport(LIB_NAME)]
    private static extern WalletList get_wallets();

    [DllImport(LIB_NAME)]
    private static extern IntPtr generate_and_add_key();

    [DllImport(LIB_NAME)]
    private static extern void free_wallet_list(WalletList walletList);

    [DllImport(LIB_NAME)]
    private static extern void import_from_private_key(System.IntPtr privateKey);

    [DllImport(LIB_NAME)]
    private static extern System.IntPtr import_from_mnemonic(System.IntPtr mnemonic, System.IntPtr sig_scheme, System.IntPtr alias);

    [DllImport(LIB_NAME)]
    private static extern IntPtr generate_wallet(string key_scheme, string word_length);

    [DllImport(LIB_NAME)]
    public static extern void free_wallet(IntPtr wallet);

    private WalletList walletList;

    // The static instance of the singleton
    private static WalletLib _instance;

    // Property to access the instance
    public static WalletLib Instance
    {
        get
        {
            // If the instance is null, find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<WalletLib>();

                // If still null, create a new GameObject and attach this script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(WalletLib).ToString());
                    _instance = singletonObject.AddComponent<WalletLib>();
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
        walletList = get_wallets();

    }

    void OnDestroy()
    {
        Debug.Log("Destroy wallets");
        free_wallet_list(walletList);
    }

    public WalletData[] LoadWallets()
    {
        free_wallet_list(walletList);
        walletList = get_wallets();
        return GetWallets();
    }

    public void ImportFromPrivateKey(string privateKey)
    {
        System.IntPtr keyBase64Ptr = Marshal.StringToHGlobalAnsi(privateKey);
        try
        {
            import_from_private_key(keyBase64Ptr);
        }
        finally
        {
            // Free the unmanaged string memory
            Marshal.FreeHGlobal(keyBase64Ptr);
        }
    }

    public void ImportFromMnemonic(string mnemonic)
    {
        System.IntPtr mnemonicPtr = Marshal.StringToHGlobalAnsi(mnemonic);
        System.IntPtr sig_schemePTR = Marshal.StringToHGlobalAnsi("ED25519");
        System.IntPtr aliasPTR = Marshal.StringToHGlobalAnsi("");
        try
        {
            Debug.Log("ImportFromMnemonic" + mnemonicPtr);
            import_from_mnemonic(mnemonicPtr, sig_schemePTR, aliasPTR);
        }
        finally
        {
            // Free the unmanaged string memory
            Marshal.FreeHGlobal(mnemonicPtr);
        }
    }

    public WalletData[] GetWallets()
    {
        WalletData[] wallets = new WalletData[walletList.length];
        System.IntPtr currentPtr = walletList.wallets;

        for (ulong i = 0; i < walletList.length; i++)
        {
            Wallet wallet = Marshal.PtrToStructure<Wallet>(currentPtr);
            wallets[i] = new WalletData
            {
                Address = Marshal.PtrToStringAnsi(wallet.address),
                Mnemonic = Marshal.PtrToStringAnsi(wallet.mnemonic),
                PublicBase64Key = Marshal.PtrToStringAnsi(wallet.public_base64_key),
                PrivateKey = Marshal.PtrToStringAnsi(wallet.private_key),
                KeyScheme = Marshal.PtrToStringAnsi(wallet.key_scheme)
            };
            currentPtr = System.IntPtr.Add(currentPtr, Marshal.SizeOf<Wallet>());
        }

        return wallets;
    }

    public WalletData GenerateWallet(string key_scheme, string word_length)
    {
        try
        {
            IntPtr walletPtr = WalletLib.generate_wallet(key_scheme, word_length);
            Wallet wallet = Marshal.PtrToStructure<Wallet>(walletPtr);
            return new WalletData
            {
                Address = Marshal.PtrToStringAnsi(wallet.address),
                Mnemonic = Marshal.PtrToStringAnsi(wallet.mnemonic),
                PublicBase64Key = Marshal.PtrToStringAnsi(wallet.public_base64_key),
                PrivateKey = Marshal.PtrToStringAnsi(wallet.private_key),
                KeyScheme = Marshal.PtrToStringAnsi(wallet.key_scheme)
            };
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in WalletTest: {ex.Message}");
            return default(WalletData);
        }
    }

    public void GenerateAndAddNew()
    {
        generate_and_add_key();

    }

    public struct WalletData
    {
        public string Address;
        public string Mnemonic;
        public string PublicBase64Key;
        public string PrivateKey;
        public string KeyScheme;
        public WalletData FromWallet(Wallet wallet)
        {
            return new WalletData
            {
                Address = Marshal.PtrToStringAnsi(wallet.address),
                Mnemonic = Marshal.PtrToStringAnsi(wallet.mnemonic),
                PublicBase64Key = Marshal.PtrToStringAnsi(wallet.public_base64_key),
                PrivateKey = Marshal.PtrToStringAnsi(wallet.private_key),
                KeyScheme = Marshal.PtrToStringAnsi(wallet.key_scheme)
            };
        }
        public void Show()
        {
            Debug.Log($"Address: {Address}");
            Debug.Log($"Mnemonic: {Mnemonic}");
            Debug.Log($"Public Base64 Key: {PublicBase64Key}");
            Debug.Log($"Private Key: {PrivateKey}");
            Debug.Log($"Key Scheme: {KeyScheme}");
        }
    }
}
