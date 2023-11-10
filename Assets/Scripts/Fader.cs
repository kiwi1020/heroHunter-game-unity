using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image image;
    RectTransform rect;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        FadeOut();
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);

        image.color = new Color(1, 1, 1, 1);
        image.DOFade(0, 1f).SetDelay(0.5f);

        rect.localPosition = new Vector3(0, -300, 0);
        rect.DOAnchorPosY(2000, 2);
    }

}
