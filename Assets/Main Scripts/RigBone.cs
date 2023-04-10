using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigBone//unused class for humanoid's bone control script
{
    public GameObject gameObject;
    public HumanBodyBones bone;
    public bool isValid;
    Animator animator;
    Quaternion savedValue;
    public RigBone(GameObject g, HumanBodyBones b)
    {
        gameObject = g;
        bone = b;
        isValid = false;
        animator = gameObject.GetComponent<Animator>();
       
        Avatar avatar = animator.avatar;
        
        isValid = true;
        savedValue = animator.GetBoneTransform(bone).localRotation;
    }
    public void set(float a, float x, float y, float z)
    {
        set(Quaternion.AngleAxis(a, new Vector3(x, y, z)));
    }
    public void set(Quaternion q)
    {
        animator.GetBoneTransform(bone).localRotation = q;
        savedValue = q;
    }
}