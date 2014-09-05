using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DataHandler : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var ardScript = GetComponent<ArduinoDataReceiver>();
		var guiScript = GetComponent<GuiHandler>();

		List<string> incommingData = ardScript.receivedData;

		if(incommingData.Count != 0){

			//checking for a number else some kind of text
			if((int)incommingData[0][0] > 47 && (int)incommingData[0][0] < 58){

				Debug.Log ("it was a number");
				guiScript.h = Convert.ToInt32(incommingData[0]);
				guiScript.m = Convert.ToInt32(incommingData[1]);
				guiScript.s = Convert.ToInt32(incommingData[2]);
				guiScript.ms = Convert.ToInt32(incommingData[3]);

				guiScript.newTime = true;

				incommingData.Clear();
			}else{

				Debug.Log ("It was some text");

				switch (incommingData[0]){
					//Start a GUI Clock
					case "start":
						guiScript.running = true;
					break;

					//Stop a GUI Clock
					case "stop":
						guiScript.running = false;
					break;
				}
			}
		}
	
	}
}
