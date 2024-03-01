using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class AppData : MonoBehaviour
{
    public static AppData instance;

    public List<Person> phoneBook;
    public List<string> plainNumbers;

    [Serializable]
    public class SaveData
    {
        [SerializeField]
        public List<string> userNames;
        [SerializeField]
        public List<string> phoneNumbers;

        [SerializeField]
        public List<string> plainNumbers;
    }

    [Serializable]
    public class Person : IEquatable<Person>
    {
        public string name { get; set; }
        public string phoneNumber { get; set; }

        public bool Equals(Person other)
        {
            return other != null
                && other.name == name
                && other.phoneNumber == phoneNumber;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (name != null ? name.GetHashCode() : 0);
                hash = hash * 23 + (phoneNumber != null ? phoneNumber.GetHashCode() : 0);
                return hash;
            }
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

        LoadData();
    }

    private void Start()
    {
        // AddPerson("Seyit Ahmet", "5511264537", true, true);

        string[] personToAdd = new string[] { "5523288342", "5469464663", "544 571 97 62", "505 053 67 10", "543 176 09 20", "546 530 07 40", "506 503 87 60", "539 773 24 63", "554 196 50 87", "551 983 92 35", "" };

        AddPerson("", "551 126 45 37");
        AddPerson("", "5511264522");
    }

    private void OnApplicationQuit()
    {
        SaveListData();
    }

    private void OnApplicationPause()
    {
        SaveListData();
    }

    public void AddPerson(string name, string phoneNumber, bool register = false, bool firstAdd = false)
    {

        string correctedPhoneNum = new string(phoneNumber.Where(c => !char.IsWhiteSpace(c)).ToArray());

        if (register)
        {
            // Check if the phone number already exists in any Person in the phoneBook
            if (instance.phoneBook.Any(person => person.phoneNumber == correctedPhoneNum))
            {
                if(!firstAdd)Notification.instance.InstantiateNotification("Bu kiþi zaten kayýtlý", true);
                Debug.Log("Person with the same phone number already exists in the phone book.");
            }
            else
            {
                Person newPerson = new Person { phoneNumber = correctedPhoneNum, name = name };
            
                instance.phoneBook.Add(newPerson);

                if (!firstAdd)Notification.instance.InstantiateNotification("Kayýt Baþarýlý",false);
                Debug.Log("New Person is Added");
            }
        }

        else
        {
            // Check if the phone number is not already in plainNumbers
            if (!instance.plainNumbers.Contains(correctedPhoneNum))
            {
                instance.plainNumbers.Add(correctedPhoneNum);
            }
            else
            {
                Debug.Log("Phone number already exists in plainNumbers.");
            }
        }
    }

    private void SaveListData()
    {
        SaveData saveData = new SaveData
        {
            userNames = ReturnUsernames(),
            phoneNumbers = ReturnPhoneNumbers(),

            plainNumbers = instance.plainNumbers
        };

        string jsonData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("saveData", jsonData);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        string jsonData = PlayerPrefs.GetString("saveData", "");

        if (!string.IsNullOrEmpty(jsonData))
        {
           // Debug.Log("Loaded data: " + jsonData);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            if (saveData != null)
            {
                instance.phoneBook = CreatePerson(saveData.userNames, saveData.phoneNumbers);
                instance.plainNumbers = saveData.plainNumbers;

                Debug.Log("PhoneBook is loaded with " + instance.phoneBook.Count + " entries.");
                Debug.Log("PlainNumbers is loaded with " + instance.plainNumbers.Count + " entries.");
            }
        }
        else
        {
            Debug.Log("No saved data found.");
        }
    }

    public bool CheckPerson(string userName, string phoneNumber)
    {
        Person assumedPerson = new Person() { name = userName, phoneNumber = new string(phoneNumber.Where(c => !char.IsWhiteSpace(c)).ToArray())
    };

        if (instance.phoneBook.Contains(assumedPerson))
        {
            Debug.Log("Logged In Successfully");
            Notification.instance.InstantiateNotification("Giriþ Baþarýlý");
            return true;
        }
        else
        {
            Debug.Log("Invalid Credentials");
            Notification.instance.InstantiateNotification("Geçersiz kullanýcý bilgileri",true);
            return false;
        }
    }

    List<string> ReturnUsernames()
    {
        List<string> ret = new List<string>();
        foreach(Person person in instance.phoneBook)
        {
            ret.Add(person.name);
        }
        return ret;
    }

    List<string> ReturnPhoneNumbers()
    {
        List<string> ret = new List<string>();
        foreach (Person person in instance.phoneBook)
        {
            ret.Add(person.phoneNumber);
        }
        return ret;
    }

    List<Person> CreatePerson(List<string> userNames, List<string> phoneNUmbers)
    {
        List<Person> ret = new List<Person>();
        for (int i = 0; i < userNames.Count; i++) 
        {
           // Debug.Log("Initilazing Person");

            Person j = new Person() { name = userNames[i], phoneNumber = phoneNUmbers[i] };
            ret.Add(j);

           // Debug.Log("Initilazed a new Person Successfully");
        }
        return ret;
    }
    /*
    public void ShowNotification(string message, bool warning = false, float duration = 200f)
    {
        GameObject notificationBox = GameObject.Find("NotificationBox");

        if (notificationBox != null)
        {
            if (warning)
            {
                Transform warningBox = notificationBox.transform.Find("WarningBox");

                if(warningBox != null )
                {
                    warningBox.GetComponentInChildren<TextMeshProUGUI>().text = message;

                    StartCoroutine(MoveNotificationBox(notificationBox.GetComponent<Transform>(), 200f));
                }
                else
                {
                    Debug.LogWarning("WarningBox is not found!");    
                }
            }
        }
        else
        {
            Debug.LogWarning("Notification Box Couldn't be found!");
        }
    }

    IEnumerator MoveNotificationBox(Transform notificationBox, float distance)
    {
        Image boxImage= notificationBox.transform.Find("WarningBox").GetComponent<Image>();
        RawImage iconImage = notificationBox.transform.Find("RawImage").GetComponent<RawImage>();
        TextMeshProUGUI ntfText = notificationBox.transform.Find("WarningBox").GetComponentInChildren<TextMeshProUGUI>();

        Color baseColor = boxImage.color;

        baseColor.a = 255f;

        boxImage.color = baseColor; iconImage.color = baseColor; ntfText.color = baseColor;

        float yAxis = notificationBox.position.y;
        float desiredY = yAxis + distance;
        float desiredTime = 0.5f;
        float startTime = Time.time;

        while (Time.time - startTime <= desiredTime)
        {
            float t = (Time.time - startTime) / desiredTime;
            notificationBox.position = new Vector3(notificationBox.position.x, Mathf.Lerp(yAxis, desiredY, t), notificationBox.position.z);
            yield return null;
        }

        notificationBox.position = new Vector3(notificationBox.position.x, desiredY, notificationBox.position.z);

        yield return new WaitForSeconds(0.5f);


        if (iconImage != null && boxImage != null && ntfText != null)
        {
            float duration = 2f;
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

    */
}
