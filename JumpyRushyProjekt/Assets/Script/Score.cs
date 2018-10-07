using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static int score;
    Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
        score = 0;
    }
    void Update()
    {
        if (score < 0)
        {
            score = 0;
        }
        text.text = "" + score;
    }

    public static void DodajTocke(int p)
    {
        score = score + p;
    }
    public static void Reset()
    {
        score = 0;
    }
}
