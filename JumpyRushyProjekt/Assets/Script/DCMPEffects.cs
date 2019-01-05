using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class DCMPEffects : MonoBehaviour
{
    public static AudioClip soundEffect1;
    public static AudioClip soundEffect2;
    static int stopnja;
    int N = 64;
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

    bool getBit(byte b, int bitNumber)
    {
        var bit = (b & (1 << bitNumber)) != 0;
        return bit;

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

    //public void callDCMP()
    //{
    //    if (ToggleSound.stopnja_zvok == stopnja)
    //    {
    //        return;
    //    }
    //    Thread nit = new Thread(new ThreadStart(startDCMP));
    //    nit.Start();
    //}

    public void startDCMP()
    {
        if (ToggleSound.stopnja_zvok == stopnja)
        {
            return;
        }
        int st_ponovitev = 1;
        while (st_ponovitev <= 2)
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




            string name = "/cmp.wav";
            if (st_ponovitev == 1)
            {
                if (ToggleSound.stopnja_zvok == 63)
                {
                    name = "/nizkaE1CMP.wav";
                }
                else if (ToggleSound.stopnja_zvok == 50)
                {
                    name = "/srednjaE1CMP.wav";
                }
                else if (ToggleSound.stopnja_zvok == 0)
                {
                    name = "/visokaE1CMP.wav";
                }
            }
            if (st_ponovitev == 2)
            {
                if (ToggleSound.stopnja_zvok == 63)
                {
                    name = "/nizkaE2CMP.wav";
                }
                else if (ToggleSound.stopnja_zvok == 50)
                {
                    name = "/srednjaE2CMP.wav";
                }
                else if (ToggleSound.stopnja_zvok == 0)
                {
                    name = "/visokaE2CMP.wav";
                }
            }
            BinaryReader r = new BinaryReader(File.Open(Application.persistentDataPath + name, FileMode.Open));
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
            for (int i = 0; i < zaUnityZapis.Length; i++)
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

            if (st_ponovitev == 1)
            {
                soundEffect1 = AudioClip.Create("effect1", zaUnityZapis.Length, 2, fVzorcenja, false);
                soundEffect1.SetData(zaUnityZapis, 0);
            }
            if (st_ponovitev == 2)
            {
                soundEffect2 = AudioClip.Create("effect2", zaUnityZapis.Length, 2, fVzorcenja, false);
                soundEffect2.SetData(zaUnityZapis, 0);
            }
            st_ponovitev++;
        }

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
