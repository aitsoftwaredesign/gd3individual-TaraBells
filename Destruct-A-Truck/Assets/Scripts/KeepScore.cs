using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepScore : MonoBehaviour
{
    public static int Score = 0;
    // Start is called before the first frame update
  

    //create a GUI box and have it show the text and the int score
    void OnGUI()
    {
        GUI.Box(new Rect(50, 50, 200, 50), "Score is: "  + Score);
    }

}
