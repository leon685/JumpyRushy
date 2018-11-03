using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject platform;
    private GameObject[] aPlatforms;
    private GameObject[] aCoins;

    public int x;
    public int y;

    public GameObject coin;
    public int numberOfPlatforms;
    public int numberOfCoins;
    public float xMin,xMax,yMin;

    private float platformWidth;
    private GameObject newObject;
    private GameObject tmp;
    // Use this for initialization
    void Start()
    {
        generatePlatforms();
        generateCoins();
    }

    void generatePlatforms()
    {
        aPlatforms = new GameObject[numberOfPlatforms];
        aCoins = new GameObject[numberOfCoins];
        tmp = platform;
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            float x2 = Random.Range(xMin, xMax);
            float y2 = Random.Range(platform.transform.position.y, tmp.transform.position.y + 1.5f);
            tmp.transform.localScale = new Vector3(Random.Range(0.3f, 2f), tmp.transform.localScale.y, tmp.transform.localScale.z);
            newObject = Instantiate(tmp, new Vector3(tmp.transform.position.x + x2, y2), tmp.transform.rotation);
            aPlatforms[i] = tmp;
            tmp = newObject;
        }
        generateCoins();
    }

    void generateCoins()
    {
        GameObject p;
        GameObject newCoin;

        for (int i=0; i< numberOfCoins; i++)
        {
            int rndPlatform = Random.Range(0, (numberOfPlatforms - 1));
            p = aPlatforms[rndPlatform];
            newCoin=Instantiate(coin, new Vector3(p.transform.position.x, Random.Range(p.transform.position.y+1f, p.transform.position.y+3.5f)), coin.transform.rotation);
            aCoins[i] = newCoin;
        }
    }

	// Update is called once per frame
	void Update () {
       
    }
}
