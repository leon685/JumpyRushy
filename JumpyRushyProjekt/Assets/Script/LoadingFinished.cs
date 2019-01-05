using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingFinished : MonoBehaviour {
	// Use this for initialization
	void Start () {
        
    }
    // Update is called once per frame
    void Update () {
        //Thread nit = new Thread(new ThreadStart(check));
        //nit.Start();

        if (CompressAll.loadingDone && CompressEffect1.loadingDoneEffect1 && CompressEffect2.loadingDoneEffect2)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
