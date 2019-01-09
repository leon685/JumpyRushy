using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    public static int popaj = 0;
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
        public GameObject fin;

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
            fin = Instantiate(finish);
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
                tmp.transform.localScale = new Vector3(Random.Range(1.5f, 2f), tmp.transform.localScale.y, tmp.transform.localScale.z);

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
                int rndPlatform = Random.Range(1, aPlatforms.Count);
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
                int rndPlatform = Random.Range(1, aPlatforms.Count);
                p = aPlatforms[rndPlatform];
                //newCoin = Instantiate(coin, new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3.5f)), coin.transform.rotation);
                newCoin.transform.position = new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3f));
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
            List<GameObject> tmpC = new List<GameObject>();
            for (int i = aCoins.Count-1; i >= 0; i--)
            {
                for (int j = i; j >=0; j--)
                {
                    if (i != j && (aCoins[i].transform.position.x == aCoins[j].transform.position.x))
                    {
                        Destroy(aCoins[j]);
                        aCoins[j].name = "delete";
                        //aCoins.RemoveAt(j);
                        forFit_c++;
                    }
                }
               
            }
            for (int i = aSpikes.Count-1; i >= 0; i--)
            {
                for (int j = i; j >= 0; j--)
                {
                    if (i != j && aSpikes[i].transform.position.x == aSpikes[j].transform.position.x 
                        || (aSpikes[i].transform.position.x - aSpikes[j].transform.position.x) < -0.6f && (aSpikes[i].transform.position.x - aSpikes[j].transform.position.x) > -3f)
                    {
                        Destroy(aSpikes[j]);
                        aSpikes[j].name = "delete";
                        //aSpikes.RemoveAt(j);                      
                        forFit_s++;
                    }
                }
            }

            for (int i = aCoins.Count-1; i >= 0; i--)
            {
                if (aCoins[i].name == "delete")
                {
                    aCoins.RemoveAt(i);
                }
            }
            for (int i = aSpikes.Count - 1; i >= 0; i--)
            {
                if (aSpikes[i].name == "delete")
                {
                    aSpikes.RemoveAt(i);
                }
            }

            GameObject tmp2=aPlatforms[0];
            fin.active = false;
            for (int i = 0; i < aPlatforms.Count; i++)
            {
                
                    if (tmp2.transform.position.x < aPlatforms[i].transform.position.x)
                    {
                        tmp2 = aPlatforms[i];
                    }
            }
            
            fin.transform.position = new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 1.5f);
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
            fin.active = true;

        }
        public void fitness()
        {
            //int diff_c = numberOfCoins - forFit_c;
            //int diff_p = numberOfPlatforms - forFit_p;
            //int diff_s = numberOfSpikes - forFit_s;
            int diff_c = aCoins.Count;
            int diff_p = aPlatforms.Count;
            int diff_s = aSpikes.Count;
            fit = (diff_c + diff_s + diff_p);
            fit = fit / (numberOfCoins + numberOfPlatforms + numberOfSpikes);
        }
        public void mutacija()
        {
            //add coin
            GameObject p;
            GameObject newCoin;
            newCoin = Instantiate(coin);
            newCoin.active = false;

            int rndPlatform = Random.Range(1, aPlatforms.Count);
            p = aPlatforms[rndPlatform];

           newCoin.transform.position = new Vector3(p.transform.position.x, Random.Range(p.transform.position.y + 1f, p.transform.position.y + 3.5f));
           aCoins.Add(Instantiate(newCoin));
           newCoin.active = false;
        }

        public void fixKrizanje()
        {
           

            double platformDiff = Mathf.Abs(aPlatforms[(aPlatforms.Count / 2) - 1].transform.position.x - aPlatforms[(aPlatforms.Count / 2)].transform.position.x);

            //add platform if difference to big
            if (platformDiff > 8)
            {
                GameObject tmp = Instantiate(platform) as GameObject;
                tmp.active = false;
               
                tmp.transform.localScale = new Vector3(Random.Range(1f, 1.5f), tmp.transform.localScale.y, tmp.transform.localScale.z);

                float middleY = aPlatforms[(aPlatforms.Count / 2) - 1].transform.position.y + aPlatforms[(aPlatforms.Count / 2)].transform.position.y;
                middleY = middleY / 2.0f;
                float middleX = aPlatforms[(aPlatforms.Count / 2) - 1].transform.position.x + aPlatforms[(aPlatforms.Count / 2)].transform.position.x;
                middleX = middleX / 2.0f;


                tmp.transform.position = new Vector3(middleX, middleY-1f);
                aPlatforms.Add(Instantiate(tmp) as GameObject);
                tmp.active = false;
            }

            //double spikeX = aSpikes[aSpikes.Count / 2].transform.position.x;
            //double spikeY = aSpikes[aSpikes.Count / 2].transform.position.y;

            /*GameObject tmp2 = aPlatforms[0];
            fin.active = false;
            for (int i = 0; i < aPlatforms.Count; i++)
            {

                if (tmp2.transform.position.x < aPlatforms[i].transform.position.x)
                {
                    tmp2 = aPlatforms[i];
                }
            }

            fin.transform.position = new Vector3(tmp2.transform.position.x, tmp2.transform.position.y + 1.5f);
            */
            fix();
        }

        public void krizanje(pop_size k_with_me)
        {
            //drugo polovico od k_with_me zamenjaj z prvotnim levelom
            //aPlatform
            //aSpike
            //aCoin

            //uredi vse v polju po x osi
            aPlatforms = aPlatforms.OrderBy(x => x.transform.position.x).ToList();
            aCoins = aCoins.OrderBy(x => x.transform.position.x).ToList();
            aSpikes = aSpikes.OrderBy(x => x.transform.position.x).ToList();

            k_with_me.aPlatforms = k_with_me.aPlatforms.OrderBy(x => x.transform.position.x).ToList();
            k_with_me.aCoins = k_with_me.aCoins.OrderBy(x => x.transform.position.x).ToList();
            k_with_me.aSpikes = k_with_me.aSpikes.OrderBy(x => x.transform.position.x).ToList();

            //odstrani polovico
            for (int i=(aPlatforms.Count/2); i<aPlatforms.Count; i++)
            {
                Destroy(aPlatforms[i]);
            }
            aPlatforms.RemoveRange((aPlatforms.Count / 2),(aPlatforms.Count/2));


            int coinIndex = aCoins.FindIndex(a => a.transform.position.x > aPlatforms[aPlatforms.Count-1].transform.position.x);
            int spikeIndex = aSpikes.FindIndex(a => a.transform.position.x > aPlatforms[aPlatforms.Count-1].transform.position.x);

            if (coinIndex != -1)
            {
                for (int i = coinIndex; i < aCoins.Count; i++)
                {

                    Destroy(aCoins[i]);
                }
                aCoins.RemoveRange(coinIndex, aCoins.Count - coinIndex);
            }
            if (spikeIndex != -1)
            {
                for (int i = spikeIndex; i < aSpikes.Count; i++)
                {

                    Destroy(aSpikes[i]);
                }
                aSpikes.RemoveRange(spikeIndex, aSpikes.Count - spikeIndex);
            }

            //dodaj polovico od drugega

            int drugiLevelPlatformIndex = k_with_me.aPlatforms.FindIndex(a => a.transform.position.x > aPlatforms[aPlatforms.Count - 1].transform.position.x);
            int drugiLevelCoinsIndex = k_with_me.aCoins.FindIndex(a => a.transform.position.x > aPlatforms[aPlatforms.Count - 1].transform.position.x);
            int drugiLevelSpikesIndex = k_with_me.aSpikes.FindIndex(a => a.transform.position.x > aPlatforms[aPlatforms.Count - 1].transform.position.x);
            if (drugiLevelPlatformIndex != -1)
            {
                for (int i = drugiLevelPlatformIndex; i < k_with_me.aPlatforms.Count; i++)
                {
                    aPlatforms.Add(Instantiate(k_with_me.aPlatforms[i]));
                    //Debug.Log("4i: " + i);

                }
            }
            if (drugiLevelCoinsIndex != -1)
            {
                for (int i = drugiLevelCoinsIndex; i < k_with_me.aCoins.Count; i++)
                {
                    //Debug.Log("5i: " + i);
                    aCoins.Add(Instantiate(k_with_me.aCoins[i]));

                }
            }
            if (drugiLevelSpikesIndex != -1)
            {
                for (int i = drugiLevelSpikesIndex; i < k_with_me.aSpikes.Count; i++)
                {
                    //Debug.Log("6i: " + i);
                    aSpikes.Add(Instantiate(k_with_me.aSpikes[i]));

                }
            }
            fixKrizanje();
            fitness();
        }
        
    }

    void Start()
    {
        Debug.Log("testTESTETSTES");
        //pop_size level= new pop_size(p,c,s,f,nP,nC,nS,xMi,xMa);
        List<pop_size> levels = new List<pop_size>(); 
        for (int i = 0; i < 100; i++)
        {
            levels.Add(new pop_size(p, c, s, f, nP, nC, nS, xMi, xMa));
            levels[i].startBuilding();
        }
        //levels.display();

        //najboljsega poiscemo in damo v polje
        double max = 0;
        int indeksMax = 0;
        for (int i = 0; i < 100; i++)
        {
            if (max < levels[i].fit)
            {
                max = levels[i].fit;
                indeksMax = i;
            }
        }
        //levels[indeksMax].display();
        List<pop_size> selekcija = new List<pop_size>();
        selekcija.Add(levels[indeksMax]);

        //random selekcija, izberemo dva, noter damo boljšega
        for (int i = 0; i < levels.Count-1; i++)
        {
            int rnd1 = Random.Range(0, levels.Count);
            int rnd2 = Random.Range(0, levels.Count);
            if (levels[rnd1].fit < levels[rnd2].fit)
            {
                selekcija.Add(levels[rnd2]);
            }
            else
            {
                selekcija.Add(levels[rnd1]);
            }
        }
        //mutacija (add coin)
        for (int i=0; i<selekcija.Count; i++)
        {
            selekcija[i].mutacija();
        }
        //krizanje
        //for (int i=0; i<selekcija.Count; i++)
       // {
        //    int rnd = Random.Range(0, selekcija.Count);
        Debug.Log("fit:" + selekcija[0].fit);
        Debug.Log("Statistika: " + selekcija[0].aPlatforms.Count + " " + selekcija[0].aCoins.Count + " " + selekcija[0].aSpikes.Count);

        //selekcija[0].krizanje(selekcija[rnd]);
        //selekcija[0].display();
        //Debug.Log("2fit:" + selekcija[0].fit);
        //Debug.Log("22Statistika: " + selekcija[0].aPlatforms.Count + " " + selekcija[0].aCoins.Count + " " + selekcija[0].aSpikes.Count);
        ////}
        int r= new int();
        List<int> indexi = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            r = Random.Range(0, selekcija.Count);
            selekcija[i].krizanje(selekcija[r]);

        }
        max = 0;
        indeksMax = 0;
        for (int i = 0; i < 100; i++)
        {
            if (max < selekcija[i].fit)
            {
                max = selekcija[i].fit;
                indeksMax = i;
            }
        }
        selekcija[indeksMax].display();
        Debug.Log("2fit:" + selekcija[indeksMax].fit);
        Debug.Log("22Statistika: " + selekcija[indeksMax].aPlatforms.Count + " " + selekcija[indeksMax].aCoins.Count + " " + selekcija[indeksMax].aSpikes.Count);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
