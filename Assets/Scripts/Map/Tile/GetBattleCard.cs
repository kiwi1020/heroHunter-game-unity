using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class GetBattleCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;
    [SerializeField] Image illust;
    [SerializeField] Outline outline;
    RectTransform rect;
    BattleCardData battleCardData;
    DiceConditioner diceConditioner;
    [SerializeField] GameObject diceconditioner;

    public int ClickCount;
    public  bool isSelect=false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetCard(BattleCardData _battleCardData)
    {
        print("in!");
        battleCardData = _battleCardData;
        illust.sprite = DataManager.instance.AlllBattleCardIllusts.Find(x => x.name == battleCardData.name).sprite;
        cardNameText.text = battleCardData.name;
        cardNameText.color = Color.white;

        cardDesText.text = "";
        if (battleCardData.skillData.effects[0] == "없음")
        {
            cardDesText.text = "없음";
        }
        else
        {
            for (int i = 0; i < battleCardData.skillData.effects.Count; i++)
            {
                var des = battleCardData.skillData.effects[i].Split('/')[0].Split(":");

                if (battleCardData.skillData.effects[i].Split('/').Length > 1) cardDesText.text += "전체 ";

                if (des.Length > 2) cardDesText.text += float.Parse(des[2]) * 100 + "% ";

                cardDesText.text += des[0] + " " + des[1] + "\n";

            }
        }

        if (battleCardData.skillData.enforcedEffects[0] == "없음")
        {
            diceconditioner.gameObject.SetActive(false);
        }
        else
        {
            diceconditioner.gameObject.SetActive(true);

            diceConditioner = GetComponent<DiceConditioner>();
            diceConditioner.SetDiceCondition(battleCardData.diceCondition);
        }
        print("out!");
    }

    public void Zoom(bool _zoom)
    {
        if (!isSelect) {
            if (_zoom)
            {
                rect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            }
            else
            {
                rect.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            }
        }
    }
    public void SelectCard()
    {
        ClickCount++;
        if(ClickCount % 2 == 0)
        {
            isSelect = false;
            outline.enabled=false;
            TileEvent.getbattleCardDatas.Remove(DataManager.instance.AllBattleCardDatas[cardNameText.text]);
        }
        else if(ClickCount % 2 == 1)
        {
            rect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            isSelect = true;
            outline.enabled = true;

            TileEvent.getbattleCardDatas.Add(DataManager.instance.AllBattleCardDatas[cardNameText.text]);
        }
    }

}
