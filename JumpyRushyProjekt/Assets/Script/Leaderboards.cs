using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboards : MonoBehaviour {
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Text text11;
    public Text text22;
    public Text text33;
    public Text text44;
    public Text text55;
    // Use this for initialization
    void Start () {

        TcpClient client = null;
        string ip = "192.168.0.17";
        int port = 1234;
        string protokol_lestvica = "L";
        string ime1, t1, ime2, t2, ime3, t3, ime4, t4, ime5, t5;
        if (MainMenu.vse_ok)
        {
            try
            {
                client = new TcpClient(ip, port);        //isto kot connect
                NetworkStream dataStream = client.GetStream();

                dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
                byte[] strMessage = Encoding.UTF8.GetBytes(protokol_lestvica);
                dataStream.Write(strMessage, 0, strMessage.Length);

                /*DOBIL SEM*/
                byte[] message = new byte[1024];
                dataStream.Read(message, 0, message.Length);
                string strMessage2 = Encoding.UTF8.GetString(message);
                Debug.Log("lestiva: "+strMessage2);
                string glava = strMessage2.Split(strMessage2[1])[0];
                if (glava == protokol_lestvica)
                {
                    string tmp = strMessage2.Split(strMessage2[0])[1];
                    text1.text = tmp.Split('|')[0];
                    text11.text = tmp.Split('|')[1];
                    text2.text = tmp.Split('|')[2];
                    text22.text = tmp.Split('|')[3];
                    text3.text = tmp.Split('|')[4];
                    text33.text = tmp.Split('|')[5];
                    text4.text = tmp.Split('|')[6];
                    text44.text = tmp.Split('|')[7];
                    text5.text = tmp.Split('|')[8];
                    text55.text = tmp.Split('|')[9];
                    


                }
                
                dataStream.Close(); // vedno, VEDNO!!! zapiramo podatkovne toke...
                client.Close();     // ...kot tudi zapremo povezavo, ko smo končali
            }
            catch (Exception e)
            {
                Console.WriteLine("Napaka:" + e);
            }

        }


            //text1.text = "BLA";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
