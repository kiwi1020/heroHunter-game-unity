using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    private MapTile mapTile;

    public GameObject playerPrefab; //플레이어 프리팹 설정
    public GameObject StartTile; //시작 타일

    private Vector2 playerPosition; //플레이어 위치(x,y)
    private Vector2 startPoint; //시작지점
    private Vector2 endPoint; //종료지점(보스 타일)
    
    // Start is called before the first frame update
    void Start()
    {
        mapTile = GetComponent<MapTile>();
        setupMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //타일 생성 및 플레이어 생성
    void setupMap()
    {
        //시작 타일위치 설정
        Transform startTileForm = StartTile.transform;
        //플레이어 생성
        GameObject player = Instantiate(playerPrefab,startTileForm);

    }

    //플레이어 이동
    void PlayerMove()
    {
        //이동카드 효과에 따른 이동
    }
    //플레이어 위치 저장
    void SetPlayerPosition()
    {
        playerPosition = playerPrefab.transform.position;
    }
}
