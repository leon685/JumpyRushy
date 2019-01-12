using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompressEffect1 : MonoBehaviour
{
    public AudioClip clip;
    public static AudioClip orgEF1clip;
    List<double> blok;
    int N = 64;
    BinaryWriter writer;
    int stopnja = -1;
    bool[] aBit;
    int n = 0;
    BitArray bitArray;
    int[] left;
    int[] right;
    int[] M;
    int[] S;
    List<List<double>> X;
    float[] sampleBuffer;
    bool glava = false;
    string dataPath;
    int frekvenceVzorcenja;
    public static bool loadingDoneEffect1 = false;
    // Use this for initialization
    void Start()
    {
        orgEF1clip = clip;
        sampleBuffer = new float[clip.samples * clip.channels];
        clip.GetData(sampleBuffer, 0);
        dataPath = Application.persistentDataPath;
        frekvenceVzorcenja = clip.frequency;
        for (int i = 0; i < sampleBuffer.Length; i++)
        {
            float tmp = sampleBuffer[i] * 32768.0f; //da dobim org vrednosti in ne med -1,1
            sampleBuffer[i] = tmp;
        }
        Thread nit = new Thread(new ThreadStart(startCMP));
        //Thread nitSrednja = new Thread(() => startCMP(30, "/srednjaCMP.wav"));
        nit.Start();
        Debug.Log("MAIN FINISH");
    }

    /*void cmp(int t)
    {
        Thread.Sleep(t);
        Debug.Log("THREAD FINISH" + t);
    }*/

    void startCMP()
    {
        int st = 1;
        string fName = "/cmp.wav";
        while (st <= 3)
        {
            blok = new List<double>();
            X = new List<List<double>>();
            aBit = new bool[8];
            bitArray = new BitArray(8);



            if (st == 1)
            {
                stopnja = 0;
                fName = "/visokaE1CMP.wav";
            }
            else if (st == 2)
            {
                stopnja = 30;
                fName = "/srednjaE1CMP.wav";
            }
            else if (st == 3)
            {
                stopnja = 55;
                fName = "/nizkaE1CMP.wav";
            }



            left = new int[sampleBuffer.Length / 2];
            right = new int[sampleBuffer.Length / 2];
            M = new int[left.Length];
            S = new int[right.Length];

            if (stopnja == 55)
            {
                int sgn = 0;
                for (int i = 0; i < sampleBuffer.Length; i++)
                {
                    if (sampleBuffer[i] < 0)
                    {
                        sgn = -1;
                    }
                    else if (sampleBuffer[i] == 0)
                    {
                        sgn = 0;
                    }
                    else if (sampleBuffer[i] > 0)
                    {
                        sgn = 1;
                    }
                    double rez = Math.Log(1 + 255 * Math.Abs(sampleBuffer[i])) / Math.Log(1 + 255);
                    float tmp = sampleBuffer[i];
                    sampleBuffer[i] = (float)sgn * (float)rez;
                    sampleBuffer[i] = sampleBuffer[i] * 128;
                }
            }

            int l = 0;
            int r = 0;
            //delitev kanalov
            for (int i = 0; i < sampleBuffer.Length; i = i + 2)
            {
                left[l] = (int)sampleBuffer[i];
                l++;
            }
            for (int i = 1; i < sampleBuffer.Length; i = i + 2)
            {
                right[r] = (int)sampleBuffer[i];
                r++;
            }

            n = 0;
            blok = new List<double>();
            X = new List<List<double>>();
            aBit = new bool[8];
            bitArray = new BitArray(8);
            glava = false;
            /*M in S*/
            for (int i = 0; i < M.Length; i++)
            {
                M[i] = (short)((left[i] + right[i]) / 2);
                S[i] = (short)((left[i] - right[i]) / 2);
            }

            /*writer*/
            writer = new BinaryWriter(File.Open(dataPath + fName, FileMode.Create));

            //short[] test = new short[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            //kompresija
            cmp(M);
            cmp(S);

            /*KONEC*/
            if (n < 8)
            {
                bitArray = new BitArray(aBit);
                byte byteZapis = ConvertToByte(bitArray);
                writer.Write(byteZapis);
                writer.Close();
            }
            else
                writer.Close();
            //Debug.Log("Done");
            //next = true;
            //AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0), 1.0f);
            st++;
        }
        loadingDoneEffect1 = true;
    }
    void cmp(int[] c)
    {
        blok.Clear();
        X.Clear();
        /*razdelitev*/

        int velBloka = 2 * N;
        int st_M_N = -N;
        //short[] test = new short[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        while (st_M_N < c.Length)
        {
            for (int i = 0; i < velBloka; i++)
            {
                if (st_M_N < 0)
                {
                    blok.Add(0);


                }
                else if (st_M_N >= c.Length)
                {
                    blok.Add(0);
                }
                else
                {
                    blok.Add(c[st_M_N]);

                }
                st_M_N++;
            }
            //blok je poln
            st_M_N = st_M_N - N;
            //okenska funkcija
            for (int i = 0; i < velBloka; i++)
            {
                double t = okenskaFunkcija(i, blok[i]);
                blok[i] = t;
            }
            //MDCT
            MDCT();
            blok.Clear();
        }

        /*kvantizacija*/
        for (int i = 0; i < X.Count; i++)
        {
            for (int j = (N - stopnja); j < N; j++)
            {
                X[i][j] = 0;
            }
        }

        //zapis glave je tukaj, posledica napačnega razumevanja
        if (glava == false)
        {
            int stevilovseh = X.Count * (N - stopnja); //stevilo vzorcev za m/s
            stevilovseh = stevilovseh * 2;      //stevilo vseh vrozcev

            short NN = (short)N;
            byte MM = (byte)stopnja;

            writer.Write(stevilovseh);
            writer.Write(NN);
            writer.Write(frekvenceVzorcenja);
            writer.Write(MM);
            glava = true;
        }
        //telo
        for (int i = 0; i < X.Count; i++)
        {
            for (int j = 0; j < N - stopnja; j++)
            {
                //byte log = (byte)Math.Ceiling(Math.Log(Math.Abs(X[i][j]), 2));  //dolzina
                int iD = (int)Math.Log(Math.Abs(X[i][j]), 2) + 2;       //+2 : 1 zaradi logaritma (gor zaokrožujemo) + 1 zaradi komplementa
                byte log = (byte)iD;
                if (X[i][j] == 0)
                {
                    log = 1;        //za ničlo 1 bit ker nima komplementa
                    iD = 1;
                }
                /*else
                {
                    log = (byte)(log + 1);  //zaradi komplementa in dekompresije
                }*/
                //dolzina za koeficient
                for (int k = 0; k < 6; k++)
                {
                    if (n < 8)
                    {
                        aBit[n] = getBit(log, k);
                        n++;
                    }
                    if (n == 8)
                    {
                        bitArray = new BitArray(aBit);
                        byte byteZapis = ConvertToByte(bitArray);
                        writer.Write(byteZapis);
                        n = 0;
                    }
                }
                /*dobim polje bitov (booleans od stevilke)*/
                byte[] numBytes = BitConverter.GetBytes((int)X[i][j]);
                int d = (int)log;  //dolzina
                bool[] poljeBitov = new bool[d];
                int l = 0;
                for (int o = 0; o < numBytes.Length; o++)
                {
                    for (int p = 0; p < 8; p++)
                    {
                        poljeBitov[l] = getBit(numBytes[o], p);
                        l++;
                        if (l == d)
                            break;
                    }
                    if (l == d)
                        break;
                }

                /*dejanski zapis v dat*/
                for (int o = 0; o < d; o++)
                {
                    if (n < 8)
                    {
                        aBit[n] = poljeBitov[o];
                        n++;
                    }
                    if (n == 8)
                    {
                        bitArray = new BitArray(aBit);
                        byte byteZapis = ConvertToByte(bitArray);
                        writer.Write(byteZapis);
                        n = 0;
                    }
                }
            }
        }
        //for (int i = 0; i < X.Count; i++)
        //{
        //    for (int j = 0; j < N - stopnja; j++)
        //    {
        //        testXCMP.Add((long)X[i][j]);
        //    }
        //}
    }
    bool getBit(byte b, int bitNumber)
    {
        var bit = (b & (1 << bitNumber)) != 0;
        return bit;

    }
    void MDCT()
    {
        List<double> lSum = new List<double>();
        double sum = 0;
        for (int k = 0; k < N; k++)
        {
            for (int n = 0; n < (2 * N); n++)
            {
                sum = sum + (blok[n] * Math.Cos((Math.PI / N) * (n + 0.5 + (N * 0.5)) * (k + 0.5)));
            }
            lSum.Add(Math.Round(sum));
            sum = 0;
        }
        X.Add(lSum);
    }
    double okenskaFunkcija(int i, double vr)
    {
        double a = vr * Math.Sin((Math.PI / (2 * N)) * (i + 0.5));
        return a;
    }
    byte ConvertToByte(BitArray bits)
    {
        byte[] bytes = new byte[1];
        bits.CopyTo(bytes, 0);
        return bytes[0];
    }
}
