![645bca632d1c27a85cee236d_sui-logo8d3c44e](https://github.com/user-attachments/assets/5b74fbae-1a87-427d-9c57-8bc164ca09cb)



# Unity-Sui-SDK #

Unity-Sui-SDK is a sample example to help developers integrate Sui blockchain technology into their C# and Unity projects.

- [Project Layout](#project-structure)
- [Features](#features)
- [Requirements](#requirements)
- [Dependencies](#dependencies)
- [Installation](#installation)
- [Using Unity-Sui-SDK](#using-unity-sui-sdk)
- [Examples](#examples)
- [License](#license)

### Project Structure ###  

1. **`Plugins/`**:: This directory contains the project's library files, including libsui_rust_sdk.dylib library to integrate with SUI network.
2. **`SuiUnitySDK/`**:: The core SDK files and scripts.
3. **`Samples/`**:: Includes sample scenes or scripts demonstrating how to use the SDK.
5. **`TextMesh Pro/`**: This directory contains the TextMesh Pro package assets.

### Features ###

#### General
- [x] Compatibility with main, dev, and test networks.
- [x] Integration with Sui blockchain using native libraries.
- [x] Cross-platform support (macOS, Windows, Linux).

#### SuiNFT.cs
- [x] Mint new NFTs.
- [x] Transfer NFTs to other addresses.
- [x] Retrieve wallet objects related to NFTs.
- [x] Conversion between raw and managed data structures for NFT objects.

#### SuiBCS.cs
- [x] Basic serialization and deserialization of Sui types.
- [x] Support for various Sui types including integers, floats, booleans, strings, and addresses.
- [x] Conversion of Sui types to BCS (Binary Canonical Serialization) format.

#### SuiMultisig.cs
- [x] Create and manage multisig wallets.
- [x] Create transactions from multisig wallets.
- [x] Sign and execute transactions using multisig wallets.
- [x] Handling of multisig data structures and transaction results.

#### SuiTransactionBuilder.cs
- [x] Create and manage transaction builders.
- [x] Add various types of commands to transactions (e.g., move call, transfer object, split coins, merge coins).
- [x] Execute transactions with or without a sponsor.

#### SuiWallet.cs
- [x] Singleton pattern for easy access to wallet functionalities.
- [x] Generate new wallets with specified key schemes and word lengths.
- [x] Import wallets from private keys.
- [x] Import wallets from mnemonics.
- [x] List all wallets.
- [x] Display wallet details.
- [x] Generate and add new keys to the wallet.

### Requirements ###

| Platforms                              | Unity Version | Status       |
| -------------------------------------- | ------------- | ------------ |
| Mac                                    | Unity 2022    | Fully Tested |


### Dependencies
- https://github.com/VAR-META-Tech/Rust2C-Sui-SDK

### Installation ###
# Installation Guide

This guide provides step-by-step instructions for installing and setting up on macOS platforms. Ensure you have the following prerequisites installed to build the project:

## Prerequisites
- **Visual Studio Code** with C# development environment
- **Install Sui** Follow this guide to install Sui https://docs.sui.io/guides/developer/getting-started/sui-install
## Project Setup
Run follow command to setting Environment before testing:
1. Check Sui Client Environment:  
    ```sh 
    Sui client envs
    ```
 **NOTE:If you dont have DevNet Please Run CMD :**
```sh 
    sui client new-env --alias devnet --rpc https://fullnode.devnet.sui.io:443
```
2. Switch to devnet network: 
    ```sh 
    sui client switch --env devnet
    ```
3. Check current network:
    ```sh 
    sui client active-env
    ```
 **NOTE: The return should be devnet**
 
4. Get the active address: 
    ```sh
    sui client active-address
    ```
5. Request token:
    ```sh
    sui client faucet 
    ```
 **NOTE: Wait for 60s to get the tokens**

6. Check the gas coin objects for the active address: 
    ```sh
    sui client gas
    ```

### Install Unity-Sui-SDK

   1. Via 'Add package from git URL' in Unity Package Manager
   2. Download the latest sui-unity-sdk.unitypackage from the releases: https://github.com/VAR-META-Tech/Unity-Sui-SDK/releases

Note: For the NFTLib.cs you need to build the NFT package from https://github.com/VAR-META-Tech/Move-Sui-SDK and replace the received NFT_PACKAGE_ID and NFT_OBJECT_TYPE

### Examples ###

The SDK comes with several examples that show how to leverage the Rust2C-Sui-SDK to its full potential. The examples include Wallet Creation and Management, Token Transfers,  NFT Minting, Account Funding, and Multi-signature.

#### Network Building
The `SuiClient` class provides methods to build and switch between different Sui networks (testnet, devnet, mainnet). Below are some examples of how to use the `SuiClient` class.

##### Build Testnet
To build the testnet environment:
```csharp
bool success = SuiClient.Instance.BuildTestnet();
if (success)
{
    Debug.Log("Testnet built successfully.");
}
else
{
    Debug.LogError("Failed to build testnet.");
}
```

##### Build Devnet
To build the devnet environment:
```csharp
bool success = SuiClient.Instance.BuildDevnet();
if (success)
{
    Debug.Log("Devnet built successfully.");
}
else
{
    Debug.LogError("Failed to build devnet.");
}
```

##### Build Mainnet
To build the mainnet environment:
```csharp
bool success = SuiClient.Instance.BuildMainnet();
if (success)
{
    Debug.Log("Mainnet built successfully.");
}
else
{
    Debug.LogError("Failed to build mainnet.");
}
```

#### Wallet
The `SuiWallet` class provides various functionalities to manage wallets in your Unity project. Below are some examples of how touse the `SuiWallet` class.

##### Generate a New Wallet
To generate a new wallet with a specified key scheme and word length:
```csharp
SuiWallet.WalletData newWallet = SuiWallet.Instance.GenerateWallet("ED25519", "12");
newWallet.Show();
```
##### Import Wallet from Private Key
To import a wallet using a private key:
```csharp
bool success = SuiWallet.Instance.ImportFromPrivateKey("your_private_key_here");
if (success)
{
    Debug.Log("Wallet imported successfully.");
}
else
{
    Debug.LogError("Failed to import wallet.");
}
```
##### Import Wallet from Mnemonic
To import a wallet using a mnemonic phrase:
```csharp
bool success = SuiWallet.Instance.ImportFromMnemonic("your_mnemonic_phrase_here");
if (success)
{
    Debug.Log("Wallet imported successfully.");
}
else
{
    Debug.LogError("Failed to import wallet.");
}
```
##### List All Wallets
To list all wallets:
```csharp
SuiWallet.WalletData[] wallets = SuiWallet.Instance.LoadWallets();
foreach (var wallet in wallets)
{
    wallet.Show();
}
```
##### Generate and Add New Key
To generate and add a new key to the wallet:
```csharp
SuiWallet.Instance.GenerateAndAddNew();
```

#### Transactions
The `SuiTransactionBuilder` class allows you to create and manage transactions. Below are some examples of how to use the `SuiTransactionBuilder` class.

##### Create a New Transaction
To create a new transaction:
```csharp
SuiTransactionBuilder transactionBuilder = new SuiTransactionBuilder();
```

##### Add a Move Call Command
To add a move call command to the transaction:
```csharp
SuiTypeTags typeTags = transactionBuilder.CreateTypeTags();
SuiAgruments arguments = transactionBuilder.CreateArguments();
transactionBuilder.AddMoveCallCommand("package_id", "module_name", "function_name", typeTags, arguments);
```

##### Add a Transfer Object Command
To add a transfer object command to the transaction:
```csharp
SuiAgruments agreements = transactionBuilder.CreateArguments();
SuiAgruments recipient = transactionBuilder.CreateArguments();
transactionBuilder.AddTransferObjectCommand(agreements, recipient);
```

##### Execute the Transaction
To execute the transaction:
```csharp
string result = transactionBuilder.ExecuteTransaction("sender_address", 1000);
Debug.Log(result);
```

##### Execute the Transaction with a Sponsor
To execute the transaction with a sponsor:
```csharp
string result = transactionBuilder.ExecuteTransactionAllowSponser("sender_address", 1000, "sponsor_address");
Debug.Log(result);
```


#### Basic Serialization and Deserialization
The `SuiBCS` class provides methods for basic serialization and deserialization of Sui types. Below are some examples of how to use the `SuiBCS` class.

##### Serialize Data
To serialize data of a specific Sui type:
```csharp
string data = "12345";
SuiPure serializedData = SuiBCS.BscBasic(SuiBCS.SuiType.U64, data);
IntPtr serializedPtr = serializedData.GetData();
```

#### Multisig Wallets
The `SuiMultisig` class provides functionalities to create and manage multisig wallets. Below are some examples of how to use the `SuiMultisig` class.

##### Create a Multisig Wallet
To create a new multisig wallet:
```csharp
SuiMultisig multisigWallet = new SuiMultisig();
multisigWallet.CreateMultisigWallet(new string[] { "address1", "address2", "address3" }, 2);
```

##### Create a Transaction from Multisig Wallet
To create a transaction from a multisig wallet:
```csharp
SuiTransactionBuilder transactionBuilder = multisigWallet.CreateTransaction();
```

##### Sign and Execute a Multisig Transaction
To sign and execute a transaction using a multisig wallet:
```csharp
string result = multisigWallet.SignAndExecuteTransaction(transactionBuilder, "signer_address");
Debug.Log(result);
```

#### NFT Operations
The `SuiNFT` class provides functionalities to mint, transfer, and retrieve NFT-related wallet objects. Below are some examples of how to use the `SuiNFT` class.

##### Mint a New NFT
To mint a new NFT:
```csharp
SuiNFT suiNFT = new SuiNFT();
string result = suiNFT.Mint_NFT("sender_address", "NFT Name", "NFT Description", "NFT URI");
Debug.Log("Mint Result: " + result);
```

##### Transfer an NFT
To transfer an NFT to another address:
```csharp
SuiNFT suiNFT = new SuiNFT();
string result = suiNFT.Transfer_NFT("sender_address", "nft_id", "recipient_address");
Debug.Log("Transfer Result: " + result);
```

##### Retrieve Wallet Objects
To retrieve wallet objects related to NFTs:
```csharp
SuiNFT suiNFT = new SuiNFT();
List<SuiNFT.CSuiObjectData> walletObjects = suiNFT.Get_wallet_objects("wallet_address");
foreach (var obj in walletObjects)
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
```

### License ###
This project is licensed under the Apache-2.0 License. Refer to the LICENSE.txt file for details.

