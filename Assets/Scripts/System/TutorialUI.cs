using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public int tutorialNumber;

    private void OnEnable()
    {
        if (PlayerData.isTutorial[tutorialNumber] == true) gameObject.SetActive(false);
    }

    public void FinishTutorial()
    {
        PlayerData.isTutorial[tutorialNumber] = true;
    }
}
