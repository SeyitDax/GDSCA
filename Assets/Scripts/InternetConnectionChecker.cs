using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InternetConnectionChecker : MonoBehaviour
{
    public static InternetConnectionChecker Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    public void Register()
    {
        StartCoroutine(CheckInternetConnection(true));
    }

    public void SendVerification()
    {
        StartCoroutine(CheckInternetConnection());
    }

    private IEnumerator CheckInternetConnection(bool register = false)
    {
        UnityWebRequest www = new UnityWebRequest("http://www.google.com");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("No internet connection");
        }
        else if(register)
        {
            Debug.Log("Internet connection is available");
        }
        else
        {
            Debug.Log("Internet connection is available");
        }
    }
}
