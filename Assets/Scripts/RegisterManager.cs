using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class RegisterManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField userName;

    [SerializeField]
    TMP_InputField phoneNumber;

   // [SerializeField]
   // TMP_InputField email;



    public void CompleteRegistery()
    {
        string correctionPhoneNum = phoneNumber.text;
        string correctedPhoneNum = new string(correctionPhoneNum.Where(c => !char.IsWhiteSpace(c)).ToArray());

        AppData.Person newPerson = new AppData.Person
        {
            name = userName.text,
            phoneNumber = correctedPhoneNum,
        };

        if(!HasPhoneNumber(newPerson)) 
        {
            if (userName != null && AppData.instance.plainNumbers.Contains(correctedPhoneNum))
            {
                if (IsAppropriate(userName.text))
                {
                    // Debug.Log("Registy is Completed");

                    AppData.instance.AddPerson(userName.text, correctedPhoneNum, true);
                }
                else
                {
                    Notification.instance.InstantiateNotification("Uygunsuz kelime kullanýmý");
                    // Debug.Log("Unappropiate UserName");
                }

            }
            else
            {
               // Debug.Log("Phone Number does not exist");
                Notification.instance.InstantiateNotification("Phone Number does not exist",true);
            }
        }
        else
        {
            Notification.instance.InstantiateNotification("Bu kiþi zaten kayýtlý", true);

           // Debug.Log("This Person has already registered");
        }
    }

    private bool IsAppropriate(string text)
    {
        string[] inappropriateWords = { "Am", "Göt", "Sik", "Yarrak", "Ebe", "@m", "s!k", "Y@rrak", "yarak", "orsupu", "faiþe", "Gerizekalý", "Salak", "Mal", "Beyinsiz", "Pislik", "Pezevenk", "piç", "ibine", "sürtük", "pezeveng", "Bok", "amcik", "Fuck", "Nigga", "GötSiktiren","Amyalayýcý" };

        // Create a regex pattern with word boundaries
        string pattern = @"\b(" + string.Join("|", inappropriateWords) + @")\b";

        // Check if the text contains any inappropriate words
        if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
        {
            Debug.Log("Inappropriate word detected!");

            return false;
        }
        else
        {
            Debug.Log("Text is clean.");

            return true;
        }
    }

    bool HasPhoneNumber(AppData.Person person)
    {
        foreach (AppData.Person registry in AppData.instance.phoneBook)
        {
            if (registry.phoneNumber.Contains(person.phoneNumber))
            {
                return true;
            }
        }
        return false;
    }


    // Email Verification System

    // public void CompleteRegister()
    // {
    //     if(phoneNumber != null && AppData.instance.plainNumbers.Contains(phoneNumber.text))
    //     {
    //         if(email != null)
    //         {
    //             //Send Email
    //         }
    //         else
    //         {
    //             Debug.Log("Email is empty or wrong");
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("Phone Number is not found");
    //     }
    // }

    // private bool IsEmailValid(string email)
    // {
    //     string validFormation = @"^\S+@\S+\.\S+$";
    //
    //     Regex regex = new Regex(validFormation);
    //
    //     if(regex.IsMatch(email))
    //     {
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }
}
