using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

using TensorFlowLite;
using Cysharp.Threading.Tasks;


public class ExerciseControl : MonoBehaviour
{
    [SerializeField, FilePopup("*.tflite")] string fileName = "posenet_mobilenet_v1_100_257x257_multi_kpt_stripped.tflite";//get learned model
    [SerializeField] RawImage cameraView = null;//camera preview to transmit between libraries
    //[SerializeField] bool runBackground;
    public Text PopUp;//Text message to show for the user


    bool camAvailable;//error handling boolean checking if the device has any cam available
    public bool playerinframe;//returns if the player during last nth checks was in frame
    int precision = 20;//amount of frames to check before assigning if the player was in the frame
    public GameObject popupstruct;//MessagePopUp notification
    WebCamTexture frontCam;//front camera of the device
    PoseNet poseNet;//Library reaading and comparing the learning outcome with realtime detection
    Vector3[] corners = new Vector3[4];//corners of the camera screen
    PrimitiveDraw draw;//draws 3D lines on capture
    UniTask<bool> task;//async background tasks worker
    PoseNet.Result[] results;//results of detection; returns x,y axis on the plane and accuracy
    CancellationToken cancellationToken;//cancels unitask worker
    public int breakoutduration;//estimated time of waiting between series
    public AudioSource soundsource;

    public Image flashthescreen;//message flash effect for better visibility of popups
    public List<bool> playerinrange;//boolean list of last nth frames returning if all body parts of the player were visible during frame capture
    public Button btnback;//transfers to main menu
    public GameObject humanoid;//unfinished humanoid model simulating player movements
    //humanoid's body parts:
    RigBone leftUpperArm;
    RigBone leftLowerArm;
    RigBone rightUpperArm;
    RigBone rightUpperLeg;
    RigBone rightLowerLeg;
    RigBone head;
    RigBone rightLowerArm;
    RigBone leftHand;
    RigBone rightHand;
    RigBone leftLowerLeg;
    RigBone leftUpperLeg;
    RigBone leftFoot;
    RigBone rightFoot;
    RigBone spine;

    //Method returning if player was visible in the frame called from both exercise scripts and exercise control
    public bool PlayerIsInFrame()
    {
        if (playerinframe)
            return true;
        return false;
    }//Unused method for getting detection outcome and assigning to humanoid body parts
    void UpdateSkeleton()
    {
        if (humanoid == null)
            return;
        try
        {
            if (SearchResult("NOSE", "con") >= 0.5f)
                head.set((float)100, SearchResult("NOSE", "x"), SearchResult("NOSE", "y"), 0);
            if (SearchResult("RIGHT_SHOULDER", "con") >= 0.5f)
                rightUpperArm.set((float)100, SearchResult("RIGHT_SHOULDER", "x"), SearchResult("RIGHT_SHOULDER", "y"), 0);
            if (SearchResult("LEFT_SHOULDER", "con") >= 0.5f)
                leftUpperArm.set((float)100, SearchResult("LEFT_SHOULDER", "x"), SearchResult("LEFT_SHOULDER", "y"), 0);


            if (SearchResult("LEFT_ELBOW", "con") >= 0.5f)
                leftLowerArm.set((float)100, SearchResult("LEFT_ELBOW", "x"), SearchResult("LEFT_ELBOW", "y"), 0);

            if (SearchResult("RIGHT_ELBOW", "con") >= 0.5f)
                rightLowerArm.set((float)100, SearchResult("RIGHT_ELBOW", "x"), SearchResult("RIGHT_ELBOW", "y"), 0);



            if (SearchResult("RIGHT_WRIST", "con") >= 0.5f)
                rightHand.set((float)100, SearchResult("RIGHT_WRIST", "x"), SearchResult("RIGHT_WRIST", "y"), 0);
            if (SearchResult("LEFT_WRIST", "con") >= 0.5f)
                leftHand.set((float)100, SearchResult("LEFT_WRIST", "x"), SearchResult("LEFT_WRIST", "y"), 0);


            if (SearchResult("RIGHT_HIP", "con") >= 0.5f)
                rightUpperLeg.set((float)100, SearchResult("RIGHT_HIP", "x"), SearchResult("RIGHT_HIP", "y"), 0);
            if (SearchResult("LEFT_HIP", "con") >= 0.5f)
                leftUpperLeg.set((float)100, SearchResult("LEFT_HIP", "x"), SearchResult("LEFT_HIP", "y"), 0);

            if (SearchResult("RIGHT_KNEE", "con") >= 0.5f)
                rightLowerLeg.set((float)100, SearchResult("RIGHT_KNEE", "x"), SearchResult("RIGHT_KNEE", "y"), 0);
            if (SearchResult("LEFT_KNEE", "con") >= 0.5f)
                leftLowerLeg.set((float)100, SearchResult("LEFT_KNEE", "x"), SearchResult("LEFT_KNEE", "y"), 0);

            //if (SearchResult("RIGHT_HIP", "con") >= 0.5f)
            //  if (SearchResult("LEFT_HIP", "con") >= 0.5f)
            //    if (SearchResult("LEFT_SHOULDER", "con") >= 0.5f)
            //      if (SearchResult("RIGHT_SHOULDER", "con") >= 0.5f)
            //        spine.set((float)100, (SearchResult("LEFT_SHOULDER", "x") - SearchResult("RIGHT_SHOULDER", "x")) / 2, (SearchResult("LEFT_SHOULDER", "y") - SearchResult("LEFT_HIP", "y")) / 2, 0);

            if (SearchResult("RIGHT_ANKLE", "con") >= 0.5f)
                rightFoot.set((float)100, SearchResult("RIGHT_ANKLE", "x"), SearchResult("RIGHT_ANKLE", "y"), 0);
            if (SearchResult("LEFT_ANKLE", "con") >= 0.5f)
                leftFoot.set((float)100, SearchResult("LEFT_ANKLE", "x"), SearchResult("LEFT_ANKLE", "y"), 0);

        }
        catch (System.NullReferenceException ex)
        {
            return;
        }
        catch (System.IndexOutOfRangeException ex2)
        {
            return;
        }

    }//assigning humanoid body parts
    void LoadSkeletorBones()
    {
        if (humanoid == null)
            return;
        head = new RigBone(humanoid, HumanBodyBones.Head);
        rightLowerArm = new RigBone(humanoid, HumanBodyBones.RightLowerArm);
        leftHand = new RigBone(humanoid, HumanBodyBones.LeftHand);
        rightHand = new RigBone(humanoid, HumanBodyBones.RightHand);
        leftUpperLeg = new RigBone(humanoid, HumanBodyBones.LeftUpperLeg);
        rightFoot = new RigBone(humanoid, HumanBodyBones.RightFoot);
        leftFoot = new RigBone(humanoid, HumanBodyBones.LeftFoot);
        leftLowerLeg = new RigBone(humanoid, HumanBodyBones.LeftLowerLeg);
        leftUpperArm = new RigBone(humanoid, HumanBodyBones.LeftUpperArm);
        leftLowerArm = new RigBone(humanoid, HumanBodyBones.LeftLowerArm);
        rightUpperArm = new RigBone(humanoid, HumanBodyBones.RightUpperArm);
        rightUpperLeg = new RigBone(humanoid, HumanBodyBones.RightUpperLeg);
        rightLowerLeg = new RigBone(humanoid, HumanBodyBones.RightLowerLeg);
        spine = new RigBone(humanoid, HumanBodyBones.Spine);
    }//Method called by "Back" button
    public void ToMainMenu()
    {
        Application.LoadLevel(0);
    }

    void Start()
    {
        songqueue = new Queue<string>();//queue of sounds which couldn't be played
        PlayPopUpSound("GetPostureSound");
        MessagePopUp(GetLanguage.instance.activeVocabulary.GetPosture);//voice sound to get prepared

        complete = false;//checks if the exercise is completed
        Results.text = "";
        //disable breakout buttons
        Breakout.enabled = false;
        ExitBreakOutButton.enabled = false;
        //load the breakout time from savefile based on the scene
        if (Application.loadedLevel == 1)
            breakoutduration = SaveORLoadData.instance.activeSave.breaktime1;
        else if (Application.loadedLevel == 2)
            breakoutduration = SaveORLoadData.instance.activeSave.breaktime2;
        else if (Application.loadedLevel == 3)
            breakoutduration = SaveORLoadData.instance.activeSave.breaktime3;
        else if (Application.loadedLevel == 4)
            breakoutduration = SaveORLoadData.instance.activeSave.breaktime4;
        breaktime = breakoutduration;
        soundsource = GetComponent<AudioSource>();


        LoadSkeletorBones();
        //playing = false;
        btnback = btnback.GetComponent<Button>();
        //set paths for learning outcome and prepare camera
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        poseNet = new PoseNet(path);

        camAvailable = SetCamera();
        //prepare popup:
        flashthescreen.enabled = false;
        PopUp.text = "";
        popupstruct.SetActive(false);
        playerinframe = false;
        //change color marker of the 3D lines to be drawn
        draw = new PrimitiveDraw()
        {
            color = Color.red,
        };

        cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    void OnDestroy()
    {
        frontCam?.Stop();
        poseNet?.Dispose();
        draw?.Dispose();
    }
    
    bool SetCamera()//check if there is front camera and set to Rawimage
    {

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No device found");
            return false;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {

                frontCam = new WebCamTexture(devices[i].name, 640, 480, 30);
                frontCam.Play();

                cameraView.texture = frontCam;
                // cameraView.rectTransform.localEulerAngles = new Vector3(0, 0, 180);
                return true;
            }

        }
        if (frontCam == null)
        {
            Debug.Log("The device has no front camera");
            return false;
        }
        return false;

    }
    void Update()
    {
        PlayQueued();//playqueued songs (if possible)
        
            if (task.Status.IsCompleted())
                task = InvokeAsync();
        
        if (playerinrange.Count == precision)//if the measurement got to accuracy point, clear the list and check if the player was in frame
        {
            UpdateSkeleton();
            playerinframe = isPlayerAtGoodPlace(findtrues(playerinrange));
            playerinrange.Clear();


        }
        else//if not add another measurement
            playerinrange.Add(checkPlayerPosition());

        if (results != null)
            DrawResult();

    }
    bool findtrues(List<bool> a)//find quickly if the player was at least once in the frame during measurement
    {
        foreach (bool item in a)
            if (item == true)
                return true;
        return false;
    }
    public float SearchResult(string name, string type)//Method used for getting body parts estimated by detection algorithm
    {
        var rect = cameraView.GetComponent<RectTransform>();//get rects of camera screen
        rect.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[2];

        int[] chosen = new int[2];

        for (int i = 0; i < 16; i++)//from 16 body parts choose by names and return their results and position connection
        {
            if (name == results[(int)PoseNet.Connections[i, 0]].part.ToString())
            {
                chosen[0] = i;
                chosen[1] = 0;
            }
            if (name == results[(int)PoseNet.Connections[i, 1]].part.ToString())
            {
                chosen[0] = i;
                chosen[1] = 1;
            }
        }


        try//not having body parts in the frame may raise exception, what requires proper error handling
        {//interpolate between the results for better handling and place the results on the screen
            var vect = MathTF.Lerp(min, max, new Vector3(results[(int)PoseNet.Connections[chosen[0], chosen[1]]].x, 1f - results[(int)PoseNet.Connections[chosen[0], chosen[1]]].y, 0));
            if (type == "x")
                return vect.x;
            else if (type == "y")
                return vect.y;
            else
                return results[(int)PoseNet.Connections[chosen[0], chosen[1]]].confidence;
        }

        catch (System.IndexOutOfRangeException ex) { return 0.0f; }
        catch (System.NullReferenceException ex) { Debug.Log("FAIL"); return 0.0f; }
    }
    bool checkPlayerPosition()//checks the presence of user by checking if eye, elbow and knee are detectable
    {
        var rect = cameraView.GetComponent<RectTransform>();
        rect.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[2];
        try
        {
            if (SearchResult("LEFT_EYE", "con") >= 0.5f || SearchResult("RIGHT_EYE", "con") >= 0.5f)
                if (SearchResult("LEFT_ELBOW", "con") >= 0.5f || SearchResult("RIGHT_ELBOW", "con") >= 0.5f)
                    if (SearchResult("LEFT_KNEE", "con") >= 0.5f || SearchResult("RIGHT_KNEE", "con") >= 0.5f)
                        return true;

            return false;
        }
        catch (System.IndexOutOfRangeException ex) { return false; }


    }


    bool complete;
    int count;
    public bool DisplayFinalScreen(float timer, string mistakes, bool HighscoreBeaten)//display final screen with mistakes and results of training
    {
        count++;
        complete = true;
        Results.text = "";
        Breakout.enabled = true;
        ExitBreakOutButton.enabled = true;
        MessagePopUp("");
        if (count == 1)
            PlayNotificationSound("victory");
        count = 2;
        Results.text = GetLanguage.instance.activeVocabulary.Congratulations + "\n";//get caption from vocabulary
        if (HighscoreBeaten == true)
            Results.text = Results.text + GetLanguage.instance.activeVocabulary.Highscorebeaten + "\n";

        if (mistakes == "")
            Results.text = Results.text + GetLanguage.instance.activeVocabulary.ExerciseTime + timer.ToString("F0") + "s";
        else
            Results.text = Results.text + GetLanguage.instance.activeVocabulary.ExerciseTime + timer.ToString("F0") + "s" + "\n" + GetLanguage.instance.activeVocabulary.MistakesMade + "\n" + mistakes;


        return true;
    }
    public float breaktime;
    public Text Results;
    public Canvas Breakout;
    public Button ExitBreakOutButton;


    public bool DisplayBreakScreen()
    {

        if (breaktime == breakoutduration)
        {
            Debug.Log("BREAK");
            Breakout.enabled = true;
            ExitBreakOutButton.enabled = true;
        }
        Results.text = GetLanguage.instance.activeVocabulary.Time + ": " + breaktime.ToString("F0") + "s";//time left to end of breakout
        breaktime -= Time.deltaTime;
        //Debug.Log(breaktime);
        if (breaktime <= 0 && !complete)//if time is finished and series left, exit the screen and go back to exercise
        {

            Results.text = "";

            breaktime = breakoutduration;
            Breakout.enabled = false;
            ExitBreakOutButton.enabled = false;


            return false;
        }
        return true;
    }


    static int numberOfExercises = 4;//estimates the amount of exercise in case if next levels would be planned
    public void ExitBreakout()//Method called by pushing the button during breakout time=>terminates the canvas and resumes the exercise
    {

        if (complete)
        {
            Time.timeScale = 1;
            Debug.Log("DONE");
            if (Application.loadedLevel < numberOfExercises)
                Application.LoadLevel(Application.loadedLevel + 1);//go to next level
            else
                Application.LoadLevel(0);//if there is no to main menu
        }

        breaktime = 0;

    }
    public void PlayPopUpSound(string name)//Method calling for estimation of voice communicates based on language packs, adds proper suffix based on chosen language
    {

        if (SaveORLoadData.instance.activeSave.language == "eng")
            PlayNotificationSound(name + "ENG");
        else if (SaveORLoadData.instance.activeSave.language == "pl")
            PlayNotificationSound(name + "PL");
    }
    bool isPlayerAtGoodPlace(bool Onscreen)//checks if the player is in the frame and changes the line color to green or red
    {

        var rect = cameraView.GetComponent<RectTransform>();
        rect.GetWorldCorners(corners);
        Vector3 min = corners[0];
        Vector3 max = corners[2];


        if (Onscreen == true)
        {
            draw = new PrimitiveDraw()
            {
                color = Color.green,
            };

            var a = MessagePopUp("");
            return true;
        }
        else
        {
            draw = new PrimitiveDraw()
            {
                color = Color.red,
            };//if there is no player, play error sound, but only when breakout canvas is off and ever 0.3 seconds (seconds are measured differently because of time complexity)
            if (breaktime == breakoutduration && !complete && (Errornotificationcooldown == 0))
                PlayNotificationSound("error");
            Errornotificationcooldown += Time.deltaTime;
            if (Errornotificationcooldown >= 0.3f)
                Errornotificationcooldown = 0f;//reset timer

            var a = MessagePopUp(GetLanguage.instance.activeVocabulary.Walkaway);//returns that user is not in the frame

            return false;
        }

    }
    public float Errornotificationcooldown;
    public bool MessagePopUp(string info)//Method used for displating alert message on the screen
    {

        if (info == "")
        {
            flashthescreen.enabled = false;
            popupstruct.SetActive(false);
            return false;
        }
        flashthescreen.enabled = true;//flashes the screen to bring attention
        popupstruct.SetActive(true);
        PopUp.text = info;
        return true;


    }
    void DrawResult()//draws lines based on body connections and positions
    {
        try
        {
            var rect = cameraView.GetComponent<RectTransform>();
            rect.GetWorldCorners(corners);
            Vector3 min = corners[0];
            Vector3 max = corners[2];

            var connections = PoseNet.Connections;
            int len = connections.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                var a = results[(int)connections[i, 0]];
                var b = results[(int)connections[i, 1]];
                if (a.confidence >= 0.5f && b.confidence >= 0.5f)
                {
                    draw.Line3D(
                        MathTF.Lerp(min, max, new Vector3(a.x, 1f - a.y, 0)),
                        MathTF.Lerp(min, max, new Vector3(b.x, 1f - b.y, 0)),
                        0.5f
                    );
                }
            }

            draw.Apply();
        }
        catch (System.IndexOutOfRangeException ex) { return; }
    }

    public bool HeadStill(float tolerance)//Method checks if the user keeps the head straight
    {
        float a = (SearchResult("LEFT_SHOULDER", "x") + SearchResult("RIGHT_SHOULDER", "x")) / 2;//center of section
        float b = SearchResult("NOSE", "x");

        float c = SearchResult("NOSE", "y");
        float hipposition = SearchResult("LEFT_HIP", "y");
        float d = hipposition + (((GetDistance(hipposition, 0f, SearchResult("LEFT_KNEE", "y"), 0f)) / 4) * 7);//head size proportions: (KNEE=>HIP)/2

        bool e = PositionsRelation(" ", " ", " ", tolerance, Mathf.Abs(a - b));
        bool f = PositionsRelation(" ", " ", " ", tolerance, Mathf.Abs(c - d));

        if (e == true && f == true)
            return true;
        // Debug.Log(headpositioncoordinates[0] + "\n Yours X: " + SearchResult("NOSE", "x"));
        //Debug.Log(headpositioncoordinates[1] + "\n Yours Y: " +SearchResult("NOSE", "y"));
        return false;
    }

    public bool PositionsRelation(string bodypart1, string bodypart2, string axis, float tolerance, float? a)//checks if the bodyparts are in the similar place based on given tolerance and axis
    {

        if (a == null)
            a = Mathf.Abs(SearchResult(bodypart1, axis) - SearchResult(bodypart2, axis));
        if (a > tolerance)
            return false;


        return true;
    }

    async UniTask<bool> InvokeAsync()//gets results delegate
    {
        results = await poseNet.InvokeAsync(frontCam, cancellationToken);
        cameraView.material = poseNet.transformMat;
        return true;
    }
    public void PlayNotificationSound(string name)//Used for playing sound, it can be either called by method with full name (as for sound effects) or passed from the one adding suffixes
    {


        StartCoroutine(PlaySoundNumerator(name));
        StopCoroutine(PlaySoundNumerator(name));

    }

    Queue<string> songqueue;
    string currentlyplaying;
    IEnumerator PlaySoundNumerator(string name)//Coroutine playing the sounds every 7 seconds
    {
        if (currentlyplaying == "" || name == "victory" || name == "rep" || name == "error")
        {

            currentlyplaying = name;//lock the method for incoming sounds

            SoundManager.PlaySound(name);
            if (TryPeek())
                if (songqueue.Peek() == name)//if the song has been chosen from queue, remove it
                    songqueue.Dequeue();
            if (name != "victory" || name != "rep" || name == "error")//exceptions due to the need of instant callback
                yield return new WaitForSeconds(7);//wait 7 seconds
            currentlyplaying = "";//unlock the method

        }
        else
        {
            if (!songqueue.Contains(name))//if queue has no assigned sound and there is no way to play it at this moment, saves it for future play
                songqueue.Enqueue(name);
            //   PlayNotificationSound(name,true);


        }
    }
    void PlayQueued()
    {
        bool a = TryPeek();
        if (a == false)
            return;
        PlayNotificationSound(songqueue.Peek());//if queue is not empty tries to play the sound once again


    }
    bool TryPeek()//checks if queue is not empty 
    {
        try
        {
            songqueue.Peek();
        }
        catch (System.InvalidOperationException ex)
        {
            return false;
        }
        return true;
    }

    public float GetDistance(float x1, float y1, float x2, float y2)//get distance between points based on formula sqrt((x2-x1)^2+(y2-y1)^2)
    {

        return (Mathf.Sqrt(Mathf.Pow((x2 - x1), 2) + Mathf.Pow((y2 - y1), 2)));
    }
}
