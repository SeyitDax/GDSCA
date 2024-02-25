using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField userName;

    [SerializeField]
    TMP_InputField phoneNumber;

    [SerializeField]
    SceneSwitcher sceneSwitcher;

    public void S�ngIn()
    {
        if(AppData.instance.CheckPerson(userName.text, phoneNumber.text))
        {
            PlayerPrefs.SetInt("S�gned_In", 1);
            PlayerPrefs.SetString("userName", userName.text);
            
            sceneSwitcher.SwitchToProfileScene();
        }
    }
}
