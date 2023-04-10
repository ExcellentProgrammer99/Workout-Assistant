using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class Handstand : MonoBehaviour//approach of determining handstand exercise assessment model, unfortunately the learning model is unable
    //to detect users upside-down; rotating rawImage didn't work
{

    public float count;//counter for current standtime
    public Text countText;//counter displaying current standtime on the screen
    public static float standtime;//amount of time to handstand taken from requirements and converted to float for better time handling
    public static int totalseries;//number of total amount of series to perform taken from requirements


    public ExerciseControl sample;//load to use control methods
    public SaveORLoadData save;//load script to modify the highscores
    public Text timetext;//shows current time on screen
   public List<string> mistakes;//list of mistakes made by the user
    bool breakenabled;//returns if user has currently a break between series
    private float timer;//time of exercise
    public Image img;//breathing dot image
    public int series;//stores actual series amount
    // Start is called before the first frame update
    void Start()
    {
        standtime = (float)SaveORLoadData.instance.activeSave.timeEX2;
        totalseries = SaveORLoadData.instance.activeSave.seriesEX2;
        //assign beginning values for time/count/series and load requirements from settings
        breathin = true;
        starting = true;
        mistakes = new List<string>();
        Time.timeScale = 1;
        timer = 0;
        series = 1;
        count = 0;


        breakenabled = false;
        InvokeRepeating("Breath", 0f, 0.1f);//invoke method (background worker) to change position of breathing dor ever 0.1sec
    }
   
    // Update is called once per frame
    void Update()
    {
        PostureCorrection();
        DoExercise();
        if(!breakenabled)//prevents counting the time when the user has a break
            timer += Time.deltaTime;
        timetext.text = GetLanguage.instance.activeVocabulary.Time + ": " + timer.ToString("F0") + "s";
        countText.text = GetLanguage.instance.activeVocabulary.Time + ": " + count.ToString("F0") + "/" + standtime.ToString();

       // Debug.Log("X:" + sample.SearchResult("LEFT_WRIST","x"));
      //  Debug.Log("Y:"+sample.SearchResult("LEFT_WRIST", "y"));
    }
   
    bool DoExercise()//Method for checking if the body is upside-down and measuring time
    {
        if (count >= standtime)//checks if user managed to the end of challenge
        {
           
            if (totalseries == series)
            {
                Time.timeScale = 0;//stop time to save the possible highscore
                ExerciseCompleted();//go to final canvas

            }
            else
            {
                breakenabled = sample.DisplayBreakScreen();//go to breakscreen and set breaktime active
                if (breakenabled == false)//after deactivation set counter to 0 and go to next series (if there are any)
                {
                    count = 0;
                    series++;
                    Debug.Log(series);
                }
            }

            
            if (breakenabled)
                return true;//ignore the rest of method in case of breakout canvas enabled to prevent errors
        }
        bool inframe = sample.PlayerIsInFrame();
        if (inframe)//to prevent errors, the algorithm works only when the player is in the frame
        {
            //sample.performHandstand(true);
            //if nose is lower than any hip means the player is upside down
            if (sample.SearchResult("NOSE", "y") < sample.SearchResult("LEFT_HIP", "y")||sample.SearchResult("NOSE", "y") < sample.SearchResult("RIGHT_HIP", "y"))
                count += Time.deltaTime;
            else
            {
                if (count > 0)//if the user fails before finishing the challenge, the counter sets to 0
                {
                    count = 0;
                    sample.MessagePopUp(GetLanguage.instance.activeVocabulary.HandstandTip);
                }
                    
            }
            
            if( count > standtime / 2)//if the player is about to finish, the proper motivational quote is displayed
            {
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.Motivation);
                FreezeTime();
            }
                
        }
        return false;
    }
    
    
    
    public bool Highscorebeaten()//checks if the user lasted for full training and saves data to savefile
    {
        if (timer <= SaveORLoadData.instance.activeSave.highscore1)
        {
            SaveORLoadData.instance.activeSave.highscore2 = true;
            return true;
        }
        return false;

    }
    
    
     
    void PostureCorrection()//Method used for explaining mistakes in realtime via popupmessages and voice sounds, method does not affect the training itself
    {//Method also writes mistakes on the list which is displayed on final screen due to possibilities of voice errors and as reminder
        //Posture Correction in handstand provides better handling, but is reserved to professional athletes (beginners learn to walk on hands first)
        //due to problems with stabilization, the idea behind application is to be for everyone, so it does not expect proper stabilization
        //and is satisfied just by keeping the body upside-down
        bool inframe = sample.PlayerIsInFrame();
        if (inframe)//posture correction works only if the player is in the frame to minimize effects of errors while player is away
        {
            //Method checks if the legs stay connected to keep proper balance of the body
            //to help in body stabilization it is recommended to keep stomach, back and buttocks contracted during performance of exercise
            if (!sample.PositionsRelation("LEFT_ANKLE", "LEFT_HIP", "x", 2f, null) || !sample.PositionsRelation("RIGHT_ANKLE", "RIGHT_HIP", "x", 2f, null))
            {
                sample.PlayPopUpSound("AbdominalContractedSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.AbdominalContracted);
                FreezeTime();
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.AbdominalContracted))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.AbdominalContracted);
            }//The main aspect of handstand stabilization relies on wrist and shoulder correction, so when the application detects
            //destabilization, muscle trembling, reminds to correct arm positioning
            if (!sample.PositionsRelation("RIGHT_SHOULDER", "LEFT_SHOULDER", "y", 3f,  null) || !sample.PositionsRelation("LEFT_HIP", "RIGHT_HIP", "y", 3f, null))
            {
                sample.PlayPopUpSound("CorrectWristPositionSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.CorrectWristPosition);
                FreezeTime();
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.CorrectWristPosition))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.CorrectWristPosition);
            }//reminds about proper shoulder position and correction
            if (!sample.PositionsRelation("RIGHT_ELBOW", "LEFT_WRIST", "X", 4f,  null) || !sample.PositionsRelation("LEFT_ELBOW", "LEFT_WRIST", "y", 4f,  null))
            {
                sample.PlayPopUpSound("KeepYourArmsStraightSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.KeepYourArmsStraight);
                FreezeTime();
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.KeepYourArmsStraight))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.KeepYourArmsStraight);
            }
            
        }
    }


    bool beaten;
    void ExerciseCompleted()//Method calls main function in control folder and finish the task of script
    {
        if (!beaten)
            beaten = Highscorebeaten();
        //Debug.Log(beaten);
        string mistakestodisplay = string.Join("\n", mistakes.ToArray());

        sample.DisplayFinalScreen(timer, mistakestodisplay, beaten);
    }

   
    
    private IEnumerator FreezeTime()//waits to the end of voice sentence
    {
        Time.timeScale = 0;
        yield return new WaitForSeconds(3);
        Time.timeScale = 1;
    }
    bool breathin;//used for switching between breath-in and breath-out simulation
    bool starting;//used for setting starting blue rgba coordinates
    byte currentred;
    byte currentblue;
    byte currentgreen;
    float currentsize;
    private void Breath()//move invoked dot to simulate proper breathing
    {
        //Debug.Log("Stillrunning");
        if (breathin)
        {
            if (starting)//shows smaller blue dot
            {
                currentred = 0;
                currentblue = 191;
                currentgreen = 255;
                currentsize = 100;
                starting = false;
            }
            img.GetComponent<RectTransform>().sizeDelta = new Vector2((int)currentsize, (int)currentsize);//set image size
            img.color = new Color32(currentred, currentblue, currentgreen, 255);//set image color


            //currentred -= 0.44f;
            //currentblue += 0.342f;
            //currentgreen += 0.39f;
            currentsize += 1.92f;//change size of blue dot to simulate breath in
            if (currentsize >= 200f)//until 0,191,255,255-crimson
                breathin = false;

        }
        else//minimize the fully-grown dot and change color to crimson to simulate breath out
        {
            img.color = new Color32(220, 20, 60, 255);//change color to crimson
            img.GetComponent<RectTransform>().sizeDelta = new Vector2((int)currentsize, (int)currentsize);//change size of dot image


            //currentred += 0.44f   (float)(220);
            //currentblue -= 0.342f;
            // currentgreen -= 0.39f;
            currentsize -= 1.92f;
            if (currentsize <= 100f)//untill blue
                breathin = true;
            starting = true;

        }


    }
}
