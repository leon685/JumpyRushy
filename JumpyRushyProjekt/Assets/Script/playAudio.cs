﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour {
	// Use this for initialization
    public AudioSource a;
	void Start () {
        if (Zvok.backgroundMusic != null)
        {
            //AudioSource.PlayClipAtPoint(Zvok.backgroundMusic, new Vector3(0, 0, 0), 1.0f);
            a.clip = Zvok.backgroundMusic;
            a.loop = false;
            a.Play();
        }
        else
        {
            a.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (a.time > 33)
        {
            a.Stop();
            a.Play();
        }
	}
}
