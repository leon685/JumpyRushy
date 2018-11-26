using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject platform;
    private GameObject[] aPlatforms;
    private GameObject[] aCoins;

    public GameObject coin;
    public GameObject spike;
    public int numberOfPlatforms;
    public int numberOfCoins;
    public int numberOfSpikes;
    public float xMin,xMax;

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
            tmp.transform.localScale = new Vector3(Random.Range(0.5f, 2f), tmp.transform.localScale.y, tmp.transform.localScale.z);
            newObject = Instantiate(tmp, new Vector3(tmp.transform.position.x + x2, y2), tmp.transform.rotation);
            aPlatforms[i] = tmp;
            tmp = newObject;
        }
        generateSpikes();
        generateCoins();
    }

    void generateSpikes()
    {
        GameObject p;
        GameObject newSpike;

        for (int i = 0; i < numberOfSpikes; i++)
        {
            int rndPlatform = Random.Range(1, (numberOfPlatforms - 1));
            p = aPlatforms[rndPlatform];
            newSpike = Instantiate(spike, new Vector3(Random.Range(p.transform.position.x-1, p.transform.position.x+(p.transform.localScale.x)), p.transform.position.y+0.5f), coin.transform.rotation);
        }
    }

    void generateCoins()
    {
        GameObject p;
        GameObject newCoin;

        for (int i=0; i< numberOfCoins; i++)
        {
            int rndPlatform = Random.Range(1, (numberOfPlatforms - 1));
            p = aPlatforms[rndPlatform];
            newCoin=Instantiate(coin, new Vector3(p.transform.position.x, Random.Range(p.transform.position.y+1f, p.transform.position.y+3.5f)), coin.transform.rotation);
            aCoins[i] = newCoin;
        }
        fix();
    }

    private void fix()
    {
        GameObject[] poljeCekinov=GameObject.FindGameObjectsWithTag("coinTag");  //returns GameObject[]
        GameObject[] poljePlatform=GameObject.FindGameObjectsWithTag("platformTag");  //returns GameObject[]
        GameObject finish=GameObject.FindGameObjectWithTag("finishTag");
        for (int i=0; i < poljeCekinov.Length; i++)
        {
            for (int j=0; j < poljeCekinov.Length; j++)
            {
                if (i!=j && poljeCekinov[i].transform.position.x == poljeCekinov[j].transform.position.x)
                {
                    //Debug.Log("true" +i+ " " + j +" coords "+ poljeCekinov[i].transform.position.x + " od j "+poljeCekinov[j].transform.position.x);
                    Destroy(poljeCekinov[j]);
                }
            }
        }
        GameObject tmp2 = new GameObject();
        for (int i=1; i<poljePlatform.Length; i++)
        {
            tmp2 = poljePlatform[i];
            if (tmp2.transform.position.x < poljePlatform[i].transform.position.x)
            {
                tmp2 = poljePlatform[i];
            }
        }

        //Instantiate(finish, new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 0.8f), tmp2.transform.rotation);
        finish.transform.position = new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 1.5f);
    }

    // Update is called once per frame
    void Update () {
       
    }
}
