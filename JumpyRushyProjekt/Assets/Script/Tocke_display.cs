using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tocke_display : MonoBehaviour {

    // Use this for initialization
    public Text t_coins;
    public Text t_shield;
    public Text t_time;
    public Text t_speed;
    public Text t_total;
	void Start () {
        t_coins.text ="+ "+ Finish.tt_coins.ToString();
        t_shield.text ="+ "+ Finish.tt_shield.ToString();
        t_time.text ="- "+ Finish.tt_time.ToString();
        t_speed.text = "+ " + Finish.tt_speed.ToString();
        t_total.text = Score.score.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
