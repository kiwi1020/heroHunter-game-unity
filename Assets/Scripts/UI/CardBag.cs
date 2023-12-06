using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBag : MonoBehaviour
{
    [SerializeField] GameObject[] cardBag, cardBack; // 0 : 배틀 1 : 움직
    [SerializeField] GameObject[] cardPre, cardLayout;
    [SerializeField] RectTransform[] cardContents;

    List<GameObject>[] cardPool = new List<GameObject>[2] { new List<GameObject>(), new List<GameObject>() };

    bool[] isOpen = new bool[] { false, false };

    public void InterectBag(int _whichCard)
    {
        //오브젝트 껏다 키는거
        if (isOpen[_whichCard])
        {
            isOpen[_whichCard] = false;
            cardBag[_whichCard].SetActive(false);
            cardBack[_whichCard].SetActive(false);
        }
        else
        {
            isOpen[_whichCard] = true;
            cardBag[_whichCard].SetActive(true);
            cardBack[_whichCard].SetActive(true);
            cardContents[_whichCard].anchoredPosition = new Vector2(0, 0);

            if (_whichCard == 0) SetBattleCardPool();
            else SetMoveCardPool();
        }


    }

    public void SetBattleCardPool()
    {
        if (cardPool[0].Count < PlayerData.playerBattleCardDeck.Count)
        {
            var tmpCount = PlayerData.playerBattleCardDeck.Count - cardPool[0].Count;
            for (int i = 0; i < tmpCount; i++)
            {
                var tmpCard = Instantiate(cardPre[0], cardLayout[0].transform);
                cardPool[0].Add(tmpCard);
            }
        }

        if (cardPool[0].Count > 0) foreach (GameObject i in cardPool[0]) i.SetActive(false);

        for (int i = 0; i < cardPool[0].Count; i++)
        {
            cardPool[0][i].SetActive(true);
            cardPool[0][i].GetComponent<BattleCard_CardBag>().SetCard(PlayerData.playerBattleCardDeck[i]);
        }
    }

    public void SetMoveCardPool()
    {

        if (cardPool[1].Count < PlayerData.playerMoveCardDeck.Count)
        {
            var tmpCount = PlayerData.playerMoveCardDeck.Count - cardPool[1].Count;
            for (int i = 0; i < tmpCount; i++)
            {
                var tmpCard = Instantiate(cardPre[1], cardLayout[1].transform);
                cardPool[1].Add(tmpCard);
            }
        }

        if (cardPool[1].Count > 0) foreach (GameObject i in cardPool[1]) i.SetActive(false);

        for (int i = 0; i < cardPool[1].Count; i++)
        {
            cardPool[1][i].SetActive(true);
            cardPool[1][i].GetComponent<MoveCard>().SetCard(PlayerData.playerMoveCardDeck[i].name);
        }
    }
}
