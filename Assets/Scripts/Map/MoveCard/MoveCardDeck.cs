using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MoveCardDeck : MonoBehaviour
{
    [SerializeField] List<MoveCard> cards;
    [SerializeField] int handPoint; // 핸드를 몇 장 받을 지 정하는 능력치, 플레이어 능력치에서 가져옴. 임시로 여기에 선언
    [SerializeField] string[] commonNames;
    [SerializeField] MoveCardData movecardData;
    public void SetHand()
    {
        /*
        if (MapSystem.instance.moveCardDraw == true) // 이동카드 뽑기를 한번만 가능 
        {
            var center = -600 + Random.Range(-50, 50f) - 400 / 2 * handPoint;

            foreach (MoveCard i in cards)
            {
                var cardRect = i.GetComponent<RectTransform>();

                DOTween.Kill(cardRect);
                cardRect.anchoredPosition = new Vector2(0, 0);

                i.gameObject.SetActive(false);
            }

            for (int i = 0; i < handPoint; i++)
            {
                var cardRect = cards[i].GetComponent<RectTransform>();

                cardRect.gameObject.SetActive(true);
                cardRect.DOAnchorPos(new Vector3(center + 400 * i, Random.Range(-50, 250f)), 1 - i * 0.2f).SetEase(Ease.OutCirc);
                cardRect.DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 2);

                //카드 조건 추가중

                cards[i].SetCard(CardPer());
            }
            MapSystem.moveCardDraw = false;    
            
        }
        */
        var center = -600 + Random.Range(-50, 50f) - 400 / 2 * handPoint;

        foreach (MoveCard i in cards)
        {
            var cardRect = i.GetComponent<RectTransform>();

            DOTween.Kill(cardRect);
            cardRect.anchoredPosition = new Vector2(0, 0);

            i.gameObject.SetActive(false);
        }

        for (int i = 0; i < handPoint; i++)
        {
            var cardRect = cards[i].GetComponent<RectTransform>();

            cardRect.gameObject.SetActive(true);
            cardRect.DOAnchorPos(new Vector3(center + 400 * i, Random.Range(-50, 250f)), 1 - i * 0.2f).SetEase(Ease.OutCirc);
            cardRect.DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 2);

            //카드 조건 추가중

            cards[i].SetCard(CardPer());
        }        
    }

    // 이동 카드 조건
    public string CardPer()
    {
        
        if (MapSystem.instance.allowHealing == true)
        {
            commonNames = new string[] { "걷기", "달리기", "당찬 전진", "준비", "휴식", "추격", "조심스러운 발걸음"};
        }
        else
        { 
            commonNames = new string[] { "걷기", "달리기", "당찬 전진", "준비", "추격", "조심스러운 발걸음"};
            MapSystem.instance.allowEffect = true;
        }

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
        else if (MapSystem.curTileNum >= 1) // 이동 -1 가능
        {
            string[] basicNames = commonNames.Concat(new string[] { "뒷걸음질", "전략적 후퇴", "발목 부상"}).ToArray();
            return GetRandomName(basicNames);
        }
        else if (MapSystem.curTileNum >= 0) // 이동 - 불가능 
        {
            return GetRandomName(commonNames);
        }
        else
        {
            return "";
        }
        

        /*
        commonNames = new string[] { "걷기", "달리기", "당찬 전진", "준비", "휴식", "추격", "조심스러운 발걸음", "발목 부상" };
        return commonNames[Random.Range(0,commonNames.Length)];
        */
    }

    private string GetRandomName(string[] nameList)
    {
        return nameList[Random.Range(0, nameList.Length)];
    }
}
