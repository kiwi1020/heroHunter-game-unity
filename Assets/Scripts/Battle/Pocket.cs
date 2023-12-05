using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Pocket : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    public RectTransform[] dices;

    Image image;

    public bool isOpen;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void OffDices()
    {
        foreach (RectTransform i in dices) i.gameObject.SetActive(false);
    }
    public void PocketClick()
    {
        if(isOpen)
        {
            isOpen = false;
            image.sprite = sprites[1];

            for (int i = 0; i < BattleSystem.instance.curDiceCount; i++)
            {
                if (!dices[i].gameObject.activeSelf) continue;

                DOTween.Kill(dices[i]);
                dices[i].GetComponent<Image>().raycastTarget = true;
                dices[i].DORotate(new Vector3(0, 0, 0), 0.5f);
                dices[i].DOAnchorPos(new Vector2(0, 0), 0.5f).OnComplete(() => OffDices());
            }
        }
        else
        {
            isOpen = true;
            image.sprite = sprites[0];

            for(int i = 0; i< BattleSystem.instance.curDiceCount; i++)
            {
                //if (!dices[i].gameObject.activeSelf) continue;
                dices[i].gameObject.SetActive(true);
                dices[i].DORotate(new Vector3(0, 0, Random.Range(-90, 90f)), Random.Range(0.5f, 1));
                dices[i].DOAnchorPos(new Vector2(Random.Range(50, 500f), Random.Range(-100, 400f)), Random.Range(0.5f, 1));
            }
            GameObject AudioManager = GameObject.Find("AudioManager");
            AudioManager.GetComponent<SoundManager>().UISfxPlay(3);
        }
    }
}
