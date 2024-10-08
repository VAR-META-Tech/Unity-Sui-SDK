using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class SuiMultisig : MonoBehaviour
{
    const string LIB_NAME = SuiConst.MACOS_LIB_NAME;
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
        public IntPtr error;

        // Constructor from hex string
        public CU8Array(string hexString)
        {
            byte[] byteArray = HexStringToBytes(hexString);
            data = Marshal.UnsafeAddrOfPinnedArrayElement(byteArray, 0);
            len = (uint)byteArray.Length;
            error = IntPtr.Zero;
        }

        // Method to convert byte data to hex string
        public string BytesToHex()
        {
            if (data == IntPtr.Zero || len == 0)
            {
                return string.Empty;
            }

            byte[] dataArray = new byte[len];
            Marshal.Copy(data, dataArray, 0, (int)len);

            StringBuilder hex = new StringBuilder(dataArray.Length * 2);
            foreach (byte b in dataArray)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
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

    [StructLayout(LayoutKind.Sequential)]
    public struct MultiSig
    {
        public IntPtr address;
        public CU8Array bytes;
        public IntPtr error;
    }

    [DllImport(LIB_NAME)]
    public static extern MultiSig get_or_create_multisig(CStringArray addresses, CU8Array weights, ushort threshold);

    [DllImport(LIB_NAME)]
    public static extern CU8Array create_transaction(string fromAddress, string toAddress, ulong amount);

    [DllImport(LIB_NAME)]
    private static extern IntPtr sign_and_execute_transaction_miltisig(CU8Array multisig, CU8Array tx, CStringArray addresses);

    public TransactionResult CreateTransaction(string fromAddress, string toAddress, ulong amount)
    {
        CU8Array result = create_transaction(fromAddress, toAddress, amount);
        if (result.error == IntPtr.Zero)
        {
            byte[] data = new byte[result.len];
            Marshal.Copy(result.data, data, 0, (int)result.len);
            return new TransactionResult(data, null);
        }
        else
        {
            string error = Marshal.PtrToStringAnsi(result.error);
            Marshal.FreeHGlobal(result.error); // Free the error message
            return new TransactionResult(null, error);
        }
    }

    public string SignAndExecuteTransaction(string multisigHex, string txHex, string[] addresses)
    {
        CU8Array multisig = new CU8Array(multisigHex);
        CU8Array tx = new CU8Array(txHex);
        CStringArray cAddresses = CreateCStringArray(addresses);
        IntPtr resultPtr = sign_and_execute_transaction_miltisig(multisig, tx, cAddresses);
        string resultString = Marshal.PtrToStringAnsi(resultPtr);
        FreeCStringArray(cAddresses);
        return resultString;
    }
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

    public class TransactionResult
    {
        public byte[] Data { get; }
        public string Error { get; }

        public TransactionResult(byte[] data, string error)
        {
            Data = data;
            Error = error;
        }

        public bool IsSuccess => Error == null;

        public void Print()
        {
            if (IsSuccess)
            {
                Debug.Log("Transaction created successfully.");
                Debug.Log($"Data length: {Data.Length}");
                Debug.Log($"Data: {BitConverter.ToString(Data).Replace("-", " ")}");
            }
            else
            {
                Debug.Log($"Error: {Error}");
            }
        }

        public string BytesToHexString()
        {
            StringBuilder hex = new StringBuilder(Data.Length * 2);
            foreach (byte b in Data)
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
    private void FreeCStringArray(CStringArray cArray)
    {
        IntPtr[] stringPtrs = new IntPtr[cArray.len];
        Marshal.Copy(cArray.data, stringPtrs, 0, (int)cArray.len);
        foreach (IntPtr ptr in stringPtrs)
        {
            Marshal.FreeHGlobal(ptr);
        }
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


