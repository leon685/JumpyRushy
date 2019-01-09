using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class Classification : MonoBehaviour {
    static int t=0;
    public static int pobral=0;
    public static int skoki=0;
    public static bool stop = false;
    List<GameObject> spikes;
    public static bool konecIgre;
    public static int stC;
    int stS;
    public static bool opravilLevel = false;
	// Use this for initialization
	void Start () {
        stop = false;
        konecIgre = false;
        opravilLevel = false;
        spikes = GameObject.FindGameObjectsWithTag("trap_spike").ToList();
        stS = spikes.Count;
        stC = 0;
    }

    // Update is called once per frame
    void Update () {
        if (MainMenu.vse_ok && konecIgre == true)
        {
            TcpClient client = null;
            string ip = "192.168.0.17";
            int port = 1234;
            string protokol_SUINZ = "U";

            try
            {
                client = new TcpClient(ip, port);        //isto kot connect
                NetworkStream dataStream = client.GetStream();

                dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
                byte[] strMessage = Encoding.UTF8.GetBytes(protokol_SUINZ + pobral + "|" + skoki + "|" + stC+ "|" + stS + "|" + opravilLevel + "|");
                dataStream.Write(strMessage, 0, strMessage.Length);

                /*DOBIL SEM*/
                byte[] message = new byte[100];
                dataStream.Read(message, 0, message.Length);
                string strMessage2 = Encoding.UTF8.GetString(message);
                string o = strMessage2.Split('|')[0];
                Debug.Log("Dobil sem sporočilo: " + o);
                if (o == "OK SUINZ")
                {

                    Debug.Log("Classification good");
                }
                dataStream.Close(); 
                client.Close();     
            }
            catch (Exception e)
            {
                Debug.Log("Napaka:" + e);
            }
            pobral = 0;
            skoki = 0;
            opravilLevel = false;
            konecIgre = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        stop = true;
        
        //kliči predikcijo
        if (MainMenu.vse_ok)
        {
            Thread nit = new Thread(new ThreadStart(predict));
            nit.Start();
        }
    }
    void predict()
    {
        TcpClient client = null;
        string ip = "192.168.0.17";
        int port = 1234;
        string protokol_PREDICT = "N";

        try
        {
            client = new TcpClient(ip, port);        //isto kot connect
            NetworkStream dataStream = client.GetStream();

            dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
            byte[] strMessage = Encoding.UTF8.GetBytes(protokol_PREDICT + pobral + "|" + skoki + "|" + stC + "|" + stS + "|");
            dataStream.Write(strMessage, 0, strMessage.Length);

            /*DOBIL SEM*/
            byte[] message = new byte[100];
            dataStream.Read(message, 0, message.Length);
            string strMessage2 = Encoding.UTF8.GetString(message);
            string odziv = strMessage2.Split('|')[0];
            int rez = Convert.ToInt32(odziv);
            if (rez == 0)
            {
                Controls.speed = 5;
                Controls.zacetna_hitrost = 5;
                Timer.max_hitrost = 5;
            }

            dataStream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Napaka:" + e);
        }
    }
}
