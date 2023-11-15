using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
public class BattleCard : MonoBehaviour, IEndDragHandler, IDropHandler
{
    RectTransform rect;
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetCard(BattleCardData _battleCardData)
    {
        cardNameText.text = _battleCardData.name;
        cardNameText.color = Color.white;
        cardDesText.text = _battleCardData.skillData.effects[0];
    }

    public void Zoom(bool _zoom) // true면 확대, 카드 Eventtrigger 컴포넌트에서 사용, 마우스가 닿으면 확대, 떨어지면 축소
    {
        if(_zoom)
        {
            rect.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
        }
        else
        {
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        BattleSystem.instance.battleCardDeck.SetHandCardPosition();
    }

    public void EnforceCard()
    {
        cardNameText.text = "*"+ cardNameText.text;
        cardNameText.color = Color.red;
        cardDesText.text = "*물리피해 20";
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (eventData.pointerDrag.GetComponent<Dice>() != null)
        {
            eventData.pointerDrag.GetComponent<Dice>().Use();
            EnforceCard();
        }
    }

}
