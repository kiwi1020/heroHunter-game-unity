using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class BattleCard_CardBag : MonoBehaviour
{
    public BattleCardData battleCardData;
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;
    [SerializeField] Image illust;

    DiceConditioner diceConditioner;

    public bool enforced = false;

    public bool[] diceCondition = new bool[3];


    [SerializeField] GameObject diceconditioner;
    public void Enfoce()
    {
        if(enforced)
        {
            SetCard();
            enforced = false;
        }
        else
        {
            EnforceCard();
            enforced = true;
        }
    }

    public void SetCard(BattleCardData _battleCardData)
    {
        battleCardData = _battleCardData;
        SetCard();
    }
    public void SetCard()
    {
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


        //

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
    }

    public void EnforceCard()
    {
        if(battleCardData.skillData.enforcedEffects[0] == "없음")
        {
            enforced = false;
            return;
        }
        cardNameText.text = '*' + battleCardData.name;
        cardNameText.color = Color.red;

        cardDesText.text = "";


        enforced = true;


        for (int i = 0; i < battleCardData.skillData.enforcedEffects.Count; i++)
        {
            var des = battleCardData.skillData.enforcedEffects[i].Split('/')[0].Split(':');

            if (battleCardData.skillData.enforcedEffects[i].Split('/').Length > 1) cardDesText.text += "전체 ";

            if (des.Length > 2) cardDesText.text += float.Parse(des[2]) * 100 + "% ";

            cardDesText.text += des[0] + " " + des[1] + "\n";

        }
    }


}
