using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            case "�̵�":
                MapSystem.instance.PlayerMove(int.Parse(eft[1]));
                if (eft[1] == "-3~3")
                {
                    var moveValue = Random.Range(-3, 3);
                    MapSystem.instance.PlayerMove(moveValue);
                }
                break;
            case "ȸ��":   
                if(PlayerData.currentHP == 100)
                {
                    break;
                }
                else
                {
                    PlayerData.currentHP += 10;
                    break;
                };
            case "�غ�":

                break;
            case "����":
                break;
            default:
                MapSystem.instance.PlayerMove(0); 

        }

        for (int i = 0; i <= CardsHand.transform.childCount - 1; i++)
        {
            CardsHand.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
  
}
