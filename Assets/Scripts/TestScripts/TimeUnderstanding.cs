using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUnderstanding : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timeTime;

    [SerializeField]
    TextMeshProUGUI startTimeBox;

    [SerializeField]
    TextMeshProUGUI differnceBox;

    bool triggered;

    float startTime;

    private void Update()
    {
        timeTime.text = Time.time.ToString();

        if (Input.GetButtonDown("Jump"))
        {
            triggered = true;
            startTime = Time.time; // Record the start time only when the "Jump" button is pressed
            startTimeBox.text = startTime.ToString();
        }

        if (triggered)
        {
            float difference = Time.time - startTime;
            differnceBox.text = difference.ToString();
        }
    }
}
