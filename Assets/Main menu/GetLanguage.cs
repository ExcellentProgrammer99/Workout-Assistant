using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class GetLanguage : MonoBehaviour//class used to load language strings from xml file and store as temporary memory instance
{
    public static GetLanguage instance;
    public Vocabulary activeVocabulary;
    private string languageactive;
    private void Awake()//store in realtime
    {
        instance = this;
        
    }
   
    void Start()
    {
        LoadData();
    }
    
    void LoadData()//load xml file
    {
        languageactive = SaveORLoadData.instance.activeSave.language;
        string dataPath = Application.streamingAssetsPath;
        
            var serializer = new XmlSerializer(typeof(Vocabulary));
            var stream = new StreamReader(File.OpenRead(dataPath + "/" + SaveORLoadData.instance.activeSave.language + ".lang"));//check chosen language and load propriate file

            activeVocabulary = serializer.Deserialize(stream) as Vocabulary;
            stream.Close();
            Debug.Log("Language Loaded!");
        
    }
    
    
    public void LoadLanguage()//after changing language-reloads the vocabulary instance strings
    {
        LoadData();
    }
}

[System.Serializable]//save information to tmp memory-disappears after closing the application
public class Vocabulary//store data from file in memory to translate the strings
{
    //strings with language translations used after loading the proper language pack 
    public string Workoutassistant;
    public string StartFullTraining;
    public string Tutorials;
    public string ChooseTraining;
    public string Settings;
    public string Time;
    public string Repetitions;
    public string Walkaway;
    public string Congratulations;
    public string MistakesMade;
    public string Highscorebeaten;
    public string ExerciseTime;
    public string LanguageSettings;
    public string Apply;
    public string Lateralraises;
    public string Handstand;
    public string verheadpress;
    public string Curls;
    public string Othersettings;
    public string Volume;
    public string NumberofSeries;
    public string Numberofrepetitions;
    public string English;
    public string Polish;
    public string GetPosture;
    public string Enter;
    public string TooFast;
    public string KeepYourHeadStill;
    public string KeepYourSpineStraight;
    public string FeetOnHipWidth;
    public string AbdominalContracted;
    public string CorrectWristPosition;
    public string KeepYourArmsStraight;
    public string KeepYourElbowsStraight;
    public string LiftSymmetrical;
    public string TutorialHandstand;
    public string TutorialLateralRaises;
    public string TutorialOverheadPress;
    public string TutorialCurls;
    public string HandstandTip;
    public string Motivation;
    public string Breaktime;
}