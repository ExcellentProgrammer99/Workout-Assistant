using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Exercise_Lateral_Raises : MonoBehaviour
{
    public int count;//counter for current repetition
    public Text countText;//UI displays current repetition on screen
    private int repetitions; //amount of repetitions to be performed taken from requirements and converted to float for better time handling
    public int totalseries;//number of total amount of series to perform taken from requirements
    public ExerciseControl sample;//load to use control methods
    public SaveORLoadData save;//load script to modify the highscores
    public Text timetext;//shows current time on screen
    public List<string> mistakes;//list of mistakes made by the user
    bool breakenabled;//returns if user has currently a break between series
    private float timer;//time of exercise
    public Image img;//breathing dot image
    public int series;//stores actual series amount

    float timebetweenreps;//used for checking if the player is performing the exercise too fast measures the time between repetitions
    // Start is called before the first frame update
    void Start()
    {//assign beginning values for time/count/series and load requirements from settings
        timebetweenreps = 5f;
        totalseries = SaveORLoadData.instance.activeSave.seriesEX1;
        repetitions = SaveORLoadData.instance.activeSave.repetitionsEX1;
        breathin = true;
        starting = true;
        mistakes = new List<string>();
        part = 1;
        Time.timeScale = 1;
        posturecorrectiontime = 0;
        timer = 0;
        count = 0;
        series = 1;
        breakenabled = false;
        InvokeRepeating("Breath",0f,0.1f);//invoke method (background worker) to change position of breathing dor ever 0.1sec

    }
    float posturecorrectiontime;//time for the user to correct previous posture mistakes before next reminiscence
    // Update is called once per frame
    void Update()
    {
        
           // StartCoroutine(Breath());
        //Debug.Log(sample.SearchResult("LEFT_WRIST","y"));
        if (!breakenabled)//prevent counting the time when the user has a break
            posturecorrectiontime += Time.deltaTime;
        if(posturecorrectiontime>=10)//remind about posture corrections every 10 seconds to prevent spam and correct mistakes; not used in handstand due to
									 //the need of constant real-time corrections
        {
            PostureCorrection();
            posturecorrectiontime = 0;
        }
        
        DoExercise();
        if (!breakenabled)//prevents counting the time when the user has a break
            timer += Time.deltaTime;
        timetext.text = GetLanguage.instance.activeVocabulary.Time + ": " + timer.ToString("F0") + "s";
        countText.text = GetLanguage.instance.activeVocabulary.Repetitions + ": " + count.ToString() + "/" + repetitions.ToString();
    }
    int part;//used for switching part of repetition scheme
    bool DoExercise()
    {
        if (count == repetitions)//checks if user managed to the end of challenge
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
        }
        if (breakenabled)
            return true;//ignore the rest of method in case of breakout canvas enabled to prevent errors
        bool inframe = sample.PlayerIsInFrame();
        if (inframe)//to prevent errors, the algorithm works only when the player is in the frame
        {

            timebetweenreps += Time.deltaTime;
            if (timer > 7 * 60&&count>repetitions/2)//if the time exceeds 7 minutes and the user is more than halfway-through the notification with stopping exercise suggestion is enabled
            {
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.Motivation);
                FreezeTime();
            }
            if (part!=1)//first part of repetition->stay in initial position
            {
                bool result = GetInitialPosition();
                if (result)
                {
                    part = 1;
                    if (timebetweenreps <= 1.5f)//if the time of part performance is less than 1.5sec->say that exercise is performed too fast; prevents injuries
                        ExercisePerformedTooFast();
                    timebetweenreps = 0f;//reset the time of part performance
                }
            }
            
            else
            {
                bool result = GetfinalPosition();//checks if the user satisfied the rules of proper exercise assesment and adds repetition
                if (result && timebetweenreps >= 1f)
                {
                    part = 2;
                    count++;
                    sample.PlayNotificationSound("rep");//play repetition completed sound effect
                    if (timebetweenreps <= 1.5f)//if the time of part performance is less than 1.5sec->say that exercise is performed too fast; prevents injuries
                        ExercisePerformedTooFast();
                    timebetweenreps = 0f;//reset the time of part performance
                    //  audioSource.PlayOneSHot(rep);

                }
            }
        }
        return false;

    }
    
    bool GetInitialPosition()//initial position->hands next to hips
    {
        if (sample.PositionsRelation("RIGHT_HIP", "RIGHT_WRIST", "y", 1f,null) && sample.PositionsRelation("LEFT_HIP", "LEFT_WRIST", "y", 1f, null))
            return true;
        return false;
    }
    public bool Highscorebeaten()//checks if the user performed the exercise and set the time highscore and saves data to savefile
    {
        if ((int)timer <= SaveORLoadData.instance.activeSave.highscore1)
        {
            SaveORLoadData.instance.activeSave.highscore1 = (int)timer;
            save.ApplyChanges();
            return true;
        }
        return false;

    }
    bool GetfinalPosition()//final position->Shoulders and elbows in one line with the shoulder (because of the fact that the model is not qualified for work with lifting, it can miss the wrists covered by weights, so the algorithm checks elbows position)
    {
       
        if ((sample.PositionsRelation("RIGHT_SHOULDER", "RIGHT_WRIST", "y", 1f,null)&& sample.PositionsRelation("LEFT_SHOULDER", "LEFT_WRIST", "y", 1f,  null))||(sample.PositionsRelation("RIGHT_SHOULDER", "RIGHT_ELBOW", "y", 1f,  null) && sample.PositionsRelation("LEFT_SHOULDER", "LEFT_ELBOW", "y", 1f,  null)))
                return true;
        return false;
    }
    
    void PostureCorrection()
    {
        //Method used for explaining, showing and lisiting mistakes made during performance. The mistakes are later displayed on final screen of application. Unfortunatelly due
        //to learning outcome, the algorithm may miss the left body parts with right ones, so the algorithms which check if both hands are performing the exercise might
        //return wrong keypoints, however, with proper error handling and tolerance settings the problem seems not to appear everytime
        bool inframe = sample.PlayerIsInFrame();
        if (inframe)//posture correction works only if the player is in the frame to minimize effects of errors while player is away
        {
            //proper feet position determines the gravity center and pressure on spine, to check it algorithm compare position of hips and ankles in x axis
            if (!sample.PositionsRelation("LEFT_ANKLE", "LEFT_HIP", "x", 3f,  null) || !sample.PositionsRelation("RIGHT_ANKLE", "RIGHT_HIP", "x", 3f,  null) )
            {
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.FeetOnHipWidth))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.FeetOnHipWidth);
                sample.PlayPopUpSound("FeetOnHipWidthSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.FeetOnHipWidth);
                
                FreezeTime();
                
            }//straight spine prevents putting pressure on individual verterbra and following spine injuries. The algorithm checks if the hips position or shoulders position on y axis is straight
            if (!sample.PositionsRelation("RIGHT_SHOULDER", "LEFT_SHOULDER", "y", 2f, null) || !sample.PositionsRelation("LEFT_HIP", "RIGHT_HIP", "y", 2f, null))
            {
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.KeepYourSpineStraight))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.KeepYourSpineStraight);
                sample.PlayPopUpSound("KeepYourSpineStraightSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.KeepYourSpineStraight);
                FreezeTime();
               
            }//The exercise is designed to be performed symmetrical to prevent unneceassary muscle contraction and spine curvature
            if (!sample.PositionsRelation("RIGHT_ELBOW", "LEFT_ELBOW", "y", 2f, null))
            {
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.LiftSymmetrical))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.LiftSymmetrical);
                sample.PlayPopUpSound("LiftSymmetricalSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.LiftSymmetrical);
                FreezeTime();
                
            }//To exercise individual shoulder muscles (in case of that exercise side muscles) elbows need to be straight, otherwise front shoulder muscle part is encouraged 
            if ((ArmDistanceCorrect(sample.SearchResult("LEFT_HIP","x"), sample.SearchResult("LEFT_HIP", "y"),sample.SearchResult("LEFT_SHOULDER","x"),sample.SearchResult("LEFT_SHOULDER","y"), sample.SearchResult("LEFT_WRIST", "x"), sample.SearchResult("LEFT_WRIST", "y"), sample.SearchResult("LEFT_SHOULDER", "x"), sample.SearchResult("LEFT_SHOULDER", "y"),7f))
                || (ArmDistanceCorrect(sample.SearchResult("LEFT_HIP", "x"), sample.SearchResult("LEFT_HIP", "y"), sample.SearchResult("LEFT_SHOULDER", "x"), sample.SearchResult("LEFT_SHOULDER", "y"),sample.SearchResult("RIGHT_WRIST", "x"), sample.SearchResult("RIGHT_WRIST", "y"), sample.SearchResult("RIGHT_SHOULDER", "x"), sample.SearchResult("RIGHT_SHOULDER", "y"),7f)))
            {
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.KeepYourElbowsStraight))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.KeepYourElbowsStraight);
                sample.PlayPopUpSound("KeepYourElbowsStraightSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.KeepYourElbowsStraight);
                FreezeTime();
                
            }//shaking head engages other muscle parts and may curve the spine
            if (!sample.HeadStill(4f))
            {
                if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.KeepYourHeadStill))
                    mistakes.Add(GetLanguage.instance.activeVocabulary.KeepYourHeadStill);
                sample.PlayPopUpSound("KeepYourHeadStillSound");
                sample.MessagePopUp(GetLanguage.instance.activeVocabulary.KeepYourHeadStill);
                FreezeTime();
                
            }
        }
    }//performing exercise too fast may cause damage on joints or muscles not adapted to instant load and provoke injuries
    void ExercisePerformedTooFast()
    {
        if (!mistakes.Contains(GetLanguage.instance.activeVocabulary.TooFast))
            mistakes.Add(GetLanguage.instance.activeVocabulary.TooFast);
        sample.PlayPopUpSound("TooFastSound");
        sample.MessagePopUp(GetLanguage.instance.activeVocabulary.TooFast);
    }//Get distance between arms from proportions and return if fits in the tolerance
   bool ArmDistanceCorrect(float x1, float y1, float x2, float y2, float x3,float y3, float x4, float y4, float tolerance)
    {
        float a = sample.GetDistance(x1, y1, x2, y2);
        float b = sample.GetDistance(x3,y3,x4,y4);

     
        return sample.PositionsRelation(" "," "," ",tolerance,Mathf.Abs(a-b));

    }

    bool beaten;
    void ExerciseCompleted()//Method calls main function in control folder and finish the task of script
    {
        if(!beaten)
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
