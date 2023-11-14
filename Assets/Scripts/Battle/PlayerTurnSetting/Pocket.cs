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

    bool isOpen;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void PocketClick()
    {
        if(isOpen)
        {
            isOpen = false;
            image.sprite = sprites[1];

            foreach(RectTransform i in dices)
            {
                i.DORotate(new Vector3(0, 0, 0),0.5f);
                i.DOAnchorPos(new Vector2(0,0),0.5f).OnComplete(()=> i.gameObject.SetActive(false));
            }
        }
        else
        {
            isOpen = true;
            image.sprite = sprites[0];

            for(int i = 0; i<PlayManager.instance.playerData.diceCount; i++)
            {
                dices[i].gameObject.SetActive(true);
                dices[i].DORotate(new Vector3(0, 0, Random.Range(-90, 90f)), Random.Range(0.5f, 1));
                dices[i].DOAnchorPos(new Vector2(Random.Range(50, 500f), Random.Range(-100, 400f)), Random.Range(0.5f, 1));
            }
        }
    }
}
