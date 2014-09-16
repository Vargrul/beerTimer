using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiHandler : MonoBehaviour {

	private int oldScrW, oldScrH;

	public bool updateTimeStamp,running,newTime;
	private string tempTimerText;
	public int h,m,s,ms;
	private float startTime;


    //UI Elements
    public Text timerText;
    public Button addUser;


    // Use this for initialization
	void Start () {
		updateTimeStamp = true;
        //timerText = GameObject.FindGameObjectWithTag("timer").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI() {
        //Updates time when new precise time is passed
		setGuiTimer();
    }

    public void testClick() {
        Debug.Log("clicked");
    }

	void setGuiTimer(){
		if(running && !newTime){
			if(updateTimeStamp){
				startTime = Time.time;
				updateTimeStamp = false;
			}

			h = (int)(Time.time - startTime)/3600;
			m = ((int)(Time.time - startTime)/60)-h*60;
			s = (int)(Time.time - startTime)%60;
			ms = (int)(((Time.time - startTime)-(s+(m*60)+(h*3600))) * 100);

			if(h < 10) tempTimerText = "0" + h.ToString() + ":"; else tempTimerText = h.ToString() + ":";
			if(m < 10) tempTimerText += "0" + m.ToString() + ":"; else tempTimerText += m.ToString() + ":";
			if(s < 10) tempTimerText += "0" + s.ToString() + ","; else tempTimerText += s.ToString() + ",";
			if(ms < 10) tempTimerText += "0" + ms.ToString(); else tempTimerText += ms.ToString();

			timerText.color = new Color(1.0f,1.0f,0.0f);
            timerText.text = tempTimerText;
		}
		if(newTime){
			if(h < 10) tempTimerText = "0" + h.ToString() + ":"; else tempTimerText = h.ToString() + ":";
			if(m < 10) tempTimerText += "0" + m.ToString() + ":"; else tempTimerText += m.ToString() + ":";
			if(s < 10) tempTimerText += "0" + s.ToString() + ","; else tempTimerText += s.ToString() + ",";
			if(ms < 10) tempTimerText += "0" + ms.ToString(); else tempTimerText += ms.ToString();

            timerText.color = new Color(0.0f,1.0f,0.0f);
            timerText.text = tempTimerText;

			newTime = false;
			running = false;
			updateTimeStamp = true;
		}
	}
}