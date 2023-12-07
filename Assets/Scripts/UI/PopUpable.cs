using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PopUpable : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{ 
    RectTransform rect;
    public string des;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        PopUp tmpPop;
        if (BattleSystem.instance != null) tmpPop = BattleSystem.instance.popUp;
        else tmpPop = MapSystem.instance.popUpObj;

        tmpPop.gameObject.SetActive(true);
        tmpPop.TextChange(des);

        var tmpPopRect = tmpPop.GetComponent<RectTransform>();
        tmpPopRect.anchoredPosition = Input.mousePosition;
            //new Vector2(rect.anchoredPosition.x + tmpPopRect.sizeDelta.x + 25, rect.anchoredPosition.y + 50);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        PopUp tmpPop;
        if (BattleSystem.instance != null) tmpPop = BattleSystem.instance.popUp;
        else tmpPop = MapSystem.instance.popUpObj;

        tmpPop.gameObject.SetActive(false);
    }
}
