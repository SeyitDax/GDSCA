using UnityEngine;
using UnityEngine.UI;

public class ImageCropper : MonoBehaviour
{
    public RawImage rawImage;
    public RectTransform croppingArea;

    // Function to crop the image to fit the cropping area
    public void CropImage()
    {
        Vector2 xControl;
        Vector2 yControl;

        Texture2D originalTexture = (Texture2D)rawImage.texture;

        // Get the normalized coordinates of the cropping area
        Rect normalizedCroppingRect = GetNormalizedCroppingRect();

        // Calculate the pixel coordinates based on the normalized cropping area
        int x = Mathf.FloorToInt(normalizedCroppingRect.x * originalTexture.width);
        int y = Mathf.FloorToInt(normalizedCroppingRect.y * originalTexture.height);
        int width = Mathf.FloorToInt(normalizedCroppingRect.width * originalTexture.width);
        int height = Mathf.FloorToInt(normalizedCroppingRect.height * originalTexture.height);

        // Create a new texture to store the cropped image
        Texture2D croppedTexture = new Texture2D(width, height);

        // Copy the pixels from the original texture to the cropped texture
        croppedTexture.SetPixels(originalTexture.GetPixels(x, y, width, height));
        croppedTexture.Apply();

        // Display the cropped texture
        rawImage.texture = croppedTexture;
    }

    // Helper function to get the normalized coordinates of the cropping area
    private Rect GetNormalizedCroppingRect()
    {
        Rect rect = new Rect(
            croppingArea.anchoredPosition.x / rawImage.rectTransform.rect.width,
            croppingArea.anchoredPosition.y / rawImage.rectTransform.rect.height,
            croppingArea.rect.width / rawImage.rectTransform.rect.width,
            croppingArea.rect.height / rawImage.rectTransform.rect.height
        );

        // Clamp values to ensure they are in the [0, 1] range
        rect.x = Mathf.Clamp01(rect.x);
        rect.y = Mathf.Clamp01(rect.y);
        rect.width = Mathf.Clamp01(rect.width);
        rect.height = Mathf.Clamp01(rect.height);

        return rect;
    }
}
