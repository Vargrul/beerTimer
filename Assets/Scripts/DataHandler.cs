using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Globalization;

public class DataHandler : MonoBehaviour {

    #region xmlDataStructure
    [XmlRoot("beerTimes")]
    public class BeerTimes {
        [XmlElement("fastestTime")]
        public float fastestTime;
        [XmlElement("rankIndex")]
        public List<int> rankIndex;
        [XmlElement("user")]
        public List<User> user;
    }
    public class User {
        [XmlElement("name")]
        public string name;
        [XmlElement("personalBest")]
        public float personalBest;
        [XmlElement("gender")]
        public E_GENDER gender;
        [XmlElement("dates")]
        public List<Dates> dates;

        public User() { }
        public User(string inN, E_GENDER inGen) {
            name = inN;
            personalBest = 0.0f;
            gender = inGen;
            dates = new List<Dates>();
        }
    }
    public class Dates {
        [XmlIgnore]
        public DateTime date;
        [XmlElement("date")]
        public string proxyDate {
            get { return date.ToString("dd-MM-yyyy"); }
            set { date = DateTime.Parse(value, CultureInfo.CreateSpecificCulture("de-DE")); }
        }
        [XmlElement("drinkingTimes")]
        public List<DrinkTime> drinkingTimes;

        public Dates() { }
        public Dates(DateTime inD) {
            date = inD;
            drinkingTimes = new List<DrinkTime>();
        }
    }
    public class DrinkTime {
        [XmlElement("approved")]
        public bool approved;
        [XmlElement("drinkTime")]
        public float drinkingTime;

        public DrinkTime() { }
        public DrinkTime(bool inAp, float inTime) {
            approved = inAp;
            drinkingTime = inTime;
        }
    }
    #endregion

    private BeerTimes timeDatabase;

    // Use this for initialization
    void Start() {
        Application.runInBackground = true;
        loadXmlData();

        generateRankIndex();
    }

    // Update is called once per frame
    void Update() {
        handleArdData();
    }

    #region arduinoDataHandler
    void handleArdData() {
        var ardScript = GetComponent<ArduinoDataReceiver>();
        var guiScript = GetComponent<GuiHandler>();

        List<string> incommingData = ardScript.receivedData;

        if (incommingData.Count != 0) {

            //checking for a number else some kind of text
            if ((int)incommingData[0][0] > 47 && (int)incommingData[0][0] < 58) {

                Debug.Log("it was a number");
                guiScript.h = Convert.ToInt32(incommingData[0]);
                guiScript.m = Convert.ToInt32(incommingData[1]);
                guiScript.s = Convert.ToInt32(incommingData[2]);
                guiScript.ms = Convert.ToInt32(incommingData[3]);

                guiScript.newTime = true;

                incommingData.Clear();
            } else {

                Debug.Log("It was some text");

                switch (incommingData[0]) {
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

    #region databaseHandlers
    void addUser(string inN, E_GENDER inS) {

        User nUser = new User(inN,inS);
        timeDatabase.user.Add(nUser);

        saveXmlData();
    }

    void addTime(int uNm, float inT, bool inAp) {

        if (timeDatabase.user[uNm].dates.Count == 0) {
            timeDatabase.user[uNm].dates.Add(new Dates(DateTime.Now));
            timeDatabase.user[uNm].dates[timeDatabase.user[uNm].dates.Count - 1].drinkingTimes.Add(new DrinkTime(inAp, inT));
        } else if (DateTime.Now.Date == timeDatabase.user[uNm].dates[timeDatabase.user[uNm].dates.Count - 1].date.Date) {
            timeDatabase.user[uNm].dates[timeDatabase.user[uNm].dates.Count - 1].drinkingTimes.Add(new DrinkTime(inAp, inT));
        } else {
            timeDatabase.user[uNm].dates.Add(new Dates(DateTime.Now));
            timeDatabase.user[uNm].dates[timeDatabase.user[uNm].dates.Count - 1].drinkingTimes.Add(new DrinkTime(inAp, inT));
        }

        if (inT < timeDatabase.user[uNm].personalBest || timeDatabase.user[uNm].personalBest == 0.0f) {
            timeDatabase.user[uNm].personalBest = inT;
            addInRankIndex(inT, uNm);
        }

        saveXmlData();
    }

    void generateRankIndex() {
        List<int> nRankIndex = new List<int>();

        for (int i = 0; i < timeDatabase.user.Count; i++) {
            if (nRankIndex.Count == 0) {
                nRankIndex.Add(i);
            } else {
                for (int j = 0; j < nRankIndex.Count; j++) {
                    if (j == 0) {
                        if (timeDatabase.user[i].personalBest <= timeDatabase.user[nRankIndex[j]].personalBest) {
                            nRankIndex.Insert(j, i);
                            break;
                        }
                    } else if (timeDatabase.user[i].personalBest > timeDatabase.user[nRankIndex[j - 1]].personalBest && timeDatabase.user[i].personalBest <= timeDatabase.user[nRankIndex[j]].personalBest) {
                        nRankIndex.Insert(j, i);
                        break;
                    }
                }
            }
        }
        timeDatabase.rankIndex = nRankIndex;
    }

    void addInRankIndex(float nTime, int uNm) {

        for (int ele = 0; ele < timeDatabase.rankIndex.Count; ele++) {
            if (timeDatabase.rankIndex.Contains(uNm)) {
                timeDatabase.rankIndex.Remove(uNm);
            }
        }

        if (timeDatabase.rankIndex.Count == 0) {
            timeDatabase.rankIndex.Add(uNm);
        } else {
            for (int j = 0; j < timeDatabase.rankIndex.Count; j++) {
                if (j == 0) {
                    if (timeDatabase.user[uNm].personalBest <= timeDatabase.user[timeDatabase.rankIndex[j]].personalBest) {
                        timeDatabase.rankIndex.Insert(j, uNm);
                        return;
                    }
                } else if (timeDatabase.user[uNm].personalBest > timeDatabase.user[timeDatabase.rankIndex[j - 1]].personalBest && timeDatabase.user[uNm].personalBest <= timeDatabase.user[timeDatabase.rankIndex[j]].personalBest) {
                    timeDatabase.rankIndex.Insert(j, uNm);
                    return;
                }
            }
        }
    }
    #endregion

    #region xmlDataHandlers
    void loadXmlData() {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(BeerTimes));
        var stream = File.Open("Assets/xmlFiles/beerTimes.xml", FileMode.Open);
        timeDatabase = (BeerTimes)xmlSerializer.Deserialize(stream);
        stream.Close();
    }

    void saveXmlData() {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(BeerTimes));
        var stream = File.Open("Assets/xmlFiles/beerTimes.xml", FileMode.OpenOrCreate);
        xmlSerializer.Serialize(stream, timeDatabase);
        stream.Close();
    }
    #endregion
}