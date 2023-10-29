using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TILETYPE { StartTile, GoblinTile, TombTile, LuckTile, BossTile } //타일 타입 추가
public class MapTile : MonoBehaviour
{
    public TileData tiledata;

    public TILETYPE tileType; 
    public GameObject EnemyPrefab; //적 프리팹 설정
    public Text tileName; //타일이름 Text
    public Button startButton; //배틀시작 버튼

    private BattleSystem battleSystem; 
    
    void Start()
    {
        battleSystem = GetComponent<BattleSystem>();
        
    }
    //타일이름 설정, 이름은 정한 후 수정
    public void SetTileName()
    {
        switch (tileType)
        {
            case TILETYPE.GoblinTile:
                tileName.text = "고블린 서식지";
                break;
            case TILETYPE.TombTile:
                tileName.text = "해골 무덤";
                break;
            case TILETYPE.LuckTile:
                tileName.text = "행운";
                break;
                // 다른 타일 타입에 따른 처리
        }
    }
    public void TileEffect()
    {
        if(tileType == TILETYPE.GoblinTile)
        {
            startButton.gameObject.SetActive(true);
            //고블린 전두 시작
        }
        if(tileType == TILETYPE.TombTile)
        {
            startButton.gameObject.SetActive(true);
            //무덤 scene 이동
        }
        if (tileType == TILETYPE.LuckTile)
        { 
            //행운 효과 발동(무작위로 덱 2개 지급)
        }
        //카드 종류 정해지면 추가 예정
    }

    //타일 위치 지정, 만일 타일 미리 배치할 경우 삭제
    public void SetTilePosition(int x, int y)
    {
        gameObject.transform.position = new Vector2(x, y);
    }

    //타일 Trasform 반환, 위와 동일
    public Transform GetTileTransform()
    {
        return gameObject.transform;
    }

    //배틀시작 버튼
    public void OnBattleStartButton()
    {
        battleSystem.StartBattle();
        tileName.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }
}
