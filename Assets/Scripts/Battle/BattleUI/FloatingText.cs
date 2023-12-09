using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    RectTransform rect;
    public TextMeshProUGUI text;

    public void SetText(string _text, Color _color)
    {
        if (rect == null || text == null) return;
        DOTween.Kill(rect);
        DOTween.Kill(text);
        gameObject.SetActive(true);

        text.text = _text;
        text.color = _color;
    }

    public void Floating()
    {
        if (rect == null || text == null) return;
        rect.DOAnchorPos(new Vector2(rect.anchoredPosition.x, 0), 2);
        text.DOFade(0, 2);
    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();
    }
}
