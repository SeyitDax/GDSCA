using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    string registerSceneName;

    [SerializeField]
    string profileSceneName;

    [SerializeField]
    string mainSceneName;
    private void Awake()
    {
        if(PlayerPrefs.GetInt("Sýgned_In", 0) == 1)
        {
            SwitchToProfileScene();
        }
    }
    public void SwitchToRegisterScene()
    {
        SceneManager.LoadScene(registerSceneName);
    }

    public void SwitchToProfileScene()
    {
        SceneManager.LoadScene(profileSceneName);
    }

    public void SwithcToMainMenu()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
