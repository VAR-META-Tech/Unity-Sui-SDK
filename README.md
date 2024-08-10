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

- [x]  Compatibility with main, dev, and test networks.
- [x]  Comprehensive Unit and Integration Test coverage.


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

### Using Unity-Sui-SDK

   1. Build the libsui_rust_sdk file from https://github.com/VAR-META-Tech/Rust2C-Sui-SDK or copy the file in the Plugin folder; and place in your project.
    
   2. There are multiple scripts to handle the integration with SUi network included (WalletLib.cs, BalanceLib.cs, NFTLib.cs, MultisigLib.cs), use can run test in this project or can include these files to your own project.

Note: For the NFTLib.cs you need to build the NFT package from https://github.com/VAR-META-Tech/Move-Sui-SDK and replace the received NFT_PACKAGE_ID and NFT_OBJECT_TYPE

### Examples ###

The SDK comes with several examples that show how to leverage the Rust2C-Sui-SDK to its full potential. The examples include Wallet Creation and Management, Token Transfers,  NFT Minting, Account Funding, and Multi-signature.


### License ###
This project is licensed under the Apache-2.0 License. Refer to the LICENSE.txt file for details.
