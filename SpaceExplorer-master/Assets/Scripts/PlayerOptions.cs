using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptions : MonoBehaviour
{

    public AudioSource backgroundMusic;
    public Toggle muteToggle;

    // Start is called before the first frame update
    void Start()
    {
        // temporary variable to get the results from playerprefs- if it's 1 then mute, otherwise don't
        bool muted = (PlayerPrefs.GetInt("Mute") == 1) ? true : false;
        //pass this variable to actually mute
        MusicClicked(muted);
        //make sure the toggle visual is updated
        muteToggle.isOn = muted;
    }

    public void MusicClicked(bool state)
    {
        //set the result of whether it is muted or not (this is set in Toggle's actions in editor)
        backgroundMusic.mute = !state;
        //pause any music then play again because volume does not appear to be updating correctly
        backgroundMusic.Pause();
        backgroundMusic.Play();
        //turn into an int, Playerprefs doesn't support bool
        int muted = state ? 1 : 0;

        //save to playerprefs with the mute "key"
        PlayerPrefs.SetInt("Mute", 0);
        PlayerPrefs.Save();
    }

    //used by the slider's onchanged event in inspector
    public void volumeChanged(float volume)
    {
        backgroundMusic.volume = volume;
        backgroundMusic.Pause();
        backgroundMusic.Play();
    }

    public void showHideOptions(GameObject panel)
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

}
