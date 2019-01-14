using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Emgu.CV;
//using Emgu.CV.Util;
//using Emgu.CV.UI;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using UnityEngine.Networking;
//using System.Drawing;
public class MainMenu : MonoBehaviour {
    
    public static bool accelerometer;
    public static bool stopnjaStiskanja;
    public InputField username;
    public InputField password;
    public InputField nPlatforms;
    public InputField nCoins;
    public InputField nSpikes;

    public static int stPlatform;
    public static int stCekinov;
    public static int stPasti;

    public GameObject login;
    public GameObject main;
    public GameObject panel;
    public GameObject ze_obstaja;
    public GameObject invalid;
    public GameObject camera_sent;

    public static string u_name;
    public static string p_word;
    private static bool vpisan,registriran;
    string ip = "192.168.0.17";
    int port = 1234;
    string protokol_vpis = "V";
    string protokol_prijava = "P";
    string protokol_acc="A";
    public static bool vse_ok;
    public static bool offline;
    //private const string URL = "http://localhost:1210/api/Pregled/1";
    public void Login() {
        Debug.Log(username.text + " " + password.text);
            TcpClient client = null;
            
            try
            {
                client = new TcpClient(ip, port);        //isto kot connect
                NetworkStream dataStream = client.GetStream();

                dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
                byte[] strMessage = Encoding.UTF8.GetBytes(protokol_vpis + username.text + "|" + password.text + "|");
                dataStream.Write(strMessage, 0, strMessage.Length);

                /*DOBIL SEM*/
                byte[] message = new byte[100];
                dataStream.Read(message, 0, message.Length);
                string strMessage2 = Encoding.UTF8.GetString(message);
                string odziv = strMessage2.Split('|')[0];
                string acc = strMessage2.Split('|')[1];
                Debug.Log("Dobil sem sporočilo: " + odziv+" cifra "+acc);
                if (odziv == "OK")
                {
                    u_name = username.text;
                    p_word = password.text;
                Finish.user = username.text;
                Finish.pass = password.text;
                    vpisan = true;
                if (acc[0] == '1')
                {
                    accelerometer = true;
                    Toggle_change(accelerometer);
                }
                else
                {
                    accelerometer = false;
                    Toggle_change(accelerometer);
                }
                    Debug.Log("lahko naprej");
                }
                dataStream.Close(); // vedno, VEDNO!!! zapiramo podatkovne toke...
                client.Close();     // ...kot tudi zapremo povezavo, ko smo končali
            }
            catch (Exception e)
            {
                Console.WriteLine("Napaka:" + e);
            }
            if (vpisan) //vpisan je samo preverimo ce je dejanski obraz za telefonom
            {
                SceneManager.LoadScene("Camera");

            }
        else
        {
            invalid.SetActive(true);
        }
        /*NetworkClient a= new NetworkClient();
        a.Connect("192.168.0.17", 1234);
        if (a.isConnected)
        {
            Debug.Log("connected");
        }*/
        //UnityWebRequest www = UnityWebRequest.Get("http://localhost:1210/api/Pregled/1");
        // WWW request = new WWW("http://localhost:1210/api/Pregled/1");

        //StartCoroutine(OnResponse(www));
        //string test1=RestClient.Get("http://localhost:1210/api/Pregled/1");
        // WWW re=new WWW()
        /* WWWForm form = new WWWForm();
         Dictionary<string, string> headers = new Dictionary<string, string>();
         headers.Add("header-name", "header content");
         headers.Add("Postman-Token", "943348fb-fffc-4747-a428-b2a2880f91b7");
         headers.Add("Cache-Control", "no-cache");
         headers.Add("Authorization", "Bearer 6XzTZG1vz-wxQkkZnrqQcYUH1CJugAt-WeDsA5KujzgsE-vNM3HBDWOWseJBJE1vhukWzbJRGvxF7LfTrArW9TCFw4Uo8S969SeiqZKvb5FB3_wvSQxTsR4lYahOCRn7cVthitqDgamqkotYnjpa7Vw8rwT8uTemxDSJAsupVfXLe5A_2_nCR_2SPu3LUSSzJTJb88tKup60zx-9OpaCfy0ItnASwsv9RqUjAUwHv9ktYslZ-OyVy_zA8t93Iz2EdoHVwB24msIA6xVM1wrDFCNZFHjswzRqdjGLblHf0At6ay4OVFcgxr_RpND_oPjsJhK_u5OsyGvCd1y3NJlhbjM5hpIL35OybBWdYhqc4gQMmFiuTc-d4rQ-1PuPiMWd");
         headers.Add("Content-Type", "application/x-www-form-urlencoded");
         form.AddField();*/
        //login.SetActive(false);
        //main.SetActive(true);
       
    }
 
   public void Register()
    {
        Debug.Log(username.text + " " + password.text);
        TcpClient client = null;
        if (username.text.Length > 2 && password.text.Length > 2)
        {
            try
            {
                client = new TcpClient(ip, port);        //isto kot connect
                NetworkStream dataStream = client.GetStream();

                dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
                byte[] strMessage = Encoding.UTF8.GetBytes(protokol_prijava + username.text + "|" + password.text + "|");
                dataStream.Write(strMessage, 0, strMessage.Length);

                /*DOBIL SEM*/
                byte[] message = new byte[100];
                dataStream.Read(message, 0, message.Length);
                string strMessage2 = Encoding.UTF8.GetString(message);
                string odziv = strMessage2.Split('|')[0];
                Debug.Log("Dobil sem sporočilo: " + odziv);
                if (odziv == "OK registriran")
                {
                    // u_name = username.text;
                    // p_word = password.text;
                    registriran = true;

                    Debug.Log("lahko vpis");
                }
                else if (odziv == "OBSTAJA")
                {
                    ze_obstaja.SetActive(true);
                    Debug.Log("ze obstaja");
                }
                dataStream.Close(); // vedno, VEDNO!!! zapiramo podatkovne toke...
                client.Close();     // ...kot tudi zapremo povezavo, ko smo končali
            }
            catch (Exception e)
            {
                Console.WriteLine("Napaka:" + e);
            }
            if (registriran)
            {
                panel.SetActive(true);

            }
        }
        else
        {
            invalid.SetActive(true);
        }
    }
    public void GeneratedPlay()
    {
        stPlatform = Convert.ToInt32(nPlatforms.text);
        stCekinov = Convert.ToInt32(nCoins.text); ;
        stPasti = Convert.ToInt32(nSpikes.text);

        SceneManager.LoadScene("Generated");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Camera()
    {
        SceneManager.LoadScene("Camera");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Toggle_change(bool t)
    {
        
        accelerometer = t;

        if (vse_ok)
        {
            TcpClient client = null;

            try
            {
                
                string cifra = "";
                if (accelerometer == true)
                    cifra = "1";
                else
                    cifra = "0";
                client = new TcpClient(ip, port);        //isto kot connect
                NetworkStream dataStream = client.GetStream();

                dataStream = client.GetStream(); // pridobivanje podatkovnega toka in pisanje vanj
                byte[] strMessage = Encoding.UTF8.GetBytes(protokol_acc + cifra);
                dataStream.Write(strMessage, 0, strMessage.Length);
                Debug.Log("vrednost= " + protokol_acc + " " + cifra);
            }
            catch (Exception ex)
            {
                Debug.Log("error " + ex);
            }
        }

    }

    void Start()
    {
        if (vse_ok)
        {
            login.SetActive(false);
            main.SetActive(true);
        }
        else if (offline == true)
        {
            login.SetActive(false);
            main.SetActive(true);
        }
        else if (!vse_ok)
        {
            Logout();
        }
       
       
    }
    public void Logout()
    {
        vse_ok = false;
        offline = false;
        u_name="";
        p_word="";
        vpisan=false;
        registriran = false;
    }
    public void Offline()
    {
        offline = true;
    }
    public void Leaderboard()
    {
        SceneManager.LoadScene("Leaderboards");
    }

}
