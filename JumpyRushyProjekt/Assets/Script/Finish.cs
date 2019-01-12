using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour {
    public GameObject panel;
    public static string user, pass;

    public static int tt_coins, tt_shield, tt_time,tt_speed;

    // Use this for initialization
    void Start()
    {
        tt_coins = tt_shield = tt_time =tt_speed= 0;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        else
        {
            tt_coins = Score.score;
            Debug.Log(Score.score);
            if (PowerUps.shield == true)
            {
                Score.score += 30;
                Debug.Log("+30");
                tt_shield = 30;
            }
            else
            {
                tt_shield = 0;
            }
            if (Controls.speed >= Timer.max_hitrost)
            {
                Score.score += 30;
                Debug.Log("+30");
                tt_speed = 30;

            }
            else if (Controls.speed < Timer.max_hitrost)
            {
                tt_speed = 3 * (int)Controls.speed;
                Score.score += 3*(int)Controls.speed;
                Debug.Log("+"+(3*(int)Controls.speed));
            }
            
            //if(2*(int)Timer.cas_static<Score.score*0.5)
            tt_time = 2 * (int)Timer.cas_static;
            Score.score -= 1*(int)Timer.cas_static;
            Debug.Log("-"+(1*(int)Timer.cas_static));
            panel.SetActive(true);
            Time.timeScale = 0;

            if (MainMenu.vse_ok)
            {
                TcpClient client = null;
                string ip = "192.168.0.17";
                int port = 1234;

                string protokol_tocke = "T";
                try
                {
                    client = new TcpClient(ip, port);        //isto kot connect
                    NetworkStream dataStream = client.GetStream();

                    dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
                    Debug.Log(Score.score + " " + user + " " + pass);
                    byte[] strMessage = Encoding.UTF8.GetBytes(protokol_tocke + Score.score + "|" + user + "|" + pass + "|");
                    dataStream.Write(strMessage, 0, strMessage.Length);

                    /*DOBIL SEM*/
                    byte[] message = new byte[100];
                    dataStream.Read(message, 0, message.Length);
                    string strMessage2 = Encoding.UTF8.GetString(message);
                    string odziv = strMessage2.Split('|')[0];
                    Debug.Log("Dobil sem sporočilo: " + odziv);
                    if (odziv == "OK tocke")
                    {

                        Debug.Log("tocke vstavljene");
                    }
                    dataStream.Close(); // vedno, VEDNO!!! zapiramo podatkovne toke...
                    client.Close();     // ...kot tudi zapremo povezavo, ko smo končali
                }
                catch (Exception e)
                {
                    Debug.Log("Napaka:" + e);
                }
                //Time.timeScale = 1;
                Classification.konecIgre = true;
                Classification.opravilLevel = true;
            }
            /*toDO zakomentiraj pol*/
            Classification.konecIgre = true;
            Classification.opravilLevel = true;


        }
    }
    public void Continue()
    {
        if (MainMenu.vse_ok)
        {
            MainMenu.vse_ok = true;
        }
        else if (MainMenu.vse_ok == false)
        {
            MainMenu.vse_ok = false;
        }
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;

    }
}
