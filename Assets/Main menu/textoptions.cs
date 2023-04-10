using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class textoptions : MonoBehaviour
{
	//language text strings for Main Menu
	public Text tim;
	public Text srs1;
	public Text rps1;
	public Text srs2;
	public Text srs3;
	public Text rps3;
	public Text srs4;
	public Text rps4;
	
	public Text ex1;
	public Text ex2;
	public Text ex3;
	public Text ex4;
	public Text ex1_1;
	public Text ex2_1;
	public Text ex3_1;
	public Text ex4_1;
	public Text ex1_2;
	public Text ex2_2;
	public Text ex3_2;
	public Text ex4_2;

	public Text breaktext1;
	public Text breaktext2;
	public Text breaktext3;
	public Text breaktext4;

	public Text appl;
	public Text Othersettings;
	public Text vol;
	public Text title;
	public Text start;
	public Text tuts;
	public Text settings;
	public Text choose;
	public Text setlang;
	private string languageactive;//stores actual language version
	

    // Update is called once per frame
    void Update()
    {
		//get actual language info from savefile
		languageactive = SaveORLoadData.instance.activeSave.language;
		//get translated strings from language files
		tim.text = GetLanguage.instance.activeVocabulary.Time;
		srs1.text = GetLanguage.instance.activeVocabulary.NumberofSeries;
		rps1.text = GetLanguage.instance.activeVocabulary.Repetitions;
		srs2.text = GetLanguage.instance.activeVocabulary.NumberofSeries;
		rps3.text = GetLanguage.instance.activeVocabulary.Repetitions;
		srs3.text = GetLanguage.instance.activeVocabulary.NumberofSeries;
		rps4.text = GetLanguage.instance.activeVocabulary.Repetitions;
		srs4.text = GetLanguage.instance.activeVocabulary.NumberofSeries;

		ex1_1.text = GetLanguage.instance.activeVocabulary.Lateralraises;
		ex2_1.text = GetLanguage.instance.activeVocabulary.Handstand;
		ex3_1.text = GetLanguage.instance.activeVocabulary.verheadpress;
		ex4_1.text = GetLanguage.instance.activeVocabulary.Curls;

		ex1_2.text = GetLanguage.instance.activeVocabulary.Lateralraises;
		ex2_2.text = GetLanguage.instance.activeVocabulary.Handstand;
		ex3_2.text = GetLanguage.instance.activeVocabulary.verheadpress;
		ex4_2.text = GetLanguage.instance.activeVocabulary.Curls;

		ex1.text = GetLanguage.instance.activeVocabulary.Lateralraises;
		ex2.text = GetLanguage.instance.activeVocabulary.Handstand;
		ex3.text = GetLanguage.instance.activeVocabulary.verheadpress;
		ex4.text = GetLanguage.instance.activeVocabulary.Curls;

		breaktext1.text = GetLanguage.instance.activeVocabulary.Breaktime;
		breaktext2.text = GetLanguage.instance.activeVocabulary.Breaktime;
		breaktext3.text = GetLanguage.instance.activeVocabulary.Breaktime;
		breaktext4.text = GetLanguage.instance.activeVocabulary.Breaktime;
		
		appl.text = GetLanguage.instance.activeVocabulary.Apply;
		setlang.text = GetLanguage.instance.activeVocabulary.LanguageSettings;
		Othersettings.text = GetLanguage.instance.activeVocabulary.Othersettings;
		vol.text = GetLanguage.instance.activeVocabulary.Volume;
		title.text = GetLanguage.instance.activeVocabulary.Workoutassistant;
		start.text = GetLanguage.instance.activeVocabulary.StartFullTraining;
		tuts.text = GetLanguage.instance.activeVocabulary.Tutorials;
		settings.text = GetLanguage.instance.activeVocabulary.Settings;
		choose.text = GetLanguage.instance.activeVocabulary.ChooseTraining;

	}
}
