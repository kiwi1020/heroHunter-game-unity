using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceConditioner : MonoBehaviour
{

    // 123456 ÀüÃ¼ Â¦¼ö È¦¼ö // up down ~
    [SerializeField] Sprite[] diceImages, conditionImages;

    [SerializeField] Image[] UpDownUI, TargetCostUI, CostsUI, RangeCostUI;

    
    public void SetDiceCondition(string _condition)
    {
        var c = _condition.Split(':');

        UpDownUI[0].gameObject.SetActive(false);
        TargetCostUI[0].gameObject.SetActive(false);
        CostsUI[0].gameObject.SetActive(false);
        RangeCostUI[0].gameObject.SetActive(false);

        switch (c[0])
        {
            case "ÁöÁ¤":

                var dc = c[1].Split(',');

                if(dc.Length == 1)
                {
                    TargetCostUI[0].gameObject.SetActive(true);

                    if(c[1] == "Â¦¼ö")
                    {
                        TargetCostUI[0].sprite = diceImages[7];
                    }
                    else if (c[1] == "È¦¼ö")
                    {
                        TargetCostUI[0].sprite = diceImages[8];
                    }
                    else if (c[1] == "ÀüÃ¼")
                    {
                        TargetCostUI[0].sprite = diceImages[6];
                    }
                    else
                    {
                        TargetCostUI[0].sprite = diceImages[int.Parse(dc[0]) - 1];
                    }
                }
                else
                {
                    foreach (Image i in CostsUI) i.gameObject.SetActive(false);
                    CostsUI[0].gameObject.SetActive(true);
                    for (int i = 0; i < dc.Length; i++)
                    {
                        CostsUI[i+1].gameObject.SetActive(true);
                        CostsUI[i+1].sprite = diceImages[int.Parse(dc[i]) - 1];
                    }
                }


                break;

            case "¹üÀ§":

                RangeCostUI[0].gameObject.SetActive(true);

                dc = c[1].Split('~');

                RangeCostUI[0].sprite = diceImages[int.Parse(dc[0]) - 1];
                RangeCostUI[1].sprite = diceImages[int.Parse(dc[1]) - 1];


                break;

            case "ÀÌ»ó":

                UpDownUI[0].gameObject.SetActive(true);
                UpDownUI[1].sprite = conditionImages[0];
                UpDownUI[0].sprite = diceImages[int.Parse(c[1]) - 1];

                break;

            case "ÀÌÇÏ":

                UpDownUI[0].gameObject.SetActive(true);
                UpDownUI[1].sprite = conditionImages[1];
                UpDownUI[0].sprite = diceImages[int.Parse(c[1]) - 1];

                break;
        }
    }
}
