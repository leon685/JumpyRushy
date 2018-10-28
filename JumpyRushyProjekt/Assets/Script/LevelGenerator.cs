using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject platform;
    public float numberOfPlatforms;
    public float xMin,xMax,yMin;

    private float platformWidth;
    private GameObject newObject;
    private GameObject tmp;
    // Use this for initialization
    void Start()
    {
        tmp = platform;
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, tmp.transform.position.y+1.5f);
            newObject=Instantiate(tmp, new Vector3(tmp.transform.position.x+x, y), transform.rotation);
            tmp = newObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
       
    }
}
