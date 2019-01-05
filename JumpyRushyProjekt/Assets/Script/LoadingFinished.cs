using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingFinished : MonoBehaviour {
    bool load = false;
	// Use this for initialization
	void Start () {
        Thread nit = new Thread(new ThreadStart(check));
        nit.Start();
    }
    void check()
    {
        while (!CompressAll.loadingDone)
        {

        }
        load = true;
    }
    // Update is called once per frame
    void Update () {
		if (load)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
