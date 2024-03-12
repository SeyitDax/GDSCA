using UnityEngine;
using UnityEngine.UI;
using static NativeGallery;
using TMPro;
using System;


#if UNITY_EDITOR || UNITY_ANDROID
using NativeGalleryNamespace;
#endif

public class UserProfile : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_ANDROID
    [SerializeField]
#endif

    public RawImage profileImage;
    private string imagePathKey = "UserProfileImage";
    private Texture2D currentProfileImage;

    [SerializeField]
    TextMeshProUGUI profileID;

    void Start()
    {
        LoadProfileImage();
        GenerateID();
    }

    // Function to load the profile image from PlayerPrefs
    void LoadProfileImage()
    {
        string savedImagePath = PlayerPrefs.GetString(imagePathKey, "");
        if (!string.IsNullOrEmpty(savedImagePath))
        {
            byte[] imageData = System.Convert.FromBase64String(savedImagePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);
            currentProfileImage = texture;
            profileImage.texture = texture;
        }
    }

    // Function to save the profile image to PlayerPrefs
    void SaveProfileImage(Texture2D texture)
    {
        Debug.Log("Trying to Save the image");

        // Create a copy of the texture
        Texture2D copyTexture = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1);
        Graphics.CopyTexture(texture, copyTexture);

        // Mark the copy texture as readable
        copyTexture.Apply();

        // Encode the copy texture to JPG
        byte[] imageData = copyTexture.EncodeToJPG();
        string savedImagePath = System.Convert.ToBase64String(imageData);
        PlayerPrefs.SetString(imagePathKey, savedImagePath);
        PlayerPrefs.Save();

        // Clean up resources
        Destroy(copyTexture);

        Debug.Log("Saved Ýmage succesfully");
    }


    // Function to allow the user to change their profile picture
    public void ChangeProfilePicture()
    {
        // Calling NativeGallery.GetImageFromGallery method with a callback function
        NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Assign texture to the profile image
                profileImage.texture = texture;

                // Save the selected image to PlayerPrefs
                SaveProfileImage(texture);
            }
        }, "Select an image", "image/*");
    }

    void GenerateID()
    {
        if(PlayerPrefs.GetString("UserID", "") == "")
        {
            Guid newGuid = Guid.NewGuid();
            string guidID = newGuid.ToString();

            string[] evenNums = new string[] { "0", "2", "4", "6", "8" };

            guidID = guidID.Substring(0,guidID.Length-1);
            guidID = guidID + evenNums[UnityEngine.Random.Range(0,4)];
            

            PlayerPrefs.SetString("UserID", guidID);
        }
        else
        {
            profileID.text = PlayerPrefs.GetString("UserID", "");
        }
    }
}
