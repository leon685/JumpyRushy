using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ToggleSound : MonoBehaviour {

    ToggleGroup toggleGroupInstance;
    public static int stopnja_zvok = 0;
	// Use this for initialization
    public Toggle currentSelection
    {
        get { return toggleGroupInstance.ActiveToggles().FirstOrDefault(); }
    }
	void Start () {
        toggleGroupInstance=GetComponent<ToggleGroup>();
	}
   public void onChangeToggle(bool t)
    {
        if (currentSelection.name == "lowq")
        {
            stopnja_zvok = 55;
        }
        else if (currentSelection.name == "mediumq")
        {
            stopnja_zvok = 30;
        }
        else if (currentSelection.name == "highq")
        {
            stopnja_zvok = 0;
        }
        Debug.Log(currentSelection.name);
    }
}
