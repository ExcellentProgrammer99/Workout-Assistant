using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goback : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToMainMenu()
    {
        Application.LoadLevel(0);
    }
}
