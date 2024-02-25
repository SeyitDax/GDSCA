using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUpdate : MonoBehaviour
{
    TextMeshProUGUI userNameText;

    private void Awake()
    {
        userNameText = GetComponent<TextMeshProUGUI>();

        string userName = PlayerPrefs.GetString("userName", "");

        if (userName != null)
        {
            Debug.Log(userName + "   " + userNameText);
           userNameText.text = userName;
        }
        else
        {
            Debug.Log("A problem occured when trying to update the username");
        }
    }
}
