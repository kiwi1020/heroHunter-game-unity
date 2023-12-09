using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public GameObject manager;
    public AudioMixer masterMixer;
    public Slider[] sliders;
    public GameObject AudioManager;

    public void ControllVolume(string audioType)
    {
        switch (audioType)
        {
            case "Bgm":
                masterMixer.SetFloat("Bgm", sliders[0].value);
                break;

            case "Sfx":
                masterMixer.SetFloat("Sfx", sliders[1].value);
                break;
        }
      
    }

    public void EnableSettingUI()
    {
        AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(0);
        manager.SetActive(true);
        manager.transform.GetChild(0).gameObject.SetActive(true);
    }


    public void DisableSettingUI()
    {
        AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(0);
        GameObject.Find("SettingUI").SetActive(false);
    }

    public void ActEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
