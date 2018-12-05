using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerator : MonoBehaviour
{

    public GameObject c;
    public GameObject s;
    public GameObject f;
    public GameObject p;

    public int nP;
    public int nC;
    public int nS;
    public float xMi, xMa;

    private float platformWidth;
    
    // Use this for initialization
    public class pop_size
    {
        public GameObject[] aPlatforms;
        public GameObject[] aCoins;
        public GameObject[] aSpikes;

        public GameObject coin;
        public GameObject spike;
        public GameObject finish;
        public GameObject platform;

        public int numberOfPlatforms;
        public int numberOfCoins;
        public int numberOfSpikes;
        public float xMin, xMax;


        public pop_size(GameObject p, GameObject c, GameObject s, GameObject f, int np,int nc,int ns, float xmi,float xma)
        {
            platform = p;
            coin = c;
            spike = s;
            finish = f;
            numberOfPlatforms = np;
            numberOfCoins = nc;
            numberOfSpikes = ns;
            xMin = xmi;
            xMax = xma;
        }

        public void start()
        {
            generatePlatforms();
            generateCoins();
            generateSpikes();
            fix();
        }


        private void generatePlatforms()
        {
            aPlatforms = new GameObject[numberOfPlatforms];
            aCoins = new GameObject[numberOfCoins];
            GameObject newObject;
            GameObject tmp;
            tmp = platform;
            for (int i = 0; i < numberOfPlatforms; i++)
            {
                float x2 = Random.Range(xMin, xMax);
                Debug.Log("Platform " + platform);
                float y2 = Random.Range(platform.transform.position.y, tmp.transform.position.y + 1.5f);
                tmp.transform.localScale = new Vector3(Random.Range(1f, 2f), tmp.transform.localScale.y, tmp.transform.localScale.z);

                //newObject = Instantiate(tmp, new Vector3(tmp.transform.position.x + x2, y2), tmp.transform.rotation);
                aPlatforms[i] = tmp;
                //tmp = newObject;
            }
        }
        private void generateSpikes()
        {
            aSpikes = new GameObject[numberOfSpikes];
            GameObject p;
            GameObject newSpike;
            newSpike = spike;
            for (int i = 0; i < numberOfSpikes; i++)
            {
                int rndPlatform = Random.Range(1, (numberOfPlatforms - 1));
                p = aPlatforms[rndPlatform];
                //newSpike = Instantiate(spike, new Vector3(Random.Range(p.transform.position.x - 1, p.transform.position.x + (p.transform.localScale.x)), p.transform.position.y + 0.5f), coin.transform.rotation);
                newSpike.transform.position = new Vector3(Random.Range(p.transform.position.x - 1, p.transform.position.x + (p.transform.localScale.x)), p.transform.position.y + 0.5f);
                aSpikes[i] = newSpike;
            }
        }
        private void generateCoins()
        {
            aCoins = new GameObject[numberOfCoins];
            GameObject p;
            GameObject newCoin;
            newCoin = coin;

            for (int i = 0; i < numberOfCoins; i++)
            {
                int rndPlatform = Random.Range(1, (numberOfPlatforms - 1));
                p = aPlatforms[rndPlatform];
                //newCoin = Instantiate(coin, new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3.5f)), coin.transform.rotation);
                newCoin.transform.position = new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3.5f));
                aCoins[i] = newCoin;
            }
        }

        private void fix()
        {
            GameObject[] poljeCekinov = aCoins;
            GameObject[] poljePlatform = aPlatforms;
            GameObject[] poljePasti = aSpikes;

            for (int i = 0; i < aCoins.Length; i++)
            {
                for (int j = 0; j < aCoins.Length; j++)
                {
                    if (i != j && aCoins[i].transform.position.x == aCoins[j].transform.position.x)
                    {
                        //Debug.Log("true" +i+ " " + j +" coords "+ poljeCekinov[i].transform.position.x + " od j "+poljeCekinov[j].transform.position.x);
                        Destroy(aCoins[j]);
                    }
                }
            }
            for (int i = 0; i < aSpikes.Length; i++)
            {
                for (int j = 0; j < aSpikes.Length; j++)
                {
                    //Debug.Log("abs " + (poljePasti[i].transform.position.x - poljePasti[j].transform.position.x));
                    if (i != j && aSpikes[i].transform.position.x == aSpikes[j].transform.position.x || (aSpikes[i].transform.position.x - aSpikes[j].transform.position.x) < -0.6f && (aSpikes[i].transform.position.x - aSpikes[j].transform.position.x) > -3f)
                    {
                        Destroy(aSpikes[j]);
                    }
                }
            }




            GameObject tmp2 = new GameObject();
            for (int i = 1; i < aPlatforms.Length; i++)
            {
                tmp2 = aPlatforms[i];
                if (tmp2.transform.position.x < aPlatforms[i].transform.position.x)
                {
                    tmp2 = aPlatforms[i];
                }
            }

            //Instantiate(finish, new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 0.8f), tmp2.transform.rotation);
            finish.transform.position = new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 1.5f);
        }
    }

    void Start()
    {
        Debug.Log("P " + p);
        pop_size levels= new pop_size(p,c,s,f,nP,nC,nS,xMi,xMa);
        levels.start();

        Debug.Log("vel "+levels.aPlatforms.Length);
        for (int i=0; i<levels.aPlatforms.Length; i++)
        {
            Debug.Log("obj " + levels.aPlatforms[i]);
            Instantiate(levels.aPlatforms[i]);
        }

    }


    // Update is called once per frame
    void Update()
    {

    }
}
