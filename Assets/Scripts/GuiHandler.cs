using UnityEngine;
using System.Collections;

public class GuiHandler : MonoBehaviour {

	public GUIText timer;
	public GUITexture background;
	public bool running,newTime;
	public int h,m,s,ms;

	// Use this for initialization
	void Start () {
		timer.text = "00:00:00,00";
		setGuiBackground();
	}
	
	// Update is called once per frame
	void Update () {

		//Updates time when new precise time is passed
		if(newTime)	setGuiTimer();
		setGuiBackground();

	}

	void setGuiTimer(){
		string tempTimerText;
		
		if(h < 10) tempTimerText = "0" + h.ToString() + ":"; else tempTimerText = h.ToString() + ":";
		if(m < 10) tempTimerText += "0" + m.ToString() + ":"; else tempTimerText += m.ToString() + ":";
		if(s < 10) tempTimerText += "0" + s.ToString() + ","; else tempTimerText += s.ToString() + ",";
		if(ms < 10) tempTimerText += "0" + ms.ToString(); else tempTimerText += ms.ToString();
		
		timer.color = new Color(0.0f,1.0f,0.0f);
		timer.text = tempTimerText;
	}

	void setGuiBackground(){
		
		//background.pixelInset = (float) Screen.height;
	}
}
