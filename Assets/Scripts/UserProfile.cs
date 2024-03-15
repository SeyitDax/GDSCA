using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Text;
using UnityEngine.Android;



#if UNITY_EDITOR || UNITY_ANDROID
using NativeGalleryNamespace;
#endif

public class UserProfile : MonoBehaviour
{
    public RawImage profileImage;
    private string imagePathKey = "UserProfileImage";
    private Texture2D currentProfileImage;
    private string imageDataFilePath; // Path to the file where image data will be saved

    [SerializeField]
    TextMeshProUGUI profileID;

    void Start()
    {
        // Initialize the file path for saving image data
        imageDataFilePath = Application.persistentDataPath + "/profileImageData.json";

        LoadProfileImage();
        GenerateID();

        Invoke("GenerateID", 0.2f);    
    }

    void LoadProfileImage()
    {
        // Attempt to read the JSON file
        if (File.Exists(imageDataFilePath))
        {
            string jsonData = File.ReadAllText(imageDataFilePath);

            // Deserialize the JSON data into a ProfileImageData object
            ProfileImageData profileImageData = JsonUtility.FromJson<ProfileImageData>(jsonData);

            if (profileImageData != null && !string.IsNullOrEmpty(profileImageData.base64ImageData))
            {
                // Convert the base64 string back to byte array
                byte[] imageData = Convert.FromBase64String(profileImageData.base64ImageData);

                // Create texture from byte array
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(imageData);
                currentProfileImage = texture;
                profileImage.texture = texture;
            }
            else
            {
                Debug.LogError("File itself seems to be null!");
            }
        }
        else
        {
            Debug.LogError("File does not exist or couldn't be found!");
        }
    }

    void SaveProfileImage(Texture2D texture)
    {
        Debug.Log("Trying to Save the image");

        texture = EnsureTextureIsReadable(texture);

        // Encode the texture to JPG
        byte[] imageData = texture.EncodeToJPG();
        string base64ImageData = Convert.ToBase64String(imageData);

        // Create a JSON object to hold the image data
        ProfileImageData profileImageData = new ProfileImageData();
        profileImageData.base64ImageData = base64ImageData;

        // Convert the object to JSON
        string jsonData = JsonUtility.ToJson(profileImageData);

        // Cheking if directory really exists
        string directoryPath = Path.GetDirectoryName(imageDataFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Write the JSON data to a file
        try
        {
            if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }

            File.WriteAllText(imageDataFilePath, jsonData);
            Debug.Log("Saved Image successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save image data: " + ex.Message);
        }
    }

    Texture2D EnsureTextureIsReadable(Texture2D texture)
    {
        RenderTexture tpRenderTexture = RenderTexture.GetTemporary(
            texture.width,
            texture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
            );

        Graphics.Blit( texture, tpRenderTexture );

        Texture2D readableTexture = new Texture2D(texture.width, texture.height);
        RenderTexture.active = tpRenderTexture;
        readableTexture.ReadPixels(new Rect(0,0, texture.width,texture.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.ReleaseTemporary(tpRenderTexture);
        return readableTexture;
    }

    void ChangeProfilePicture()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                profileImage.texture = texture;
                SaveProfileImage(texture);
                currentProfileImage = texture;
            }
        }, "Select an image", "image/*");
    }

    void GenerateID()
    {
        if (PlayerPrefs.GetString("UserID", "") == "")
        {
            Guid newGuid = Guid.NewGuid();
            string guidID = newGuid.ToString();

            string[] evenNums = new string[] { "0", "2", "4", "6", "8" };

            guidID = guidID.Substring(0, guidID.Length - 1);
            guidID = guidID + evenNums[UnityEngine.Random.Range(0, 4)];

            PlayerPrefs.SetString("UserID", guidID);
        }
        else
        {
            profileID.text = PlayerPrefs.GetString("UserID", "");
        }
    }
}

[Serializable]
public class ProfileImageData
{
    public string base64ImageData;
}
