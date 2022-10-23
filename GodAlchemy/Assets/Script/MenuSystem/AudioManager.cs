using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer theMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Master Vol"))
        {
            theMixer.SetFloat("Master Vol", PlayerPrefs.GetFloat("Master Vol"));
        }

        if (PlayerPrefs.HasKey("Music Vol"))
        {
            theMixer.SetFloat("Music Vol", PlayerPrefs.GetFloat("Music Vol"));
        }

        if (PlayerPrefs.HasKey("SFX Vol"))
        {
            theMixer.SetFloat("SFX Vol", PlayerPrefs.GetFloat("SFX Vol"));
        }
    }
}
