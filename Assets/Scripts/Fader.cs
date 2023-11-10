using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);

        spriteRenderer.color = new Color(1, 1, 1, 1);
        spriteRenderer.DOFade(0, 1f).SetDelay(0.5f);

        transform.position = Vector3.zero;
        transform.DOMoveY(24, 1);
    }

}
