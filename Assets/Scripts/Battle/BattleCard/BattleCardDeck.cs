using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
public class BattleCardDeck : MonoBehaviour
{
    [SerializeField] RectTransform[] battleCardPool;

    //핸드 몇 장
    //하나 고르면 커지고, 나머지 작아지고
    //안 쓰고 내려놓으면 다시 원상태로 복귀

    List<BattleCardData> instantBattleCardData = new List<BattleCardData>();
    List<BattleCard> curHands = new List<BattleCard>();

    public Pocket pocket;


    public int curHandCardCount = 3; // 이거는 실시간으로 변경되는 손패 개수

    public void SetPlayerTurn()
    {
        curHandCardCount = 3;
        SetHandCardData();
        SetHand(); //초기화할 손패 개수
        SetDices();
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(1);
    }
    void SetDices()
    {
        foreach (RectTransform i in pocket.dices)
        {
            i.GetComponent<Dice>().Set();
        }
    }

    public void SetBattleCardDeck()
    {
        instantBattleCardData = PlayerData.playerBattleCardDeck.ToList(); // 덱 
    }

    public void SetHand() //매개변수 굳이?
    {
        foreach (RectTransform i in battleCardPool) i.gameObject.SetActive(false);

        SetHandCardPosition();
    }

    public void SetHandCardData()
    {
        for (int i = 0; i < curHandCardCount; i++)
        {
            var randomInt = Random.Range(0, instantBattleCardData.Count);
            var battleCard = battleCardPool[i].GetComponent<BattleCard>();
            battleCard.SetCard(instantBattleCardData[randomInt]);
            instantBattleCardData.RemoveAt(randomInt);
            curHands.Add(battleCard);
        }
    }

    public void SetHandCardPosition()
    {
        int i = 0;
        foreach(BattleCard j in curHands)
        {

            j.gameObject.SetActive(true);
            j.GetComponent<RectTransform>().DOAnchorPos(new Vector2(800 + (curHandCardCount * 20) - (200 - 10 * curHandCardCount) * (curHandCardCount - i + 1),
                -400 - System.MathF.Abs(curHandCardCount / 2 - i) * 20), 0.2f * (i + 1));
            j.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -5 + (5 * curHandCardCount) - (10 * i)), 0.2f * (i + 1));

            i++;
        }

    }
    public void UseCard(BattleCard _battleCard)
    {
        curHandCardCount--;
        curHands.Remove(_battleCard);
        SetHandCardPosition();
    }
}
