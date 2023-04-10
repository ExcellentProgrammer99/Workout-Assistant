using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
public class TranslateLanguage : MonoBehaviour//class used for designing custom xml file; disabled in the project
{
    public static TranslateLanguage instance;
    public VocabularyToSave activeVocabulary;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        activeVocabulary.Workoutassistant = "Asystent ćwiczeń";
        activeVocabulary.StartFullTraining = "Zacznij pełny trening";
        activeVocabulary.Tutorials = "Tutoriale";
        activeVocabulary.ChooseTraining = "Wybierz Trening";
        activeVocabulary.Settings = "Ustawienia";
        activeVocabulary.Time = "Czas";
        activeVocabulary.Repetitions = "Powtórzenie";
        activeVocabulary.Walkaway = "Odejdź dalej od ekranu i stań w ramce";
        activeVocabulary.Congratulations = "Gratulacje, udało Ci się wykonać ćwiczenie w ";
        activeVocabulary.LanguageSettings = "Ustaw język";
        activeVocabulary.Apply = "Zastosuj";
        activeVocabulary.Lateralraises = "Podciąganie hantli do boku";
        activeVocabulary.Handstand = "Stanie na rękach";
        activeVocabulary.verheadpress = "Wznoszenie hantli nad głowę";
        activeVocabulary.Curls = "Uginanie ramion";
        activeVocabulary.Othersettings = "Inne ustawienia";
        activeVocabulary.Volume = "Dźwięk";
        activeVocabulary.NumberofSeries = "Liczba serii";
        activeVocabulary.Numberofrepetitions = "Liczba powtórzeń";
        activeVocabulary.English = "Angielski";
        activeVocabulary.Polish = "Polski";
        activeVocabulary.Enter = "Dodaj";
        Debug.Log("Saved");
        string dataPath = Application.dataPath;
        var serializer = new XmlSerializer(typeof(VocabularyToSave));
        var stream = new FileStream(dataPath + "templ.lang", FileMode.Create);
        serializer.Serialize(stream, activeVocabulary);
        stream.Close();
    }

    
}
[System.Serializable]
public class VocabularyToSave
{

    public string Workoutassistant;
    public string StartFullTraining;
    public string Tutorials;
    public string ChooseTraining;
    public string Settings;
    public string Time;
    public string Repetitions;
    public string Walkaway;
    public string Congratulations;
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
    public string Enter;
}