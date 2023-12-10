using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class StartStory : MonoBehaviour
{
    int index = 0;
    bool able = true;

    [SerializeField] TextMeshProUGUI[] text;
    [SerializeField] GameObject tutorial;
    private void OnEnable()
    {
        if (PlayerData.isTutorial[0]) gameObject.SetActive(false);
        GetComponent<Image>().color = Color.black;

    }
    public void ViewStory()
    {
        if (!able) return;

        able = false;

        if (index == 0)
        {
            text[0].gameObject.SetActive(true);
            text[0].color = new Color(1, 1, 1, 0);
            text[0].DOFade(1, 2).OnComplete(() => able = true);
        }
        else if (index == 1)
        {

            text[0].DOFade(0, 2).OnComplete(() =>
            {
                text[0].gameObject.SetActive(false);
                text[1].gameObject.SetActive(true);
                text[1].color = new Color(1, 1, 1, 0);
                text[1].DOFade(1, 2).OnComplete(() => able = true);
            });
        }
        else if (index == 2)
        {
            text[1].DOFade(0, 2).OnComplete(() =>
            {
                text[1].gameObject.SetActive(false);
                GetComponent<Image>().DOFade(0, 2).OnComplete(() =>
                {
                    tutorial.SetActive(true);
                    gameObject.SetActive(false);
                });
            });
        }
        index += 1;
    }
}
