using UnityEngine;
using System.Collections;

public class GuiHandler : MonoBehaviour {

	public GUIText timer;

	public GUITexture background;
	private int oldScrW, oldScrH;

	public bool updateTimeStamp,running,newTime;
	private string tempTimerText;
	public int h,m,s,ms;
	private float startTime;

    // Use this for initialization
	void Start () {
		updateTimeStamp = true;
		timer.text = "00:00:00,00";
		setGuiBackground();
	}
	
	// Update is called once per frame
	void Update () {



	}

    void OnGUI() {
        //Updates time when new precise time is passed
        setGuiBackground();
		setGuiTimer();

        guiButtons();
    }

    void guiButtons() {

        //Add User Button
        if (GUI.Button(new Rect(10, 10, 100, 50), "Add User")) {

        }

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

			timer.color = new Color(1.0f,1.0f,0.0f);
			timer.text = tempTimerText;
		}
		if(newTime){
			if(h < 10) tempTimerText = "0" + h.ToString() + ":"; else tempTimerText = h.ToString() + ":";
			if(m < 10) tempTimerText += "0" + m.ToString() + ":"; else tempTimerText += m.ToString() + ":";
			if(s < 10) tempTimerText += "0" + s.ToString() + ","; else tempTimerText += s.ToString() + ",";
			if(ms < 10) tempTimerText += "0" + ms.ToString(); else tempTimerText += ms.ToString();
			
			timer.color = new Color(0.0f,1.0f,0.0f);
			timer.text = tempTimerText;

			newTime = false;
			running = false;
			updateTimeStamp = true;
		}
	}

	void setGuiBackground(){
		if(oldScrH != Screen.height || oldScrW != Screen.width){
			float sX,sY,sZ;
			sX = ((float)(Screen.width/16) / (float)(Screen.height/9) > 1)?1.0f:(float)(Screen.height/9 / (float)(Screen.width/16));
			sY = ((float) (Screen.width/16) / (float) (Screen.height/9) > 1)?(float)(Screen.width/16) / (float)(Screen.height/9):1.0f;
			sZ = 1.0f;
			
			background.transform.localScale = new Vector3(sX,sY,sZ);
			oldScrH = Screen.height;
			oldScrW = Screen.width;
		}
	}
}