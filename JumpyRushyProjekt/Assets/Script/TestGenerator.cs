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
            /*prestej se kolko je zaporednih spikeov na platformah? tisti ko ma manj zaporednih je bolši?*/
            //al bi naredo v fix? da jih izbriše če so?
            for (int i=0; i<aPlatforms.Count; i++)
            {

            }

            int diff_c = numberOfCoins - forFit_c;
            int diff_p = numberOfPlatforms - forFit_p;
            int diff_s = numberOfSpikes - forFit_s;
            fit = (diff_c + diff_s + diff_p);
            Debug.Log(diff_c + " "+ diff_s + " "+ diff_p);
            fit = fit / 100;
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
        public void krizanje(pop_size k_with_me)
        {

        }
        public void fixKrizanje()
        {

        }
    }

    void Start()
    {

        //pop_size level= new pop_size(p,c,s,f,nP,nC,nS,xMi,xMa);
        List<pop_size> levels = new List<pop_size>(); 
        for (int i = 0; i < 100; i++)
        {
            levels.Add(new pop_size(p, c, s, f, nP, nC, nS, xMi, xMa));
            levels[i].startBuilding();
        }
        //levels.display();

        //najboljsega damo v polje
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
        levels[indeksMax].display();
        List<pop_size> selekcija = new List<pop_size>();
        selekcija.Add(levels[indeksMax]);
        //random selekcija
        for (int i = 0; i < levels.Count; i++)
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
        //mutacija, add coin
        for (int i=0; i<selekcija.Count; i++)
        {
            selekcija[i].mutacija();
        }
        ////krizanje
        //for (int i=0; i<selekcija.Count; i++)
        //{
        //    int rnd = Random.Range(0, selekcija.Count);
        //    selekcija[i].krizanje(selekcija[rnd]);
        //}
        ////popravljanje
        //for (int i = 0; i < selekcija.Count; i++)
        //{
        //    selekcija[i].fixKrizanje();
        //}
        ////ovrednotenje
        //for (int i = 0; i < selekcija.Count; i++)
        //{
        //    selekcija[i].fitness();
        //}
    }


    // Update is called once per frame
    void Update()
    {

    }
}
