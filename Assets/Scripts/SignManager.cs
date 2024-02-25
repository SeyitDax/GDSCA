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

    public void SýngIn()
    {
        if(AppData.instance.CheckPerson(userName.text, phoneNumber.text))
        {
            PlayerPrefs.SetInt("Sýgned_In", 1);
            PlayerPrefs.SetString("userName", userName.text);
            
            sceneSwitcher.SwitchToProfileScene();
        }
    }
}
