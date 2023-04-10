using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
	public Canvas Tutorials;//menu with tutorials
	public Canvas Settings;//button to load settings options
	public Canvas ChooseTraining;//button to load individual training options
	public Button btnStart;//button to load fulltraining
	public Button btnlevel2;//Button to load level 2
	public Button btnlevel3;//Button to load level 3
	public Button btnlevel4;//Button to load level 4
	public Button btntut1;//button to load fulltraining
	public Button btntut2;//Button to load level 2 tutorial
	public Button btntut3;//Button to load level 3 tutorial
	public Button btntut4;//Button to load level 4 tutorial
	public Button MainMenu;//Button disabling/enabling canvas
	private Canvas MainCanvas; //Main Menu Canvas
	public Button applychanges;//button to save changes to savefile
	public AudioMixer audioMixer;
	public GetLanguage lang;//the script is loaded to use method applying changes in settings
	public SaveORLoadData save;//the script is loaded to use method applying changes in settings
	public Dropdown languagesDrop;//dropdown toolbar
	//Inputfields for settings menu
	public InputField series1;
	public InputField series2;
	public InputField series3;
	public InputField series4;
	public GameObject mark;//adds cross on AR button if disabled
	public InputField reps1;
	public InputField time2;
	public InputField reps3;
	public InputField reps4;
	public InputField breaktime1;
	public InputField breaktime2;
	public InputField breaktime3;
	public InputField breaktime4;
	private string[] languages;//language options
	[Range(0, 1)] public float SliderPercentage = 0.75f;//volume slider
	
	void Start()
	{
		
		MainCanvas = (Canvas)GetComponent<Canvas>();//Load main menu
		Tutorials = Tutorials.GetComponent<Canvas>(); //Load tutorials canvas
		Settings = Settings.GetComponent<Canvas>();// Load training settings
		ChooseTraining = ChooseTraining.GetComponent<Canvas>();//Load choose training canvas
		UpdateLanguageList();
		mark.SetActive(!(SaveORLoadData.instance.activeSave.aron)); //decides if AR is on loading info from savefile
		applychanges = applychanges.GetComponent<Button>();//Load button to apply changes in settings
		btnStart = btnStart.GetComponent<Button>();//Set load game button
		btnlevel2 = btnlevel2.GetComponent<Button>();//Set load level button
		btnlevel3 = btnlevel3.GetComponent<Button>();//Set load level button
		btnlevel4 = btnlevel4.GetComponent<Button>();//Set load level button
		btntut1 = btntut1.GetComponent<Button>();//Set load game button
		btntut2 = btntut2.GetComponent<Button>();//Set load level button
		btntut3 = btntut3.GetComponent<Button>();//Set load level button
		btntut4 = btntut4.GetComponent<Button>();//Set load level button
		MainMenu = MainMenu.GetComponent<Button>();//Set go back button

		Tutorials.enabled = false; //Hide tutorials canvas
		Settings.enabled = false;//Hide training settings canvas
		ChooseTraining.enabled = false;//Hide choose training canvas
		
	

	}
	List<string> m_DropOptions = new List<string>();//list of language options in proper order
	public void UpdateLanguageList()//puts loaded language on the first place of dropdown list
    {
		m_DropOptions.Clear();
		if (SaveORLoadData.instance.activeSave.language=="eng")
        {
			m_DropOptions.Add(GetLanguage.instance.activeVocabulary.English);
			m_DropOptions.Add(GetLanguage.instance.activeVocabulary.Polish);
		}
			else if(SaveORLoadData.instance.activeSave.language == "pl")
        {
			m_DropOptions.Add(GetLanguage.instance.activeVocabulary.Polish);
			m_DropOptions.Add(GetLanguage.instance.activeVocabulary.English);
			
		}

		languagesDrop.ClearOptions();
		languagesDrop.AddOptions(m_DropOptions);
    }
	
	public void TrainingOptions()
	{
		Settings.enabled = false;//Hide settings menu
		Tutorials.enabled = false; //Hide tutorials menu
		btnStart.enabled = false; //Deactivate "Start" button
		ChooseTraining.enabled = true;//Activate Choose Training canvas
		btnlevel2.enabled = true;//Activate Load Level Button
		btnlevel3.enabled = true;//Activate Load Level Button
		btnlevel4.enabled = true;//Activate Load Level Button
		//Deactivate tutorial buttons while showing training options
		btntut1.enabled = false;
		btntut2.enabled = false;
		btntut3.enabled = false;
		btntut4.enabled = false;
	}
	//Method called by pressing Start Full Training Button
	public void StartFullTraining()
	{
		Application.LoadLevel(1); //loads first level from build settings. "1" is the second scene

	}
	//Methods called by pressing Load Exercise Buttons:
	public void Exercise2()
	{
		Application.LoadLevel(2);//loads second level from build settings. "2" is the third scene

	}
	public void Exercise3()
	{
		Application.LoadLevel(3);//loads second level from build settings. "2" is the third scene

	}
	public void Exercise4()
	{
		Application.LoadLevel(4);//loads second level from build settings. "2" is the third scene

	}//Methods called by pressing Load tutorial buttons:
	public void Tutorial1()
	{
		Application.LoadLevel(5);//loads second level from build settings. "2" is the third scene

	}
	public void Tutorial2()
	{
		Application.LoadLevel(6);//loads second level from build settings. "2" is the third scene

	}
	public void Tutorial3()
	{
		Application.LoadLevel(7);//loads second level from build settings. "2" is the third scene

	}
	public void Tutorial4()
	{
		Application.LoadLevel(8);//loads second level from build settings. "2" is the third scene

	}
	//Method called by pressing Tutorials button

	public void LoadTutorialsList()
	{
		Settings.enabled = false;//Hide settings menu
		Tutorials.enabled = true;  //Activate Tutorials menu
		btnStart.enabled = false; //Deactivate "Start" button
		ChooseTraining.enabled = false;//Deactivate "Choose training" button
		btnlevel2.enabled = false;//Deactivate Load Level 2 button
		btnlevel3.enabled = false;//Deactivate Load Level 2 button
		btnlevel4.enabled = false;//Deactivate Load Level 2 button
		btntut1.enabled = true;
		btntut2.enabled = true;
		btntut3.enabled = true;
		btntut4.enabled = true;
	}
	//Method called by pressing GoBack Button
	public void BTNMainMenu()
	{
		Settings.enabled = false;//Hide Settings menu
		Tutorials.enabled = false; //Hide Tutorials menu
		btnStart.enabled = true; ////Activate "Start" button
		ChooseTraining.enabled = false;//Deactivate "Choose training" button
		btnlevel2.enabled = false;//Deactivate Load Level 2 button

	}
	//Method called by pressing Training Settings button
	public void LoadSettings()
	{
		Settings.enabled = true;//Activate settings menu
		Tutorials.enabled = false; //Hide Tutorials menu
		btnStart.enabled = false; //Deactivate "Start" button
		ChooseTraining.enabled = false;//Deactivate "Choose training" button
		btnlevel2.enabled = false;//Deactivate Load Level 2 button
		btnlevel3.enabled = false;//Deactivate Load Level 2 button
		btnlevel4.enabled = false;//Deactivate Load Level 2 button
		btntut1.enabled = false;
		btntut2.enabled = false;
		btntut3.enabled = false;
		btntut4.enabled = false;
		//load all information from savefiles to display for the user
		series1.text= SaveORLoadData.instance.activeSave.seriesEX1.ToString();
		series2.text = SaveORLoadData.instance.activeSave.seriesEX2.ToString();
		series3.text = SaveORLoadData.instance.activeSave.seriesEX3.ToString();
		series4.text = SaveORLoadData.instance.activeSave.seriesEX4.ToString();
		breaktime1.text = SaveORLoadData.instance.activeSave.breaktime1.ToString();
		breaktime2.text = SaveORLoadData.instance.activeSave.breaktime2.ToString();
		breaktime3.text = SaveORLoadData.instance.activeSave.breaktime3.ToString();
		breaktime4.text = SaveORLoadData.instance.activeSave.breaktime4.ToString();

		reps1.text= SaveORLoadData.instance.activeSave.repetitionsEX1.ToString();
		time2.text = SaveORLoadData.instance.activeSave.timeEX2.ToString();
	reps3.text= SaveORLoadData.instance.activeSave.repetitionsEX3.ToString();
		reps4.text = SaveORLoadData.instance.activeSave.repetitionsEX4.ToString();
		//SetDropdownBar();

	}
	public void Reload()//reloads current level
    {
		Application.LoadLevel(Application.loadedLevel);
	}
	public void ApplyChanges()//save changes made in settings menu to memory
    {

		
		if (series1.text != ""|| series1.text!=null)
			SaveORLoadData.instance.activeSave.seriesEX1 = int.Parse(series1.text);
		if (series2.text != "" || series2.text != null)
			SaveORLoadData.instance.activeSave.seriesEX2=  int.Parse(series2.text);
		if (series3.text != "" || series3.text != null)
			SaveORLoadData.instance.activeSave.seriesEX3= int.Parse(series3.text);
		if (series4.text != "" || series4.text != null)
			SaveORLoadData.instance.activeSave.seriesEX4= int.Parse(series4.text);
		if (reps1.text != "" || reps1.text != null)
			SaveORLoadData.instance.activeSave.repetitionsEX1= int.Parse(reps1.text);
		if (time2.text != "" || time2.text!= null)
			SaveORLoadData.instance.activeSave.timeEX2= int.Parse(time2.text);
		if (reps3.text != "" || reps3.text != null)
			SaveORLoadData.instance.activeSave.repetitionsEX3= int.Parse(reps3.text);
		if (reps4.text != "" || reps4.text != null)
			SaveORLoadData.instance.activeSave.repetitionsEX4= int.Parse(reps4.text);
		if (breaktime1.text != "" || breaktime1.text != null)
			SaveORLoadData.instance.activeSave.breaktime1 = int.Parse(breaktime1.text);
		if (breaktime2.text != "" || breaktime2.text != null)
			SaveORLoadData.instance.activeSave.breaktime2 = int.Parse(breaktime2.text);
		if (breaktime3.text != "" || breaktime3.text != null)
			SaveORLoadData.instance.activeSave.breaktime3 = int.Parse(breaktime3.text);
		if (breaktime4.text != "" || breaktime4.text != null)
			SaveORLoadData.instance.activeSave.breaktime4 = int.Parse(breaktime4.text);


	}
	public Slider slider;
	public void Awake()//get information from memory to slidebar
    {
		if (!PlayerPrefs.HasKey("Volume"))
			PlayerPrefs.SetFloat("Volume", SliderPercentage);
		slider.onValueChanged.AddListener(SetVolume);
		slider.value = PlayerPrefs.GetFloat("Volume");
    }
	public void SetVolume(float volume)//save user preferences to memory
	{

		audioMixer.SetFloat("Volume", Mathf.Clamp(Mathf.Log10(volume/SliderPercentage)*170f, -80f,20f));
		PlayerPrefs.SetFloat("Volume",volume);
		PlayerPrefs.Save();
		
	}

	
	public void SetLanguage(Dropdown languagesDrop)//the continuation of Update language list method assigning language options to dropdown bar
	{
		
		if (languagesDrop.value == 0)
        {
			if(m_DropOptions[0]== GetLanguage.instance.activeVocabulary.English)
				SaveORLoadData.instance.activeSave.language = "eng";
			else if(m_DropOptions[0] == GetLanguage.instance.activeVocabulary.Polish)
				SaveORLoadData.instance.activeSave.language = "pl";
		}
			
		else if(languagesDrop.value == 1)
        {
			if (m_DropOptions[1] == GetLanguage.instance.activeVocabulary.English)
				SaveORLoadData.instance.activeSave.language = "eng";
			else if (m_DropOptions[1] == GetLanguage.instance.activeVocabulary.Polish)
				SaveORLoadData.instance.activeSave.language = "pl";
		}
		lang.LoadLanguage();//reload new language
		save.ApplyChanges();//save information from the memory to savefile
		UpdateLanguageList();
	}
	public Button aron;
	public void SetAR()//decide if the ar feature is enabled and save to local memory and savefile
    {
		if (SaveORLoadData.instance.activeSave.aron)
        {
			SaveORLoadData.instance.activeSave.aron = false;
			mark.SetActive(true);
        }
		else
        {
			SaveORLoadData.instance.activeSave.aron = true;
			mark.SetActive(false);
		}
		save.ApplyChanges();//save information from the memory to savefile
			
	}
}
