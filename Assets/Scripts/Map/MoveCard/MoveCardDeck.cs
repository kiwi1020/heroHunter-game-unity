using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class MoveCardDeck : MonoBehaviour
{
    [SerializeField] List<MoveCard> cards;
    [SerializeField] int handPoint; // 핸드를 몇 장 받을 지 정하는 능력치, 플레이어 능력치에서 가져옴. 임시로 여기에 선언
    [SerializeField] MoveCardData movecardData;

    string[] commonNames;
    bool isCardPositionSet = false;

    public void SetHand()
    {   
        if (MapSystem.instance.moveCardDraw == true) // 이동카드 뽑기를 한번만 가능 
        {
            PositioningCard();

            //카드 조건 추가 중
            for (int i = 0; i < handPoint; i++) 
                cards[i].SetCard(CardPer());

            MapSystem.instance.moveCardDraw = false;
            MapSystem.instance.cardHideButton.SetActive(true);
        }

        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(1);

    }

    public void PositioningCard() //이동하는 부분만 분리
    {
        if(!isCardPositionSet)
        {

            ResetCardPosition();

            //카드 뿌리기
            var center = -600 + Random.Range(-50, 50f) - 400 / 2 * handPoint;

            for (int i = 0; i < handPoint; i++)
            {
                var cardRect = cards[i].GetComponent<RectTransform>();

                cardRect.gameObject.SetActive(true);
                cardRect.DOAnchorPos(new Vector3(center + 400 * i, Random.Range(-50, 250f)), 1 - i * 0.2f).SetEase(Ease.OutCirc).OnComplete(() => cardRect.GetComponent<Button>().interactable = true);
                cardRect.DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 2);

                isCardPositionSet = true;
            }
        }
        else // 카드 치우기
        {

            ResetCardPosition();

            foreach (MoveCard i in cards)
            {
                //카드 사라지는 효과 추가 11-28
                var cardRect = i.GetComponent<RectTransform>();

                i.GetComponent<Button>().interactable = false;

                cardRect.DOAnchorPos(new Vector3(-680, -600), 1).SetEase(Ease.OutCirc);
                cardRect.DORotate(new Vector3(0, 0, 0), 1).OnComplete(() => i.gameObject.SetActive(false));
            }

            isCardPositionSet = false;
        }
    }

    void ResetCardPosition()//위치 리셋
    {
        foreach (MoveCard i in cards)
        {
            var cardRect = i.GetComponent<RectTransform>();

            DOTween.Kill(cardRect);
            if(MapSystem.instance.moveCardDraw) cardRect.anchoredPosition = new Vector2(0, 0);
        }
    }

    // 이동 카드 조건
    public string CardPer()
    {

        commonNames = new string[] { "걷기", "달리기", "당찬 전진", "준비", "추격", "조심스러운 발걸음" };
        MapSystem.instance.allowEffect = true;      

        if (MapSystem.curTileNum >= 3) // 이동 -3 가능
        {
            string[] lastNames = 
                commonNames.Concat(new string[] { "뒷걸음질", "전략적 후퇴", "도망치기", "나침반 고장", "발목 부상"}).ToArray();
            return GetRandomName(lastNames);
            
        }
        else if (MapSystem.curTileNum >= 2) // 이동 -2 가능
        {
            string[] middleNames = commonNames.Concat(new string[] { "뒷걸음질", "전략적 후퇴","발목 부상" }).ToArray();
            return GetRandomName(middleNames);
        }
        else if (MapSystem.curTileNum >= 0) // 이동 - 불가능 
        {
            return GetRandomName(commonNames);
        }
        else
        {
            return "";
        }
    }

    private string GetRandomName(string[] nameList)
    {
        var wrPicker = new WeightRandomPick<string>();
         
        foreach (string name in nameList)
        {
            int CardWeight = int.Parse(DataManager.instance.AllMoveCardDatas[name].weight);       
            wrPicker.Add(name,CardWeight);
        }

        return wrPicker.GetRandomPick(); ;
    }



}
