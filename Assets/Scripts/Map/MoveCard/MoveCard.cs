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

    List<string> remainEffect = new List<string>();

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

    }

    //카드 클릭 시 발동
    public void SelectCard()
    {
        remainEffect = moveCardData.effects;

        MoveEffect(remainEffect.Count); //카드가 가지고 있는 효과, 2개가 잇으면 2부터 효과를 발동할때마다 1씩 감소하는 메서드

    }

    public void MoveEffect(int _stack)
    {
        MapSystem.instance.ActMoveCardEffect(remainEffect[0].Split(':'), this);
        // this : 효과를 MapSystem에서 실행하고, 더 실행할 효과가 남았을 때 다시 돌아올 수 있도록
        // remainEffect[0].Split(':') : 0번 인덱스 실행하고 삭제하면 그 다음 효과가 0번이 됨

        /*
        CardsHand = transform.parent.gameObject;

        var eft = moveCardData.effects[0].Split(':');
        string[] eft2 = null;

        switch (eft[0])
        {
            #region 이동카드 조건
            case "이동":

                if (eft[1].Contains('~'))
                {
                    var eftValue = eft[1].Split('~').Select(x => int.Parse(x)).ToArray();
                    var moveValue = Random.Range(eftValue[0], eftValue[1] + 1);
                    MapSystem.instance.PlayerMove(moveValue);
                }

                if(moveCardData.effects.Count > 1)
                {
                    eft2 = moveCardData.effects[1].Split(':');

                    if (eft2[0].Contains("추격"))
                    {
                        int[] value = { -1, 1 };
                        int moveDirection = 0;

                        int initialTileNum = MapSystem.curTileNum;
                        int finalTileNum = initialTileNum + int.Parse(eft[1]);

                        MapSystem.instance.PlayerMove(int.Parse(eft[1]));

                        if (finalTileNum == MapSystem.curTileNum)
                        {
                            if (PlayManager.instance.tileMapData[MapSystem.curTileNum - 1].type == "전투" &&
                                PlayManager.instance.tileMapData[MapSystem.curTileNum + 1].type == "전투")
                            {
                                // 두 방향에 모두 전투 타일이 있는 경우 랜덤으로 왼쪽 또는 오른쪽으로 이동
                                moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
                            }
                            else if (PlayManager.instance.tileMapData[MapSystem.curTileNum - 1].type == "전투")
                            {
                                // 왼쪽에 전투 타일이 있는 경우 왼쪽으로 이동
                                moveDirection = -1;
                            }
                            else if (PlayManager.instance.tileMapData[MapSystem.curTileNum + 1].type == "전투")
                            {
                                // 오른쪽에 전투 타일이 있는 경우 오른쪽으로 이동
                                moveDirection = 1;
                            }
                            MapSystem.instance.PlayerMove(value[moveDirection]);
                        }
                    }
                    else if (eft2[0].Contains("준비"))
                    {
                        MapSystem.instance.PlayerMove(int.Parse(eft[1]));
                    }
                    else if (eft2[0].Contains("추격"))
                    {
                        MapSystem.instance.PlayerMove(int.Parse(eft[1]));
                    }
                   
                }              
                
                else
                {
                    MapSystem.instance.PlayerMove(int.Parse(eft[1]));
                }
                break;
            #endregion

            case "회복":
                if (PlayerData.currentHP < 100)
                {
                    PlayerData.currentHP += 10;
                }
                break;

            default:
                MapSystem.instance.PlayerMove(0);
                break;
        }

        for (int i = 0; i <= CardsHand.transform.childCount - 1; i++)
        {
            CardsHand.transform.GetChild(i).gameObject.SetActive(false);
        }
        */
    }

}
