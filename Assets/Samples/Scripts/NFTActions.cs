using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SuiNFT;

public class NFTActions : MonoBehaviour
{
    private SuiNFT nftLib;
    [System.Serializable]
    public class NFT
    {
        public string name;
        public string description;
        public string url;

    }
    public GameObject listItemPrefab;
    public Transform content;
    public TMP_InputField nftName;
    public TMP_InputField nftDescription;
    public TMP_InputField nftUrl;
    public TMP_InputField nftId;
    public TMP_InputField toAddress;
    public TMP_Dropdown addresses;
    List<CSuiObjectData> nfts;
    void Awake()
    {
        if (nftLib == null)
        {
            nftLib = FindObjectOfType<SuiNFT>();
        }
        addresses.onValueChanged.AddListener(delegate
     {
         DropdownValueChanged(addresses);
     });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        Get_wallet_objects();
    }
    public void Get_wallet_objects()
    {
        Debug.Log("Load Get_wallet_objects of: " + addresses.options[addresses.value].text);
        if (nftLib == null) return;
        Debug.Log("Get_wallet_objects of: " + addresses.options[addresses.value].text);
        nfts = nftLib.Get_wallet_objects(addresses.options[addresses.value].text);
        PopulateList();
    }

    void PopulateList()
    {
        if (!content) return;
        // Clear old list items
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var nft in nfts)
        {
            // Create a new list item from the prefab
            GameObject newItem = Instantiate(listItemPrefab, content);

            // Find and set the text components of the list item
            TextMeshProUGUI[] textComponents = newItem.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textComponent in textComponents)
            {
                switch (textComponent.name)
                {
                    case "ObjectID":
                        textComponent.text = nft.object_id;
                        break;
                    case "Version":
                        textComponent.text = nft.version.ToString();
                        break;
                    case "Digest":
                        textComponent.text = nft.digest;
                        break;
                    case "Type":
                        textComponent.text = nft.type_;
                        break;
                    case "Content":
                        textComponent.text = nft.content;
                        break;
                }
                NFT nftContent = JsonUtility.FromJson<NFT>(nft.content);

                Debug.Log("URL: " + nftContent.url);
                Image[] images = newItem.GetComponentsInChildren<Image>();
                foreach (Image image in images)
                {
                    Debug.Log(image.name);
                    if (image.name == "nftImage")
                    {
                        StartCoroutine(LoadImageFromUrl(image, nftContent.url));
                    }
                }
                Button[] buttons = newItem.GetComponentsInChildren<Button>();
                foreach (Button btn in buttons)
                {
                    Debug.Log(btn.name);
                    if (btn.name == "CopyObjectID")
                    {
                        btn.onClick.AddListener(() => CopyToClipboard(nft.object_id));
                    }
                }
            }
        }
    }
    IEnumerator LoadImageFromUrl(Image imageComponent, string url)
    {
        Debug.Log("load image from url: " + url);
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            // Set User-Agent header
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading image: " + request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Log texture details
                Debug.Log("Texture Width: " + texture.width);
                Debug.Log("Texture Height: " + texture.height);

                if (texture != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    imageComponent.sprite = sprite;
                }
                else
                {
                    Debug.LogError("Downloaded texture is null.");
                }
            }
        }
    }
    void CopyToClipboard(string text)
    {
        GUIUtility.systemCopyBuffer = text;
    }
    public void Mint_NFT()
    {
        string result = nftLib.Mint_NFT(addresses.options[addresses.value].text, nftName.text, nftDescription.text, nftUrl.text);
        Debug.Log(result);
    }
    public void Transfer_NFT()
    {
        string result = nftLib.Transfer_NFT(addresses.options[addresses.value].text, nftId.text, toAddress.text);
        Debug.Log(result);
    }
}
