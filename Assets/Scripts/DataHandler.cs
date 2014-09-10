using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class DataHandler : MonoBehaviour {

	#region xmlDataStructure
	[XmlRoot("beerTimes")]
	public class BeerTimes {
		[XmlElement("fastestTime")]
		public float fastestTime;
		[XmlElement("user")]
		public User[] user;
	}
	public class User {
		[XmlElement("name")]
		public string name;
		[XmlElement("personalBest")]
		public float personalBest;
		[XmlElement("sex")]
		public string sex;
		[XmlElement("dates")]
		public Dates[] dates;
	}
	public class Dates {
		[XmlElement("date")]
		public string date;
		[XmlElement("drinkingTimes")]
		public DrinkTime[] drinkTimes;
	}
	public class DrinkTime {
		[XmlElement("approved")]
		public bool approved; 
		[XmlElement("drinkTime")]
		public float drinkingTime;
	}
	#endregion


	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		loadXmlData();
	}
	
	// Update is called once per frame
	void Update () {
		handleArdData();	
	}

	#region ArduinoDataHandler
	void handleArdData()
	{
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
	#endregion

	#region xmlDataHandler
	void loadXmlData(){
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(BeerTimes));
		var stream = File.Open("Assets/xmlFiles/beerTimes.xml", FileMode.Open);
		BeerTimes beerTimer = (BeerTimes)xmlSerializer.Deserialize(stream);
	}
	void saveXmlData(){

	}
	#endregion
}
