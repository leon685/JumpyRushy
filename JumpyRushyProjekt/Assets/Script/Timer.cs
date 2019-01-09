using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private float cas;
    private Text text;
    public float pospesek;
    public float max;
    public static int max_hitrost;
    public static int cas_static;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        max_hitrost = (int)max;
	}
	
	// Update is called once per frame
	void Update () {
        cas = cas + Time.deltaTime;
        cas_static = (int)cas;

        if (!Classification.stop)
        {
            Classification.stC = cas_static;
        }
        string min = ((int)cas / 60).ToString();
        string s = Mathf.Round((cas % 60)).ToString();
        if (Controls.speed < Controls.zacetna_hitrost)
        {
            Controls.speed += Time.deltaTime * (pospesek*8);
        }
        else if (Controls.speed>=Controls.zacetna_hitrost && Controls.speed < max_hitrost)
        {
            Controls.speed += Time.deltaTime * pospesek;
        }
        Debug.Log("hitrost= "+Controls.speed);
        text.text = min+":"+s;

    }
}
