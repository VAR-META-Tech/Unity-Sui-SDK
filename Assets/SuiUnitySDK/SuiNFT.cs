using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SuiNFT : MonoBehaviour
{
    const string LIB_NAME = SuiConst.MACOS_LIB_NAME;
    const string NFT_PACKAGE_ID = "0x43d037dda49e37c977a1e2a4ed261147659a2913867d439a101c57b41216c216";
    const string NFT_OBJECT_TYPE = "0x43d037dda49e37c977a1e2a4ed261147659a2913867d439a101c57b41216c216::nft::NFT";
    public struct CSuiObjectData
    {
        public string object_id;
        public ulong version;
        public string digest;
        public string type_;
        public string owner;
        public string previous_transaction;
        public ulong storage_rebate;
        public string display;
        public string content;
        public string bcs;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CSuiObjectDataRaw
    {
        public IntPtr object_id;
        public ulong version;
        public IntPtr digest;
        public IntPtr type_;
        public IntPtr owner;
        public IntPtr previous_transaction;
        public ulong storage_rebate;
        public IntPtr display;
        public IntPtr content;
        public IntPtr bcs;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CSuiObjectDataArray
    {
        public IntPtr data;
        public IntPtr len;
    }

    [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr mint_nft(string package_id, string sender_address, string name, string description, string uri);

    [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr transfer_nft(string package_id, string sender_address, string nft_id, string recipient_address);

    [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern CSuiObjectDataArray get_wallet_objects(string address, string object_type);

    public string Mint_NFT(string sender_address, string name, string description, string uri)
    {
        IntPtr resultPtr = mint_nft(NFT_PACKAGE_ID, sender_address, name, description, uri);
        string result = Marshal.PtrToStringAnsi(resultPtr);
        Marshal.FreeHGlobal(resultPtr);
        return result;
    }

    public string Transfer_NFT(string sender_address, string nft_id, string recipient_address)
    {
        IntPtr resultPtr = transfer_nft(NFT_PACKAGE_ID, sender_address, nft_id, recipient_address);
        string result = Marshal.PtrToStringAnsi(resultPtr);
        Marshal.FreeHGlobal(resultPtr);
        return result;
    }

    private static string PtrToString(IntPtr ptr)
    {
        return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
    }

    private static CSuiObjectData ConvertRawToManaged(CSuiObjectDataRaw rawObj)
    {
        return new CSuiObjectData
        {
            object_id = PtrToString(rawObj.object_id),
            version = rawObj.version,
            digest = PtrToString(rawObj.digest),
            type_ = PtrToString(rawObj.type_),
            owner = PtrToString(rawObj.owner),
            previous_transaction = PtrToString(rawObj.previous_transaction),
            storage_rebate = rawObj.storage_rebate,
            display = PtrToString(rawObj.display),
            content = PtrToString(rawObj.content),
            bcs = PtrToString(rawObj.bcs),
        };
    }

    private static List<CSuiObjectData> ConvertArrayToList(CSuiObjectDataArray array)
    {
        int length = array.len.ToInt32();
        List<CSuiObjectData> list = new List<CSuiObjectData>(length);
        for (int i = 0; i < length; ++i)
        {
            IntPtr dataPtr = new IntPtr(array.data.ToInt64() + i * Marshal.SizeOf(typeof(CSuiObjectDataRaw)));
            CSuiObjectDataRaw rawObj = Marshal.PtrToStructure<CSuiObjectDataRaw>(dataPtr);
            list.Add(ConvertRawToManaged(rawObj));
        }
        return list;
    }

    public List<CSuiObjectData> Get_wallet_objects(string address)
    {
        Debug.Log("Get wallet objects of : " + address);

        string object_type = NFT_OBJECT_TYPE;
        CSuiObjectDataArray result = get_wallet_objects(address, object_type);
        List<CSuiObjectData> objects = ConvertArrayToList(result);
        foreach (var obj in objects)
        {
            Debug.Log("Object ID: " + obj.object_id);
            Debug.Log("Version: " + obj.version);
            Debug.Log("Digest: " + obj.digest);
            Debug.Log("Type: " + obj.type_);
            Debug.Log("Owner: " + obj.owner);
            Debug.Log("Previous Transaction: " + obj.previous_transaction);
            Debug.Log("Storage Rebate: " + obj.storage_rebate);
            Debug.Log("Display: " + obj.display);
            Debug.Log("Content: " + obj.content);
            Debug.Log("BCS: " + obj.bcs);
        }
        return objects;
    }
}
