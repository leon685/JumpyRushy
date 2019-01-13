using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public Rigidbody2D charbody;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask ground;
    private bool onGround;
    public AudioSource a;

    float old_y;
    float y;
    public float hitrost;
    static public float zacetna_hitrost;
    static public float speed;

    List<double> vrY = new List<double>();
    float glajanjeY;

    private Animator anim;
    int st = 1;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        zacetna_hitrost = hitrost;
        speed = hitrost;
        charbody = GetComponent<Rigidbody2D>();
        old_y = Input.acceleration.y;
    }

    // Update is called once per frame
    void FixedUpdate() {

        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);

    }
    void Update () {
        bool nJump = false;
        bool hJump = false;
        bool sJump = false;
        /*-1 do 1 so vrednosti*/
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        Quaternion rot = new Quaternion(0, 0, 1, 0);
        y = Input.acceleration.y;

        Debug.Log("y: "+Input.acceleration.y);

        //vrY.Add(Input.acceleration.y);
      
        //if (vrY.Count == 3)
        //{
        //    if (st <= 2)
        //    {
        //        st++;
        //    }
        //    double Ps = (vrY[0] + vrY[1] + vrY[2]) / vrY.Count;
        //    double Pn = Ps * 0.66 + Input.acceleration.y * 0.33;
        //    glajanjeY = (float)Pn;
        //    y = glajanjeY;
        //    vrY.Clear();
        //    if (st == 1)
        //        old_y = glajanjeY;
        //}
       

        charbody.velocity = new Vector2(speed, charbody.velocity.y);
        anim.SetBool("Grounded", onGround);
        float diffY = Mathf.Abs(y) - Mathf.Abs(old_y);
        Debug.Log("diffY: " + diffY);
        //if (st >=2)
        //{
            if (diffY >= 0.3f)
            {
                Debug.Log("oldY: " + old_y + " newY:" + y);
                hJump = true;
            }
            else if (diffY >= 0.15f)
            {
                Debug.Log("oldY: " + old_y + " newY:" + y);
                nJump = true;
            }
            else if (diffY >= 0.5f)
            {
                Debug.Log("oldY: " + old_y + " newY:" + y);
                sJump = true;
            }
        //}
        if ((nJump && onGround && MainMenu.accelerometer==true) ||(Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 13);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null) { 
        
                if (ToggleSound.stopnja_zvok == 0)
                    a.clip = CompressEffect1.orgEF1clip;
                else
                    a.clip = DCMPEffects.soundEffect1;
                a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }
        }
        else if ((hJump && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 18);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null)
            {

                if (ToggleSound.stopnja_zvok == 0)
                    a.clip = CompressEffect1.orgEF1clip;
                else
                    a.clip = DCMPEffects.soundEffect1; a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }

        }
        else if ((sJump && onGround && MainMenu.accelerometer == true) || (Input.GetMouseButtonDown(0) && onGround))
        {
            charbody.velocity = new Vector2(charbody.velocity.x, 8);
            a.Stop();
            if (!Classification.stop)
            {
                Classification.skoki++;
            }
            if (DCMPEffects.soundEffect1 != null)
            {
                if (ToggleSound.stopnja_zvok == 0)
                    a.clip = CompressEffect1.orgEF1clip;
                else
                    a.clip = DCMPEffects.soundEffect1; a.loop = false;
                a.Play();
            }
            else
            {
                a.Play();
            }

        }
        else
        {
            old_y = Input.acceleration.y;
            y = Input.acceleration.y;
        }
    }
}
