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
        public List<GameObject> aPlatforms;
        public List<GameObject> aCoins;
        public List<GameObject> aSpikes;

        public GameObject coin;
        public GameObject spike;
        public GameObject finish;
        public GameObject platform;

        public int forFit_s;
        public int forFit_c;
        public int forFit_p;

        public int numberOfPlatforms;
        public int numberOfCoins;
        public int numberOfSpikes;
        public float xMin, xMax;
        public double fit;


        public pop_size(GameObject p, GameObject c, GameObject s, GameObject f, int np, int nc, int ns, float xmi, float xma)  //constructor
        {
            aPlatforms = new List<GameObject>();
            aSpikes = new List<GameObject>();
            aCoins = new List<GameObject>();
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

        public void startBuilding()
        {
            generatePlatforms();
            generateCoins();
            generateSpikes();
            fix();
            fitness();
        }


        private void generatePlatforms()
        {
            //aPlatforms = new GameObject[numberOfPlatforms];
            GameObject tmp;
            tmp = Instantiate(platform) as GameObject;
            tmp.active = false;
            for (int i = 0; i < numberOfPlatforms; i++)
            {
                float x2 = Random.Range(xMin, xMax);
                float y2 = Random.Range(platform.transform.position.y, tmp.transform.position.y + 1.5f);
                tmp.transform.localScale = new Vector3(Random.Range(1f, 2f), tmp.transform.localScale.y, tmp.transform.localScale.z);

                tmp.transform.position = new Vector3(tmp.transform.position.x + x2, y2);
                aPlatforms.Add(Instantiate(tmp) as GameObject);
                tmp.active = false;
                //tmp = newObject;
            }
            //generateCoins();
        }
        private void generateSpikes()
        {
            GameObject p;
            GameObject newSpike;
            newSpike = Instantiate(spike);
            newSpike.active = false;
            for (int i = 0; i < numberOfSpikes; i++)
            {
                int rndPlatform = Random.Range(1, (aPlatforms.Count - 1));
                p = aPlatforms[rndPlatform];
                //newSpike = Instantiate(spike, new Vector3(Random.Range(p.transform.position.x - 1, p.transform.position.x + (p.transform.localScale.x)), p.transform.position.y + 0.5f), coin.transform.rotation);
                newSpike.transform.position = new Vector3(Random.Range(p.transform.position.x - 1, p.transform.position.x + (p.transform.localScale.x)), p.transform.position.y + 0.5f);
                aSpikes.Add(Instantiate(newSpike));
                newSpike.active = false;
            }
            //fix();
        }
        private void generateCoins()
        {
            GameObject p;
            GameObject newCoin;
            newCoin = Instantiate(coin);
            newCoin.active = false;
            for (int i = 0; i < numberOfCoins; i++)
            {
                int rndPlatform = Random.Range(1, (aPlatforms.Count - 1));
                p = aPlatforms[rndPlatform];
                //newCoin = Instantiate(coin, new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3.5f)), coin.transform.rotation);
                newCoin.transform.position = new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3.5f));
                aCoins.Add(Instantiate(newCoin));
                newCoin.active = false;
            }
            //generateSpikes();
        }

        private void fix()
        {
            /*fix RemoveAt*/
            forFit_s = 0;
            forFit_p = 0;
            forFit_c = 0;
            for (int i = 0; i < aCoins.Count; i++)
            {
                for (int j = 0; j < aCoins.Count; j++)
                {
                    if (i != j && aCoins[i].transform.position.x == aCoins[j].transform.position.x)
                    {
                        Destroy(aCoins[j]);
                        //aCoins.RemoveAt(j);
                        forFit_c++;
                    }
                }
            }
            for (int i = 0; i < aSpikes.Count; i++)
            {
                for (int j = 0; j < aSpikes.Count; j++)
                {
                    if (i != j && aSpikes[i].transform.position.x == aSpikes[j].transform.position.x || (aSpikes[i].transform.position.x - aSpikes[j].transform.position.x) < -0.6f && (aSpikes[i].transform.position.x - aSpikes[j].transform.position.x) > -3f)
                    {
                        Destroy(aSpikes[j]);
                        //aSpikes.RemoveAt(j);
                        forFit_s++;
                    }
                }
            }
            GameObject tmp2=platform;
            for (int i = 0; i < aPlatforms.Count; i++)
            {
                if (i != 0)
                {
                    tmp2 = aPlatforms[i];
                    if (tmp2.transform.position.x < aPlatforms[i].transform.position.x)
                    {
                        tmp2 = aPlatforms[i];
                    }
                }
            }
            
            //Instantiate(finish, new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 0.8f), tmp2.transform.rotation);
            finish.transform.position = new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 1.5f);
            //fitness();
        }
        public void display()
        {
            for (int i = 0; i < aPlatforms.Count; i++)
            {
                //Instantiate(levels.aPlatforms[i]);
                aPlatforms[i].active = true;
            }
            for (int i = 0; i < aSpikes.Count; i++)
            {
                //Instantiate(levels.aPlatforms[i]);
                aSpikes[i].active = true;
            }
            for (int i = 0; i < aCoins.Count; i++)
            {
                //Instantiate(levels.aPlatforms[i]);
                aCoins[i].active = true;
            }
        }
        private void fitness()
        {
            int diff_c = aCoins.Count - forFit_c;
            int diff_p = aPlatforms.Count - forFit_p;
            int diff_s = aSpikes.Count - forFit_s;
            fit = (diff_c + diff_s + diff_p);
            Debug.Log(diff_c + " "+ diff_s + " "+ diff_p);
            fit = fit / 100;
        }
    }

    void Start()
    {
        pop_size levels= new pop_size(p,c,s,f,nP,nC,nS,xMi,xMa);
        levels.startBuilding();
        levels.display();
        Debug.Log("Fitness: "+levels.fit);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
