using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxtGetStr : MonoBehaviour {

    public string message = "";

    void Start () {
		
	}
	
	void Update () {
        this.GetComponent<Text>().text = message;
    }





}
