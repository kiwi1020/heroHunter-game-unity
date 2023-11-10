using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class StartSceneButton : MonoBehaviour
{
    [SerializeField] Image fade;

    public void ActStartButton()
    {
        SceneManager.LoadScene("MoveScene");
    }

    public void ActSettingButton()
    {

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
