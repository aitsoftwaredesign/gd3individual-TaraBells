using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public static float currentTime = 0f;
    public static float startTime = 15f;
    public Text countdownText;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if(currentTime <= 0)
        {
            currentTime = 0;
            if (KeepScore.Score <= 10)
            {
                SceneManager.LoadScene("Level1");
            }
            else if(KeepScore.Score >= 15)
            {
                SceneManager.LoadScene("Level2");
            }
        }
        
    }
}
