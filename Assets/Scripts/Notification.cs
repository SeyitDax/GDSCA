using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.ComponentModel;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Notification : MonoBehaviour
{
    public static Notification instance;

    GameObject notificationPrefab; // = Resources.Load<GameObject>("Prefabs/NotificationBox"); // Adjust path as needed

    Sprite boxSpriteWarning;

    Sprite boxSpriteConfirmation;

    [SerializeField] private AssetReferenceGameObject notificationPrefabRef;
    [SerializeField] private AssetReference confirmationBoxRef;
    [SerializeField] private AssetReference warningBoxRef;

    private void Start()
    {
        Addressables.LoadAssetAsync<GameObject>(notificationPrefabRef).Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                notificationPrefab = asyncOperationHandle.Result;
            }
            else
            {
                Debug.LogError("Notification Load Failed!");
            }
        }; 
        Addressables.LoadAssetAsync<Sprite> (confirmationBoxRef).Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                boxSpriteConfirmation = asyncOperationHandle.Result;
            }
            else
            {
                Debug.LogError("Confirmation Box Load Failed!");
            }
        }; 
        Addressables.LoadAssetAsync<Sprite>(warningBoxRef).Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                boxSpriteWarning = asyncOperationHandle.Result;
            }
            else
            {
                Debug.LogError("Warning Box Load Failed!");
            }
        };
    }
    public void InstantiateNotification(string message, bool warning = false)
    {
        GameObject notificationObject = Instantiate(notificationPrefab, transform);

        // Set message text
        TextMeshProUGUI messageText = notificationObject.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
        if (messageText != null)
        {
            if(warning)
            {
                messageText.color = Color.white;
            }
            else
            {
                messageText.color = Color.black;
            }

            messageText.text = message;
        }

        // Set warning visuals if necessary

        if (notificationObject != null)
        {
            Transform warningBox = notificationObject.transform.Find("WarningBox");

            SetSizeBasedOnScene(notificationObject);

            if (warningBox != null)
            {
                Image boxImage = warningBox.GetComponent<Image>();

                RawImage warningImage = notificationObject.transform.Find("WarningImage").GetComponent<RawImage>();
                RawImage confirmationImage = notificationObject.transform.Find("ConfirmationImage").GetComponent<RawImage>();

                if (warningImage != null && confirmationImage != null)
                {
                    if (warning)
                    {
                        Debug.Log("This is a warning");

                        boxImage.sprite = boxSpriteWarning; //Resources.Load<Sprite>("AppAssets/Warning_Box");

                        warningImage.enabled = true;
                        confirmationImage.enabled = false;
                    }
                    else
                    {
                        Debug.Log("Confirmation is true");

                        boxImage.sprite = boxSpriteConfirmation; //Resources.Load<Sprite>("AppAssets/Confirmation_Box");

                        warningImage.enabled = false;
                        confirmationImage.enabled = true;
                    }

                    ShowNotification(notificationObject);
                }
                else
                {
                    Debug.LogWarning("Images insidde the NotificationObject is not found.");
                }

            }
            else
            {
                Debug.LogWarning("NotificationBox is not found.");
            }
            
        }
        else
        {
            Debug.LogWarning("NotificationObject is not found.");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    public void ShowNotification(GameObject notificationBox)
    {
        if (notificationBox != null)
        {
            StartCoroutine(MoveNotificationBox(notificationBox.GetComponent<Transform>(), 200f));
        }
        else
        {
            Debug.LogWarning("NotificationBox is not found");
        }
    }

    IEnumerator MoveNotificationBox(Transform notificationBox, float distance)
    {
        Image boxImage = notificationBox.transform.Find("WarningBox").GetComponent<Image>();
        RawImage warningImage = notificationBox.transform.Find("WarningImage").GetComponent<RawImage>();
        RawImage confirmationImage = notificationBox.transform.Find("ConfirmationImage").GetComponent<RawImage>();
        TextMeshProUGUI ntfText = notificationBox.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();

        Color boxBaseColor = boxImage.color;
        boxBaseColor.a = 255f;
        boxImage.color = boxBaseColor;

        Color warImgCol = warningImage.color;
        warImgCol.a = 255f;
        warningImage.color = warImgCol;

        Color confImgColor = confirmationImage.color;
        confImgColor.a = 255f;
        confirmationImage.color = confImgColor;

        Color textColor = ntfText.color;
        textColor.a = 255f;
        ntfText.color = textColor;

        float yAxis = notificationBox.position.y;
        float desiredY = yAxis + distance;
        float desiredTime = 0.25f;
        float startTime = Time.time;

        while (Time.time - startTime <= desiredTime)
        {
            float t = (Time.time - startTime) / desiredTime;
            notificationBox.position = new Vector3(notificationBox.position.x, Mathf.Lerp(yAxis, desiredY, t), notificationBox.position.z);
            yield return null;
        }

        notificationBox.position = new Vector3(notificationBox.position.x, desiredY, notificationBox.position.z);

        yield return new WaitForSeconds(0.65f);


        if (warningImage != null && boxImage != null && ntfText != null && confirmationImage)
        {
            float duration = 2.3f;
            float elapsedTime = Time.time;

            while (Time.time - elapsedTime <= duration)
            {
                float t = (Time.time - elapsedTime) / duration;

                Color boxColor = boxImage.color;
                Color warningColor = warningImage.color;
                Color confColor = confirmationImage.color;
                Color ntfColor = ntfText.color;

                boxColor.a = Mathf.Lerp(1f, 0, t);
                warningColor.a = Mathf.Lerp(1f, 0, t);
                confColor.a = Mathf.Lerp(1f, 0, t);
                ntfColor.a = Mathf.Lerp(1f, 0, t);


                boxImage.color = boxColor;
                warningImage.color = warningColor;
                confirmationImage.color = confColor;
                ntfText.color = ntfColor;

                yield return null;
            }

            notificationBox.position = new Vector3(notificationBox.position.x, yAxis, notificationBox.position.z);
        }
        else
        {
            Debug.LogWarning("Color Component cannot be found");
        }
    }


    private void SetSizeBasedOnScene(GameObject notificationBox)
    {
        if (SceneManager.GetActiveScene().name == "MainScene") notificationBox.transform.localScale = notificationBox.transform.localScale/1.25f;
    }
}
