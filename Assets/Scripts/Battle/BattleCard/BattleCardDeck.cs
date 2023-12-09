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
        ResetCard();

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

    public void RerollDice(int _c)
    {
        while(_c-1 > BattleSystem.instance.curDiceCount)
        {
            _c--;
        }

        for(int i = 0; i<_c; i++)
        {
            if(!pocket.dices[i].GetComponent<Dice>().gameObject.activeSelf)
            {
                i--;
                continue;
            }
            else
            {
                pocket.dices[i].DOShakeAnchorPos(0.5f,50);
                pocket.dices[i].GetComponent<Dice>().Set();
            }
        }
    }

    void ResetCard()
    {
        foreach(RectTransform j in battleCardPool)
        {
            ResetColor(j);
        }
    }

    void ResetColor(RectTransform _tmp)
    {
        var tmp = _tmp.GetComponent<BattleCard>();
        tmp.enforced = false;
        foreach (Image i in tmp.GetComponentsInChildren<Image>())
        {
            i.color = Color.white;
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

    public void AddHand(BattleCardData _battleCardData = null) //null이면 드로우
    {
        SetHandCardPosition();
        curHandCardCount++;
        var battleCard = battleCardPool[System.Array.FindIndex(battleCardPool, x => !x.gameObject.activeSelf)].GetComponent<BattleCard>();
        if (_battleCardData == null)
        {
            var randomInt = Random.Range(0, instantBattleCardData.Count);
            battleCard.SetCard(instantBattleCardData[randomInt]);
            instantBattleCardData.RemoveAt(randomInt);
        }
        else
        {
            battleCard.SetCard(_battleCardData);
        }
        curHands.Add(battleCard); //넣을때 위치 잘 보고
        SetHandCardPosition();
        ArrangeCurHand();
    }

    void ArrangeCurHand()
    {
        var tmp = new List<BattleCard>();
        foreach (RectTransform i in battleCardPool)
        {
            if (i.gameObject.activeSelf)
            {
                tmp.Add(i.GetComponent<BattleCard>());
            }
            else continue;
        }
        curHands = tmp;
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
            DOTween.Kill(j.GetComponent<RectTransform>());

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

    public void EndTurn()
    {
        curHandCardCount = 0;
        curHands.Clear();
        foreach (RectTransform i in battleCardPool)
        {
            i.DOAnchorPos(new Vector2(0, -850), 1).OnComplete(() => i.gameObject.SetActive(false));
        }
    }
}
