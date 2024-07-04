using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class MultisigLib : MonoBehaviour
{
    const string LIB_NAME = "libsui_rust_sdk";
    [StructLayout(LayoutKind.Sequential)]
    public struct CStringArray
    {
        public IntPtr data;
        public int len;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CU8Array
    {
        public IntPtr data;
        public uint len;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MultiSig
    {
        public IntPtr address;
        public CU8Array bytes;
        public IntPtr error;
    }

    [DllImport(LIB_NAME)]
    public static extern MultiSig get_or_create_multisig(CStringArray addresses, CU8Array weights, ushort threshold);

    public MultiSigData Get_or_create_multisig(string[] addresses, byte[] weights, ushort threshold)
    {
        // Prepare input data for P/Invoke
        CStringArray cAddresses = CreateCStringArray(addresses);
        CU8Array cWeights = CreateCU8Array(weights);

        // Call the Rust function
        MultiSig result = get_or_create_multisig(cAddresses, cWeights, threshold);

        // Create MultiSigData instance
        MultiSigData multiSigData = new MultiSigData(result);

        // Process and display the result
        if (string.IsNullOrEmpty(multiSigData.Error))
        {
            Debug.Log($"MultiSigData: {multiSigData}");
        }
        else
        {
            Debug.LogError($"Error: {multiSigData.Error}");
        }

        // Clean up allocated memory
        FreeCStringArray(cAddresses);
        FreeCU8Array(cWeights);
        return multiSigData;
    }

    private static CStringArray CreateCStringArray(string[] strings)
    {
        IntPtr[] ptrs = new IntPtr[strings.Length];
        for (int i = 0; i < strings.Length; i++)
        {
            ptrs[i] = Marshal.StringToHGlobalAnsi(strings[i]);
        }
        GCHandle handle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
        return new CStringArray
        {
            data = handle.AddrOfPinnedObject(),
            len = strings.Length
        };
    }

    private static CU8Array CreateCU8Array(byte[] bytes)
    {
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        return new CU8Array
        {
            data = handle.AddrOfPinnedObject(),
            len = (uint)bytes.Length
        };
    }

    private static void FreeCStringArray(CStringArray cArray)
    {
        IntPtr[] ptrs = new IntPtr[cArray.len];
        Marshal.Copy(cArray.data, ptrs, 0, cArray.len);
        foreach (IntPtr ptr in ptrs)
        {
            Marshal.FreeHGlobal(ptr);
        }
        GCHandle handle = GCHandle.FromIntPtr(cArray.data);
        handle.Free();
    }

    private static void FreeCU8Array(CU8Array cArray)
    {
        GCHandle handle = GCHandle.FromIntPtr(cArray.data);
        handle.Free();
    }

    // Define the structure for MultiSigData
    public struct MultiSigData
    {
        public string Address;
        public byte[] Bytes;
        public string Error;

        public MultiSigData(MultiSig multiSig)
        {
            Address = Marshal.PtrToStringAnsi(multiSig.address);
            Bytes = new byte[multiSig.bytes.len];
            if (multiSig.bytes.data != IntPtr.Zero)
            {
                Marshal.Copy(multiSig.bytes.data, Bytes, 0, (int)multiSig.bytes.len);
            }
            Error = Marshal.PtrToStringAnsi(multiSig.error);
        }

        public override string ToString()
        {
            return $"Address: {Address}, Bytes: {BitConverter.ToString(Bytes)}, Error: {Error}";
        }
        // Method to convert bytes to a hexadecimal string
        public string BytesToHexString()
        {
            StringBuilder hex = new StringBuilder(Bytes.Length * 2);
            foreach (byte b in Bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        // Method to convert a hexadecimal string to a byte array
        public static byte[] HexStringToBytes(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
