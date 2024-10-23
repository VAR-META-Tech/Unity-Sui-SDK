using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class SuiTransactionBuilder
{
    const string LIB_NAME = SuiConst.MACOS_LIB_NAME;

    [DllImport(LIB_NAME)]
    private static extern IntPtr create_type_tags();

    [DllImport(LIB_NAME)]
    private static extern void add_type_tag(IntPtr type_tags, string tag);

    [DllImport(LIB_NAME)]
    private static extern void destroy_type_tags(IntPtr type_tags);

    [DllImport(LIB_NAME)]
    private static extern IntPtr create_arguments();

    [DllImport(LIB_NAME)]
    private static extern void destroy_arguments(IntPtr arguments);

    [DllImport(LIB_NAME)]
    private static extern void add_argument_gas_coin(IntPtr arguments);

    [DllImport(LIB_NAME)]
    private static extern void add_argument_result(IntPtr arguments, ushort value);

    [DllImport(LIB_NAME)]
    private static extern void add_argument_input(IntPtr arguments, ushort value);

    [DllImport(LIB_NAME)]
    private static extern void add_argument_nested_result(IntPtr arguments, ushort value1, ushort value2);

    [DllImport(LIB_NAME)]
    private static extern void make_pure(IntPtr builder, IntPtr arguments, IntPtr value);

    [DllImport(LIB_NAME)]
    private static extern IntPtr create_builder();

    [DllImport(LIB_NAME)]
    private static extern void destroy_builder(IntPtr builder);

    [DllImport(LIB_NAME)]
    private static extern void add_move_call_command(IntPtr builder, string package, string module, string function, IntPtr type_arguments, IntPtr arguments);

    [DllImport(LIB_NAME)]
    private static extern void add_transfer_object_command(IntPtr builder, IntPtr agreements, IntPtr recipient);

    [DllImport(LIB_NAME)]
    private static extern void add_split_coins_command(IntPtr builder, IntPtr coin, IntPtr agreements);

    [DllImport(LIB_NAME)]
    private static extern void add_merge_coins_command(IntPtr builder, IntPtr coin, IntPtr agreements);

    [DllImport(LIB_NAME)]
    private static extern IntPtr execute_transaction(IntPtr builder, string sender, ulong gas_budget);

    [DllImport(LIB_NAME)]
    private static extern IntPtr execute_transaction_allow_sponser(IntPtr builder, string sender, ulong gas_budget, string sponser);

    private static IntPtr builder;
    public static void CreateBuilder()
    {
        builder = create_builder();
    }

    public static void DestroyBuilder()
    {
        destroy_builder(builder);
    }

    public static SuiTypeTags CreateTypeTags()
    {
        IntPtr typeTags = create_type_tags();
        return new SuiTypeTags(typeTags);
    }

    public static void AddTypeTag(SuiTypeTags typeTags, string tag)
    {
        add_type_tag(typeTags.GetData(), tag);
    }

    public static SuiAgruments CreateArguments()
    {
        IntPtr arguments = create_arguments();
        return new SuiAgruments(arguments);
    }

    public static void AddArgumentGasCoin(SuiAgruments arguments)
    {
        add_argument_gas_coin(arguments.GetData());
    }

    public static void AddArgumentResult(SuiAgruments arguments, ushort value)
    {
        add_argument_result(arguments.GetData(), value);
    }

    public static void AddArgumentInput(SuiAgruments arguments, ushort value)
    {
        add_argument_input(arguments.GetData(), value);
    }

    public static void AddArgumentNestedResult(SuiAgruments arguments, ushort value1, ushort value2)
    {
        add_argument_nested_result(arguments.GetData(), value1, value2);
    }

    public static void MakePure(SuiAgruments arguments, SuiPure value)
    {
        make_pure(builder, arguments.GetData(), value.GetData());
    }

    public static void AddMoveCallCommand(string package, string module, string function, SuiTypeTags typeArguments, SuiAgruments arguments)
    {
        add_move_call_command(builder, package, module, function, typeArguments.GetData(), arguments.GetData());
    }

    public static void AddTransferObjectCommand(SuiAgruments agreements, SuiAgruments recipient)
    {
        add_transfer_object_command(builder, agreements.GetData(), recipient.GetData());
    }

    public static void AddSplitCoinsCommand(SuiAgruments coin, SuiAgruments agreements)
    {
        add_split_coins_command(builder, coin.GetData(), agreements.GetData());
    }

    public static void AddMergeCoinsCommand(SuiAgruments coin, SuiAgruments agreements)
    {
        add_merge_coins_command(builder, coin.GetData(), agreements.GetData());
    }

    public static String ExecuteTransaction(string sender, ulong gasBudget)
    {
        IntPtr resultPtr = execute_transaction(builder, sender, gasBudget);
        return Marshal.PtrToStringAnsi(resultPtr);
    }

    public static String ExecuteTransactionAllowSponser(string sender, ulong gasBudget, string sponser)
    {
        IntPtr resultPtr = execute_transaction_allow_sponser(builder, sender, gasBudget, sponser);
        return Marshal.PtrToStringAnsi(resultPtr);
    }
}

public class SuiAgruments
{
    private IntPtr arguments;

    public SuiAgruments(IntPtr arguments)
    {
        this.arguments = arguments;
    }

    public IntPtr GetData()
    {
        return arguments;
    }
}

public class SuiTypeTags
{
    private IntPtr typeTags;

    public SuiTypeTags(IntPtr typeTags)
    {
        this.typeTags = typeTags;
    }


    public IntPtr GetData()
    {
        return typeTags;
    }
}