using System;
using System.Runtime.InteropServices;

public class SuiBCS
{
    const string LIB_NAME = SuiConst.MACOS_LIB_NAME;

    [DllImport(LIB_NAME)]
    private static extern IntPtr bsc_basic(string type_, string data);

    public enum SuiType
    {
        U8,
        U64,
        U128,
        U256,
        I8,
        I64,
        I128,
        F32,
        F64,
        Bool,
        Uleb128,
        String,
        Address
    }

    public static SuiPure BscBasic(SuiType type, string data) {
        string typeStr = type.ToString().ToLower();
        IntPtr result = bsc_basic(typeStr, data);
        return new SuiPure(result);
    }
}

public class SuiPure
{
    private IntPtr data;

    public SuiPure(IntPtr data)
    {
        this.data = data;
    }

    public IntPtr GetData(){
        return data;
    }
}