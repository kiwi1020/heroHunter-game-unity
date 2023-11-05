using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveCard : MonoBehaviour
{
    public MoveCardData moveCardData;
    
    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI nameText, desText;

    GameObject CardsHand;
    public void SetCard(string cardName)
    {
        moveCardData = DataManager.instance.AllMoveCardDatas[cardName];

        nameText.text = moveCardData.name;

        desText.text = "";
        foreach(string i in moveCardData.effects)
        {
            desText.text += i;
        }
        //카드 정보 가져와서 해당 카드 이미지랑 텍스트 세팅
        //이거는 그냥 카드에 무브카드 주고 거기서 세팅해도 될 듯?
    }
    public void MoveEffect()
    {
        CardsHand = transform.parent.gameObject;
        if(nameText.text == "걷기")
        {
            MapSystem.instance.PlayerMove();
            MapSystem.tileCount++;
            for(int i=0; i<=CardsHand.transform.childCount-1; i++)
            {
                CardsHand.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
