using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
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
        foreach (string i in moveCardData.effects)
        {
            desText.text += i;
            desText.text += "\n";
        }

            //ī�� ���� �����ͼ� �ش� ī�� �̹����� �ؽ�Ʈ ����
            //�̰Ŵ� �׳� ī�忡 ����ī�� �ְ� �ű⼭ �����ص� �� ��?
        }
    public void MoveEffect()
    {
        CardsHand = transform.parent.gameObject;

        var eft = moveCardData.effects[0].Split(':');

        switch (eft[0])
        {
            case "이동":
                if(eft[1].Contains('~'))
                {
                    var eftValue = eft[1].Split('~').Select(x => int.Parse(x)).ToArray();
                    var moveValue = Random.Range(eftValue[0], eftValue[1]);
                    MapSystem.instance.PlayerMove(moveValue);
                }
                else
                {
                    MapSystem.instance.PlayerMove(int.Parse(eft[1]));
                }

                /*
                if (eft[1] == "-3~3")
                {
                }
                */
                break;
            case "회복":   
                if(PlayerData.currentHP == 100)
                {
                    break;
                }
                else
                {
                    PlayerData.currentHP += 10;
                    break;
                };
            default:
                MapSystem.instance.PlayerMove(0);
                break;

        }

        for (int i = 0; i <= CardsHand.transform.childCount - 1; i++)
        {
            CardsHand.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
  
}
