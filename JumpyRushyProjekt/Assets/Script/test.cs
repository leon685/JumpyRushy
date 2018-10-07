using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Emgu.CV;
//using Emgu.CV.Util;
//using Emgu.CV.UI;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System;
//using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour {

    // Use this for initialization
    /*public void Shrani() {
        
        Image<Bgr, byte> picture = new Image<Bgr, byte>(200,200);
        Bgr myWhiteColor = new Bgr(255, 0, 0);
        for(int i = 0; i < 200; i++)
        { picture[i, i] = myWhiteColor; }
        picture.Save("test2.jpg");
    }*/
    public GameObject sent;
    public GameObject failcheck;

    private bool jecamera;
    public static string pot;
    private WebCamTexture frontCam;
    // private Image<Bgr, byte> currentFrameBgr;
    // private Image<Bgr, byte> frame;
    string ip = "192.168.1.104";
    int port = 1234;
    string protokol_vpis = "V";
    string protokol_prijava = "P";
    string protokol_slika = "S";
    string protokol_check = "C";

    bool poslal_sliko;
    public static bool slika;
    public RawImage rawimage2;
   // private VideoCapture cap2;
    Texture2D tex;
    private Texture defaultBackground;
    public AspectRatioFitter fit;
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("Ni kamere");
            jecamera = false;
            return;
        }

        for (int i=0; i<devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                frontCam = new WebCamTexture(devices[i].name);
            }
        }
        if (frontCam == null)
        {
            Debug.Log("ni sprednje kamere");
            return;
        }
        frontCam.Play();
        rawimage2.texture = frontCam;
        jecamera = true;
        /*cap2 = new VideoCapture(0);
        if (cap2.IsOpened)
        {
            Debug.Log("naso kamero?");

        }
        cap2.SetCaptureProperty(CapProp.Fps, 60);

        tex = new Texture2D(2, 2);
        //*/

        //Image<Bgr, byte> slika = new Image<Bgr, byte>("test.jpg");
        //    slika.ToBitmap();

        //MemoryStream memstream = new MemoryStream();
        //slika.Bitmap.Save(memstream, slika.Bitmap.RawFormat);

        //tex = new Texture2D(slika.Width, slika.Height);
        //tex.LoadImage(memstream.ToArray());
        //rawimage2.texture = tex;

    }
    public void Slikaj() {
        //Application.CaptureScreenshot("slika.png");
        tex = new Texture2D(frontCam.width,frontCam.height);
        tex.SetPixels(frontCam.GetPixels());
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        pot = "Slika.png";
        File.WriteAllBytes(Application.persistentDataPath+"/"+pot, bytes);  //shrani sliko
        //File.WriteAllBytes(pot, bytes);
        //Debug.Log(Application.persistentDataPath);

        Stream imageFileStream = File.OpenRead(Application.persistentDataPath + "/" + pot);

        byte[] byteArray = new byte[imageFileStream.Length];

        imageFileStream.Read(byteArray, 0, (int)imageFileStream.Length);
        TcpClient client = null;
        
        imageFileStream.Close();
        try
        {
            client = new TcpClient(ip, port);        //isto kot connect
            NetworkStream dataStream = client.GetStream();
            /*poslemo S da server ve da bo naslednje bral sliko*/
            byte[] strMessage = Encoding.UTF8.GetBytes(protokol_slika);
            dataStream.Write(strMessage, 0, strMessage.Length);

            /*beremo ce je ok*/
            byte[] ok_s = new byte[100];
            dataStream.Read(ok_s, 0, ok_s.Length);
            string strMessage2 = Encoding.UTF8.GetString(ok_s);
            string odziv = strMessage2.Split('|')[0];
            Debug.Log("Dobil sem sporočilo: " + odziv);
            if (odziv=="OK slika")
            {
                dataStream.Write(byteArray, 0, byteArray.GetLength(0));
                Debug.Log("poslal sliko");
                poslal_sliko = true;
                sent.SetActive(true);
                
            }
            else
            {
                poslal_sliko = false;
            }

            /*DOBIL SEM*/
            /*byte[] message = new byte[100];
            Debug.Log("pred freeze ");
            dataStream.Read(message,0, message.Length);
            string strMessage2 = Encoding.UTF8.GetString(message);
            string odziv = strMessage2.Split('|')[0];
            Debug.Log("Dobil sem sporočilo: " + odziv);
            */
            dataStream.Close(); // vedno, VEDNO!!! zapiramo podatkovne toke...
            client.Close();     // ...kot tudi zapremo povezavo, ko smo končali
        }
        catch (Exception e)
        {
            Console.WriteLine("Napaka:" + e);
        }
        if (poslal_sliko)
        {
            slika = true;
        }
        else
        {
            
        }

    }

    public void Check()
    {
        if (poslal_sliko)
        {
            TcpClient client = null;

            try
            {
                client = new TcpClient(ip, port);        //isto kot connect
                NetworkStream dataStream = client.GetStream();
                /*poslemo C da server preveri če je oseba na sliki*/
                byte[] strMessage = Encoding.UTF8.GetBytes(protokol_check);
                dataStream.Write(strMessage, 0, strMessage.Length);

                /*beremo ce je ok*/
                byte[] ok_s = new byte[100];
                dataStream.Read(ok_s, 0, ok_s.Length);
                string strMessage2 = Encoding.UTF8.GetString(ok_s);
                string msg = strMessage2.Split('|')[0];
                string odziv = "";
                if (msg.Length > 1)
                {
                    odziv = msg.Split(' ')[0];

                }
                Debug.Log("Dobil sem sporočilo: " + odziv);
                if (odziv == "True")
                {
                    Debug.Log("OK odziv " + odziv);
                    MainMenu.vse_ok = true;
                    MainMenu.offline = false;
                }
                else
                {
                    MainMenu.vse_ok = false;
                    Debug.Log("WRONG odziv " + odziv);
                    failcheck.SetActive(true);
                }
                if (MainMenu.vse_ok)
                {
                    SceneManager.LoadScene("Menu");

                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception: " + ex);
            }

        }
    }
    void Update()
    {
        if (!jecamera)
            return;
        float ratio = (float)frontCam.width / (float)frontCam.height;
        fit.aspectRatio = ratio;

        float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
        rawimage2.rectTransform.localScale = new Vector3(1f,scaleY,1f);

        int orient = -frontCam.videoRotationAngle;
        rawimage2.rectTransform.localEulerAngles = new Vector3(0,0,orient);
        //Debug.Log("error0");
        ////currentFrameBgr = cap2.QueryFrame().ToImage<Bgr, byte>();
        //Debug.Log("error1");
        //Mat m = new Mat();
        //cap2.Read(m);
        //if (!m.IsEmpty)
        //{
        //    Debug.Log("error2");

        //    frame = m.ToImage<Bgr,byte>();
        //}
        //if (frame != null)
        //{
        //    Debug.Log("error3");
        //    // Image<Gray, byte> grayFrame = currentFrameBgr.Convert<Gray, byte>();



        //    //Convert this image into Bitmap, the pixel values are copied over to the Bitmap
        //    frame.ToBitmap();

        //    MemoryStream memstream = new MemoryStream();
        //    frame.Bitmap.Save(memstream, frame.Bitmap.RawFormat);

        //    tex = new Texture2D(frame.Width, frame.Height);
        //    tex.LoadImage(memstream.ToArray());
        //    rawimage2.texture = tex;

        //}

    }

    // Update is called once per frame

}
