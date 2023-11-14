using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Dice : MonoBehaviour, IEndDragHandler
{
    RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        rect.DOAnchorPos(rect.anchoredPosition + new Vector2(Random.Range(-10, 10f), Random.Range(-10, 10f)), Random.Range(0.5f, 1));
        rect.DORotate(new Vector3(0, 0, Random.Range(-30, 30f)), Random.Range(0.5f, 1));
    }

    public void Use()
    {
        gameObject.SetActive(false);
    }
}
