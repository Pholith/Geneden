using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionsScreen : MonoBehaviour
{

    public Toggle fullScreenTog;

    public AudioMixer theMixer;

    public TMP_Text masterLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        fullScreenTog.isOn = Screen.fullScreen;

        float vol = 0f;
        theMixer.GetFloat("Master Vol", out vol);
        masterSlider.value = vol;

        theMixer.GetFloat("Music Vol", out vol);
        musicSlider.value = vol;

        theMixer.GetFloat("SFX Vol", out vol);
        sfxSlider.value = vol;

        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullScreenTog.isOn;
    }

    public void SetMasterVol()
    {
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        theMixer.SetFloat("Master Vol", masterSlider.value);

        PlayerPrefs.SetFloat("Master Vol", masterSlider.value);
    }

    public void SetMusicVol()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        theMixer.SetFloat("Music Vol", musicSlider.value);

        PlayerPrefs.SetFloat("Music Vol", musicSlider.value);
    }

    public void SetSFXVol()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        theMixer.SetFloat("SFX Vol", sfxSlider.value);

        PlayerPrefs.SetFloat("SFX Vol", sfxSlider.value);
    }
}
