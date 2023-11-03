using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public GameObject EnemyPrefab; //적 프리팹 설정
    public Text tileName; //타일이름 Text
    public Button startButton; //배틀시작 버튼

    [SerializeField] string tname; //*

    
    void Start()
    {
    }
    //타일이름 설정, 이름은 정한 후 수정
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;

        tname = tileData.name; //*
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
                startButton.gameObject.SetActive(true);
                break;

            case "함정":
                break;

            case "보상":
                break;

            default:
                break;

        }
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
        //battleSystem.StartBattle();
        //tileName.gameObject.SetActive(false);
        //startButton.gameObject.SetActive(false);

        SceneManager.LoadScene("PlayScene");
    }
}
