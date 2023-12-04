using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class StartSceneButton : MonoBehaviour
{
    [SerializeField] Image fade;
    public GameObject AudioManager;
    

    public void ActStartButton()
    {
        SceneManager.LoadScene("MoveScene");
        AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(0);
    }

    //public void ActSettingButton()
    //{
        //AudioManager = GameObject.Find("AudioManager");
        //AudioManager.GetComponent<SoundManager>().UISfxPlay(0);
    //}

    public void ActEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
