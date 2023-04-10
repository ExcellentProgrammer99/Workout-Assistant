using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;//load xml library to save data and store in the file in phone appdata
using System.Xml.Serialization;

public class SaveORLoadData : MonoBehaviour
{
    public static SaveORLoadData instance;//instance for 
    public SaveData activeSave;//separate active class with data
    private string dataPath;
    private void Awake()//instance to save/load data in all possible scripts
    {
        instance = this;
       
    }
    void Start()
    {
        
        dataPath = Application.persistentDataPath + "/SaveGame.save";//save in application data memory (mobile phones build project differently than PC, so the data can't be stored in project folder)
        //SetDefault(); //used for resetting highscores in debug mode
       // ApplyChanges();
        LoadData();//load data from file
        
    }
  
    void Save()//save data to xml file
    {
        
        
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath, FileMode.Create);
            serializer.Serialize(stream, activeSave);
            stream.Close();
        
    }
    void SetDefault()//if there is no data, the basic settings are loaded
    {
        
        activeSave.repetitionsEX1 = 15;
        activeSave.seriesEX1 = 3;
        activeSave.timeEX2 = 20;
        activeSave.seriesEX2 = 1;
        activeSave.repetitionsEX3 = 15;
        activeSave.seriesEX3 = 3;
        activeSave.repetitionsEX4 = 15;
        activeSave.seriesEX4 = 3;
        activeSave.language = "eng";
        activeSave.highscore1 = int.MaxValue;//max to set any new highscore in the shortest time of perfomed exercise
        activeSave.highscore2 = false;//if the exercise was done
        activeSave.highscore3 = int.MaxValue;
        activeSave.highscore4 = int.MaxValue;
        activeSave.breaktime1 =120;
        activeSave.breaktime2 =120;
        activeSave.breaktime3 =120;
        activeSave.breaktime4 =120;
        activeSave.aron =true;
    }
    void LoadData()
    {
       
        if (System.IO.File.Exists(dataPath))//load data from file
        {
            Debug.Log("Gotee");
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath, FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();
            Debug.Log("Loaded!");
        }
        else
        {
          
            SetDefault();
            Save();
        }
    }
   
    public void ApplyChanges()
    {
        Save();
    }
   
    
}

    [System.Serializable]//store in memory
public class SaveData
    {
        //store information about amount of repetitions and series
        public int repetitionsEX1;
        public int seriesEX1;
        public int timeEX2;
        public int seriesEX2;
        public int repetitionsEX3;
        public int seriesEX3;
        public int repetitionsEX4;
        public int seriesEX4;
    //store local highscores
    public int highscore1;
    public bool highscore2;//if the exercise was done
    public int highscore3;
    public int highscore4;
    //store chosen language
    public string language;
    //store break times between series
    public int breaktime1;
    public int breaktime2;
    public int breaktime3;
    public int breaktime4;
    //store information if the user prefers to use AR mode
    public bool aron;
    }