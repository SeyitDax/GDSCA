using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification instance;

    public void InstantiateNotification(string message, bool warning = false)
    {
        GameObject notificationPrefab = Resources.Load<GameObject>("Prefabs/NotificationBox"); // Adjust path as needed
        GameObject notificationObject = Instantiate(notificationPrefab, transform);

        // Set message text
        TextMeshProUGUI messageText = notificationObject.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
        if (messageText != null)
        {
            messageText.text = message;
        }

        // Set warning visuals if necessary
        if (warning)
        {
            Image warningImage = notificationObject.transform.Find("WarningImage").GetComponent<Image>();
            if (warningImage != null)
            {
                warningImage.gameObject.SetActive(true);
            }
        }

        StartCoroutine(MoveNotificationBox(notificationObject.transform,200f));
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
    public void ShowNotification(string message, bool warning = false)
    {
        GameObject notificationBox = GameObject.Find("NotificationBox");

        if (notificationBox != null)
        {
            Transform warningBox = notificationBox.transform.Find("WarningBox");

            if (warningBox != null)
            {
                TextMeshProUGUI notificationMessage = warningBox.GetComponentInChildren<TextMeshProUGUI>();

                if (notificationBox != null)
                {
                    notificationMessage.text = message;

                    Image boxImage = warningBox.GetComponent<Image>();

                    RawImage warningImage = notificationBox.transform.Find("WarningImage").GetComponent<RawImage>();
                    RawImage confirmationImage = notificationBox.transform.Find("ConfirmationImage").GetComponent<RawImage>();

                    if (warning)
                    {
                        boxImage.sprite = Resources.Load<Sprite>("AppAssets/Warning_Box");

                        warningImage.enabled = true;
                        confirmationImage.enabled = false;
                    }
                    else
                    {
                        boxImage.sprite = Resources.Load<Sprite>("AppAssets/Confirmation_Box");

                        warningImage.enabled = false;
                        confirmationImage.enabled = true;
                    }

                    StartCoroutine(MoveNotificationBox(notificationBox.GetComponent<Transform>(), 200f));
                }

                else
                {
                    Debug.LogWarning("Notification text is not found");
                }
            }
            else
            {
                Debug.LogWarning("WarningBox is not found!");
            }
        }
        else
        {
            Debug.LogWarning("Notification Box Couldn't be found!");
        }
    }

    IEnumerator MoveNotificationBox(Transform notificationBox, float distance)
    {
        Image boxImage = notificationBox.transform.Find("WarningBox").GetComponent<Image>();
        RawImage iconImage = notificationBox.transform.Find("WarningImage").GetComponent<RawImage>();
        TextMeshProUGUI ntfText = notificationBox.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();

        Color baseColor = boxImage.color;

        baseColor.a = 255f;

        boxImage.color = baseColor; iconImage.color = baseColor; ntfText.color = baseColor;

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


        if (iconImage != null && boxImage != null && ntfText != null)
        {
            float duration = 2.3f;
            float elapsedTime = Time.time;

            while (Time.time - elapsedTime <= duration)
            {
                float t = (Time.time - elapsedTime) / duration;

                Color boxColor = boxImage.color;
                Color iconColor = iconImage.color;
                Color ntfColor = ntfText.color;

                boxColor.a = Mathf.Lerp(boxColor.a, 0, t);
                iconColor.a = Mathf.Lerp(boxColor.a, 0, t);
                ntfColor.a = Mathf.Lerp(boxColor.a, 0, t);


                boxImage.color = boxColor;
                iconImage.color = iconColor;
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
}
