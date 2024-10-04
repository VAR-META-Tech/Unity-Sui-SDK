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
2. **`Prefabs/`**:: This directory contains all the project's prefabs.
3. **`Scenes/`**:: This directory contains all the scenes.
4. **`Scripts/`**:: This directory contains all the project's scripts.
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

### License ###
This project is licensed under the Apache-2.0 License. Refer to the LICENSE.txt file for details.
