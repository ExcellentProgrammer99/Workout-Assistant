using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class used for loading, storing and playing sound effects/voice communications
public class SoundManager : MonoBehaviour
{
    public static AudioClip AbdominalContractedSoundENG, AbdominalContractedSoundPL,
        CorrectWristPositionSoundENG, CorrectWristPositionSoundPL, KeepYourArmsStraightSoundENG, KeepYourArmsStraightSoundPL,
        KeepYourHeadStillSoundENG, KeepYourHeadStillSoundPL, FeetOnHipWidthSoundENG, FeetOnHipWidthSoundPL, KeepYourSpineStraightSoundENG,
        KeepYourSpineStraightSoundPL, LiftSymmetricalSoundENG, LiftSymmetricalSoundPL, KeepYourElbowsStraightSoundENG, KeepYourElbowsStraightSoundPL, error, victory,rep,TooFastSoundPL,TooFastSoundENG,GetPostureSoundENG,GetPostureSoundPL;


    static AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {//All audios are stored in Resources folder
        AbdominalContractedSoundENG = Resources.Load<AudioClip>("AbdominalContractedSoundENG");
        AbdominalContractedSoundPL = Resources.Load<AudioClip>("AbdominalContractedSoundPL");
        CorrectWristPositionSoundENG = Resources.Load<AudioClip>("CorrectWristPositionSoundPL");
        CorrectWristPositionSoundPL = Resources.Load<AudioClip>("CorrectWristPositionSoundPL");
        KeepYourArmsStraightSoundENG = Resources.Load<AudioClip>("KeepYourArmsStraightSoundENG");
        KeepYourArmsStraightSoundPL = Resources.Load<AudioClip>("KeepYourArmsStraightSoundPL");

        KeepYourHeadStillSoundENG = Resources.Load<AudioClip>("KeepYourHeadStillSoundENG");
        KeepYourHeadStillSoundPL = Resources.Load<AudioClip>("KeepYourHeadStillSoundPL");
        FeetOnHipWidthSoundENG = Resources.Load<AudioClip>("FeetOnHipWidthSoundENG");
        FeetOnHipWidthSoundPL = Resources.Load<AudioClip>("FeetOnHipWidthSoundPL");
        KeepYourSpineStraightSoundENG = Resources.Load<AudioClip>("KeepYourSpineStraightSoundENG");

        KeepYourSpineStraightSoundPL = Resources.Load<AudioClip>("KeepYourSpineStraightSoundPL");
        LiftSymmetricalSoundENG = Resources.Load<AudioClip>("LiftSymmetricalSoundENG");
        LiftSymmetricalSoundPL = Resources.Load<AudioClip>("LiftSymmetricalSoundPL");
        KeepYourElbowsStraightSoundENG = Resources.Load<AudioClip>("KeepYourElbowsStraightSoundENG");
        KeepYourElbowsStraightSoundPL = Resources.Load<AudioClip>("KeepYourElbowsStraightSoundPL");

        TooFastSoundENG = Resources.Load<AudioClip>("TooFastSoundENG");
        TooFastSoundPL = Resources.Load<AudioClip>("TooFastSoundPL");
       GetPostureSoundENG= Resources.Load<AudioClip>("GetPostureSoundENG");
        GetPostureSoundPL = Resources.Load<AudioClip>("GetPostureSoundPL");


        error = Resources.Load<AudioClip>("error");
        victory = Resources.Load<AudioClip>("victory");
        rep = Resources.Load<AudioClip>("rep");
        audio = GetComponent<AudioSource>();
    }

    // plays clip based on name and previously added suffix
    public static void PlaySound(string clip)
    {
        Debug.Log("Playing "+clip);
        switch (clip)
        {
            case "error":
                audio.PlayOneShot(error);
                break;
            case "AbdominalContractedSoundENG":
                audio.PlayOneShot(AbdominalContractedSoundENG);
                break;
            case "AbdominalContractedSoundPL":
                audio.PlayOneShot(AbdominalContractedSoundPL);
                break;
            case "CorrectWristPositionSoundPL":
                audio.PlayOneShot(CorrectWristPositionSoundPL);
                break;
            case "victory":
                audio.PlayOneShot(victory);
                break;
            case "CorrectWristPositionSoundENG":
                audio.PlayOneShot(KeepYourArmsStraightSoundENG);
                break;
            case "KeepYourArmsStraightSoundENG":
                audio.PlayOneShot(KeepYourArmsStraightSoundENG);
                break;
            case "KeepYourArmsStraightSoundPL":
                audio.PlayOneShot(KeepYourArmsStraightSoundPL);
                break;
            case "KeepYourHeadStillSoundENG":
                audio.PlayOneShot(KeepYourHeadStillSoundENG);
                break;
            case "KeepYourHeadStillSoundPL":
                audio.PlayOneShot(KeepYourHeadStillSoundPL);
                break;
            case "FeetOnHipWidthSoundENG":
                audio.PlayOneShot(FeetOnHipWidthSoundENG);
                break;
            case "FeetOnHipWidthSoundPL":
                audio.PlayOneShot(FeetOnHipWidthSoundPL);
                break;
            case "KeepYourSpineStraightSoundENG":
                audio.PlayOneShot(KeepYourSpineStraightSoundENG);
                break;
            case "KeepYourSpineStraightSoundPL":
                audio.PlayOneShot(KeepYourSpineStraightSoundPL);
                break;
            case "LiftSymmetricalSoundENG":
                audio.PlayOneShot(LiftSymmetricalSoundENG);
                break;
            case "LiftSymmetricalSoundPL":
                audio.PlayOneShot(LiftSymmetricalSoundPL);
                break;
            case "KeepYourElbowsStraightSoundENG":
                audio.PlayOneShot(KeepYourElbowsStraightSoundENG);
                break;
            case "KeepYourElbowsStraightSoundPL":
                audio.PlayOneShot(KeepYourElbowsStraightSoundPL);
                break;
            case "rep":
                audio.PlayOneShot(rep);
                break;
            case "TooFastSoundENG":
                audio.PlayOneShot(TooFastSoundENG);
                break;
            case "TooFastSoundPL":
                audio.PlayOneShot(TooFastSoundPL);
                break;
            case "GetPostureSoundENG":
                audio.PlayOneShot(GetPostureSoundENG);
                break;
            case "GetPostureSoundPL":
                audio.PlayOneShot(GetPostureSoundPL);
                break;

        }
    }
}
