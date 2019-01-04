using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Zvok : MonoBehaviour {
    public AudioClip clip;
    public static AudioClip backgroundMusic;
    int stopnja=0;
     int N=64;
     List<double> blok;
     List<List<double>> X;
     List<List<double>> Y;
     int[] left;
    int[] right;
    int[] M;
    int[] S;
     int n = 0;
     int m = 0;
     bool[] aBit;
     BitArray bitArray;
     BinaryWriter writer;
     List<long> dcmpM_S;
     List<long> testXCMP;
     List<long> dcmpM;
     List<long> dcmpS;
     bool glava = false;

    public void startCMP () {
        blok = new List<double>();
        X = new List<List<double>>();
        Y = new List<List<double>>();
        dcmpM = new List<long>();
        dcmpS = new List<long>();
        dcmpM_S = new List<long>();
        testXCMP = new List<long>();
        aBit = new bool[8];
        bitArray = new BitArray(8);

        stopnja = ToggleSound.stopnja_zvok;


        float[] sampleBuffer= new float[clip.samples*clip.channels];
        clip.GetData(sampleBuffer, 0);

        left = new int[sampleBuffer.Length / 2];
        right = new int[sampleBuffer.Length / 2];
        M = new int[left.Length];
        S = new int[right.Length];

        for (int i=0; i<sampleBuffer.Length; i++)
        {
            float tmp = sampleBuffer[i] * 32768.0f; //da dobim org vrednosti in ne med -1,1
            sampleBuffer[i] = tmp;
        }

        if (stopnja == 63)
        {
            int sgn = 0;
            for (int i=0; i<sampleBuffer.Length; i++)
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
        writer = new BinaryWriter(File.Open(Application.persistentDataPath+"/cmp.wav", FileMode.Create));

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
       
        Debug.Log("Done");
        Debug.Log("Done");
        Debug.Log("Done");
        Debug.Log("Done");
        Debug.Log("Done");
        //AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0), 1.0f);
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
            int frekvenceVzorcenja = clip.frequency;
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

    public void startDCMP()
    {
        blok = new List<double>();
        X = new List<List<double>>();
        Y = new List<List<double>>();
        dcmpM = new List<long>();
        dcmpS = new List<long>();
        dcmpM_S = new List<long>();
        testXCMP = new List<long>();
        aBit = new bool[8];
        bitArray = new BitArray(8);






        BinaryReader r = new BinaryReader(File.Open(Application.persistentDataPath +"/cmp.wav", FileMode.Open));
        m = 0;
        int st = 0;
        /*glava*/
        int stVzorcev = r.ReadInt32();
        short N_read = r.ReadInt16();
        int fVzorcenja = r.ReadInt32();
        byte M_read = r.ReadByte();

        N = N_read;
        stopnja = M_read;

        /*telo*/
        byte tmp = r.ReadByte();
        bool[] boolD = new bool[6]; //boolean polje dolzine
        bool[] boolKoeficient;
        while (st < stVzorcev)
        {
            //za dolzino
            for (int i = 0; i < 6; i++)
            {
                if (m < 8)
                {
                    boolD[i] = getBit(tmp, m);
                    m++;
                    //st++;
                }
                if (m == 8)
                {
                    tmp = r.ReadByte();
                    m = 0;
                }
            }
            /*pretvorba da dobim dejansko dolzino*/
            BitArray baDolzina = new BitArray(boolD);
            byte dejanskaDolzina = ConvertToByte(baDolzina);
            //polje za koeficient

            boolKoeficient = new bool[dejanskaDolzina]; //potrebna vel
            bool[] fullK = new bool[64];   //polna velikost (64bitov)-zaradi komplementa, max dolzina (63?)
                                           /*beremo koeficient do dolzine*/
            for (int i = 0; i < dejanskaDolzina; i++)
            {
                if (m < 8)
                {
                    boolKoeficient[i] = getBit(tmp, m);
                    m++;
                }
                if (m == 8)
                {
                    tmp = r.ReadByte();
                    m = 0;
                }
            }
            /*zapolnimo vse bite*/
            for (int i = 0; i < fullK.Length; i++)
            {
                if (i >= dejanskaDolzina - 1)
                {
                    fullK[i] = boolKoeficient[dejanskaDolzina - 1];
                }
                else
                {
                    fullK[i] = boolKoeficient[i];
                }
            }
            //pretvorba, dobimo vrednost koeficienta
            BitArray tt = new BitArray(fullK);

            byte[] aByte = BitArrayToByteArray(tt);

            long num = BitConverter.ToInt64(aByte, 0);
            st++;
            dcmpM_S.Add(num);
        }
        bool same = true;
        for (int i = 0; i < testXCMP.Count; i++)
        {
            if (testXCMP[i] != dcmpM_S[i])
            {
                same = false;
            }
        }
        int velBloka = 2 * N;
        int st_M_N = 0;


        /*razdelitev v m in s*/
        for (int i = 0; i < dcmpM_S.Count; i++)
        {
            if (i < (dcmpM_S.Count / 2))
            {
                dcmpM.Add(dcmpM_S[i]);

            }
            else
            {
                dcmpS.Add(dcmpM_S[i]);
            }
        }

        //prave vrednosti M in S kanala
        List<int> orgSignalM = dcmp(dcmpM);
        Y.Clear();
        List<int> orgSignalS = dcmp(dcmpS);
        // levi in desni kanal
        List<long> dcmpL = new List<long>();
        List<long> dcmpR = new List<long>();
        for (int i = 0; i < orgSignalM.Count; i++)
        {
            dcmpL.Add(orgSignalM[i] + orgSignalS[i]);
            dcmpR.Add(orgSignalM[i] - orgSignalS[i]);
        }
        List<short> finalSignals = new List<short>();
        // damo v en list, (izmenično L in D tako kot smo pri cmp prebrali wav)
        bool l = true;
        int stL = 0;
        int stD = 0;
        for (int i = 0; i < (dcmpL.Count * 2); i++)
        {
            if (l == true)
            {
                finalSignals.Add((short)dcmpL[stL]);
                stL++;
                l = false;
            }
            else if (l == false)
            {
                finalSignals.Add((short)dcmpR[stD]);
                stD++;
                l = true;
            }
        }

        //zapis wav datoteke
        float[] zaUnityZapis = new float[finalSignals.Count];
        for (int i=0; i<zaUnityZapis.Length; i++)
        {
            if (stopnja == 63)
            {
                float temp = finalSignals[i] / 128.0F;
                int sgn = 0;
                if (finalSignals[i] < 0)
                {
                    sgn = -1;
                }
                else if (finalSignals[i] == 0)
                {
                    sgn = 0;
                }
                else if (finalSignals[i] > 0)
                {
                    sgn = 1;
                }
                double rez = (1 / 255.0F) * (Math.Pow((1 + 255.0F), Math.Abs(temp)) - 1);
                zaUnityZapis[i] = (float)sgn * (float)rez;
                zaUnityZapis[i] = zaUnityZapis[i] / 32768.0F;
            }
            else
            {
                zaUnityZapis[i] = finalSignals[i] / 32768.0F;
            }
        }
        backgroundMusic = AudioClip.Create("background", zaUnityZapis.Length, 2, fVzorcenja, false);
        backgroundMusic.SetData(zaUnityZapis, 0);
        
    }
    List<int> dcmp(List<long> a)
    {
        int st_M_N = 0;

        while (st_M_N < a.Count)
        {
            for (int i = 0; i < N - stopnja; i++)
            {
                blok.Add(a[st_M_N]);
                st_M_N++;
            }
            //blok je poln\
            IMDCT();
            blok.Clear();
        }

        //okenska funkcija
        for (int i = 0; i < Y.Count; i++)
        {
            for (int j = 0; j < Y[i].Count; j++)
            {
                double t = okenskaFunkcija(j, Y[i][j]);
                Y[i][j] = t;
            }
        }

        // rekonstrukcija
        List<double> dcmpSignala = new List<double>();
        for (int i = 0; i < Y.Count; i++)
        {
            for (int j = 0; j < Y[i].Count; j++)
            {
                dcmpSignala.Add(Math.Round(Y[i][j], 4));
            }
        }



        //pristevanje in zaokrozevanje
        List<int> orgSignala = new List<int>();
        for (int i = N; i < dcmpSignala.Count - (2 * N); i++)
        {
            if (i % (2 * N) == 0)
            {
                i = i + N;
            }
            orgSignala.Add((int)(Math.Ceiling((dcmpSignala[i] + dcmpSignala[i + N]))));
        }
        return orgSignala;
    }
    void IMDCT()
    {
        List<double> lSum = new List<double>();
        double sum = 0;
        for (int n = 0; n < 2 * N; n++)
        {
            for (int k = 0; k < N - stopnja; k++)
            {
                sum = sum + (blok[k] * Math.Cos((Math.PI / N) * (n + 0.5 + (N * 0.5)) * (k + 0.5)));
            }
            sum = (sum * ((double)2 / (double)N));
            lSum.Add(sum);
            sum = 0;
        }
        Y.Add(lSum);
    }
    byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }
}
