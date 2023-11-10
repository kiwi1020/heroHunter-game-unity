using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCardDeck : MonoBehaviour
{
    [SerializeField] List<MoveCard> cards;
    [SerializeField] int handPoint; // 핸드를 몇 장 받을 지 정하는 능력치, 플레이어 능력치에서 가져옴. 임시로 여기에 선언

    public void SetHand()
    {
        if (MapSystem.moveCardDraw == true) // 이동카드 뽑기를 한번만 가능 
        {
            //-600 기준으로 2 장일 때에는 400 차이
            //-600 언저리로 기준 정하고, 400 언저리로 카드간 간격
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


                var names = new string[] { "걷기", "뒷걸음질", "달리기","준비", "도망치기" };


                //타일 개수에 따른 이동카드 조건(나중에 추가로 수정해야 할듯?)
                if(MapSystem.tileCount >= 1)
                {
                    cards[i].SetCard(names[2]);
                }
                //cards[i].SetCard(names[Random.Range(0, 4)]);
            }
            MapSystem.moveCardDraw = false;
        }
    }
}
