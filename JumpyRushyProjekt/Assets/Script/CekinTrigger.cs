using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CekinTrigger : MonoBehaviour {

    // Use this for initialization
    public int da_Tock;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>()==null)
        {
            return;
        }
        Score.DodajTocke(da_Tock);
        Destroy(gameObject);
    }
}
