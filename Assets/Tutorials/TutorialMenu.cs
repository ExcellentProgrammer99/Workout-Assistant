using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialMenu : MonoBehaviour
{
   public GameObject ARSession;
   public GameObject origin;
   public SaveORLoadData save;
    //public GameObject Menu;
    public Text text;
    public bool arOn;
    // Start is called before the first frame update
    void Start()
    {
        arOn = SaveORLoadData.instance.activeSave.aron;
        Debug.Log(arOn);
        Debug.Log(SaveORLoadData.instance.activeSave.aron);
        Loadtxt();
    }
    public void Loadtxt()
    {
        if(Application.loadedLevelName=="Tutorial1")
        {
            text.text = GetLanguage.instance.activeVocabulary.TutorialLateralRaises;
        }
        else if (Application.loadedLevelName == "Tutorial2")
        {
            text.text = GetLanguage.instance.activeVocabulary.TutorialHandstand;
        }
        else if (Application.loadedLevelName == "Tutorial3")
        {
            
            text.text = GetLanguage.instance.activeVocabulary.TutorialOverheadPress;
        }
        else if (Application.loadedLevelName == "Tutorial4")
        {
            text.text = GetLanguage.instance.activeVocabulary.TutorialCurls;
        }
      
    }
    public void GoToMainMenu()
    {
        Application.LoadLevel(0);
    }
    void Update()
    {
        if (arOn)
        {
           
            ARSession.SetActive(true);
            origin.SetActive(true);
        }
        else
        {
            ARSession.SetActive(false);
            origin.SetActive(false);
        }
    }
    public void TurnARONOFF()
    {
        switch (arOn)
        {
            case true:
                    arOn = false;
                break;
                
            default:
                    arOn = true;
                    break;
        }
        SaveORLoadData.instance.activeSave.aron =arOn;
        save.ApplyChanges();
        Reload();
    }
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
