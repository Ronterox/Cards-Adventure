using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("main menu");
    }

    public void StopBackgroundMusic()
    {
        AudioManager.instance.Stop("main menu");
    }
}
