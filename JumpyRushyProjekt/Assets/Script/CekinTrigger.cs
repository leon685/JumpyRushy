using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CekinTrigger : MonoBehaviour {
    public AudioSource a;
    // Use this for initialization
    public int da_Tock;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>()==null)
        {
            return;
        }
        Score.DodajTocke(da_Tock);
        a.Stop();
        if (DCMPEffects.soundEffect2 != null)
        {

            //AudioSource.PlayClipAtPoint(Zvok.backgroundMusic, new Vector3(0, 0, 0), 1.0f);
            a.clip = DCMPEffects.soundEffect2;
            a.loop = false;
            a.Play();
        }
        else
        {
            a.Play();
        }
        Destroy(gameObject);
    }
}
