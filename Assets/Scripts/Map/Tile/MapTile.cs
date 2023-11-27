using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public TextMeshPro tileName; //타일이름 Text
    public Button startButton; //배틀시작 버튼
  
    //타일이름 설정, 이름은 정한 후 수정
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;       
        //시작타일은 타일 이름이 없도록 수정함
        if(tileData.name == "시작")
        {
            tileName.text = "";
        }
        else
        {
            tileName.text = tileData.name;
        }
    }
    public void TileEffect()
    {     
        PlayManager.instance.curTile = tileData;
        switch (tileData.type)
        {
            case "전투":
                OnBattleStartButton();
                break;

            case "선택":
                MapSystem.instance.selectEvent.SetEvent(this);
                break;

            case "함정":
                MapSystem.instance.gainEvent.SetEvent(this);
                DeleteBattleCard();
                break;

            case "보상":
                MapSystem.instance.gainEvent.SetEvent(this);
                GetBattleCard();
                break;

            default:               
                break;

        }
    }

    //배틀시작 버튼
    public void OnBattleStartButton()
    {
        //battleSystem.StartBattle();
        //tileName.gameObject.SetActive(false);
        //startButton.gameObject.SetActive(false);

        SceneManager.LoadScene("PlayScene"); // 임시로 바로 시작
    }
    public void GetBattleCard()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject cardObject = GameObject.Find("BattleCard").transform.GetChild(i).gameObject;
            print(cardObject.name);

            string[] BattleCardNames = new string[] { "갈라치기", "급소 찌르기", "뒤통수치기", "뺨때리기", "성급한판단", "속사", "비열한 찌르기" };

            // 랜덤으로 BattleCardNames에서 카드 이름을 선택
            string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

            // 선택한 카드 이름으로 실제 데이터를 얻어옴
            BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];
            print(card.name);

            // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
            var battleCard = cardObject.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            PlayerData.playerBattleCardDeck.Add(card);
        }
    }
    public void DeleteBattleCard()
    {   
        if (PlayerData.playerBattleCardDeck.Count != 0)
        {
            PlayerData.playerBattleCardDeck.RemoveAt(
                Random.Range(0, PlayerData.playerBattleCardDeck.Count));
        }
    }
    public void SelectEvent()
    {

    }
}
