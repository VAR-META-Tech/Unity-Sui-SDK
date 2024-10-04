using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SuiWallet : MonoBehaviour
{
    #if UNITY_STANDALONE_OSX
        private const string LIB_NAME = SuiConst.MACOS_LIB_NAME;
    #elif UNITY_STANDALONE_WIN
        private const string LIB_NAME = "path/to/your/library.dll";
    #elif UNITY_STANDALONE_LINUX
        private const string LIB_NAME = "path/to/your/library.so";
    #endif

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

    [StructLayout(LayoutKind.Sequential)]
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

    [StructLayout(LayoutKind.Sequential)]
    public struct ImportResult
    {
        public int status;
        public IntPtr address;
        public IntPtr error;

        public string Address
        {
            get { return Marshal.PtrToStringAnsi(address); }
        }

        public string Error
        {
            get { return Marshal.PtrToStringAnsi(error); }
        }
    }


    [DllImport(LIB_NAME)]
    private static extern WalletList get_wallets();

    [DllImport(LIB_NAME)]
    private static extern IntPtr generate_and_add_key();

    [DllImport(LIB_NAME)]
    private static extern void free_wallet_list(WalletList walletList);

    [DllImport(LIB_NAME)]
    private static extern System.IntPtr import_from_private_key(System.IntPtr privateKey);

    [DllImport(LIB_NAME)]
    private static extern System.IntPtr import_from_mnemonic(System.IntPtr mnemonic,System.IntPtr key_scheme);

    [DllImport(LIB_NAME)]
    private static extern IntPtr generate_wallet(string key_scheme, string word_length);

    [DllImport(LIB_NAME)]
    public static extern void free_wallet(IntPtr wallet);

    private WalletList walletList;

    // The static instance of the singleton
    private static SuiWallet _instance;

    // Property to access the instance
    public static SuiWallet Instance
    {
        get
        {
            // If the instance is null, find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<SuiWallet>();

                // If still null, create a new GameObject and attach this script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SuiWallet).ToString());
                    _instance = singletonObject.AddComponent<SuiWallet>();
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

    public bool ImportFromPrivateKey(string privateKey)
    {
        System.IntPtr keyBase64Ptr = Marshal.StringToHGlobalAnsi(privateKey);
        try
        {
            IntPtr resultPtr = import_from_private_key(keyBase64Ptr);
            //covert resultPtr to ImportResult struct
            ImportResult result = Marshal.PtrToStructure<ImportResult>(resultPtr);
            if (result.status == 0)
            {
                Debug.Log($"Imported address: {result.Address}");
                return true;
            }
            else
            {
                Debug.LogError($"Error importing private key: {result.Error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ImportFromPrivateKey exception: " + ex.Message);
            return false;
        }
        finally
        {
            // Free the unmanaged string memory
            Marshal.FreeHGlobal(keyBase64Ptr);
        }
    }

    public bool ImportFromMnemonic(string mnemonic)
    {
         IntPtr mnemonicPtr = Marshal.StringToHGlobalAnsi(mnemonic);
         IntPtr keySchemePtr = Marshal.StringToHGlobalAnsi("ED25519");
        string jsonResult = string.Empty;

        try
        {
            IntPtr resultPtr = import_from_mnemonic(mnemonicPtr, keySchemePtr);
            ImportResult result = Marshal.PtrToStructure<ImportResult>(resultPtr);
            if (result.status == 0)
            {
                Debug.Log($"Imported address: {result.Address}");
                return true;
            }
            else
            {
                Debug.LogError($"Error importing mnemonic: {result.Error}");
                return false;
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("ImportFromMnemonic exception: " + ex.Message);
            return false;
        }
        finally
        {
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
            IntPtr walletPtr = SuiWallet.generate_wallet(key_scheme, word_length);
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

}
