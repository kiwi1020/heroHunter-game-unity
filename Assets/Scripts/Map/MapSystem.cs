using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    [SerializeField] GameObject tileParents;

    public GameObject playerPrefab; //플레이어 프리팹 설정
    public GameObject tilePrefab;

    public static bool moveCardDraw; // 카드 드로우 가능 여부

    private Vector2 playerPosition; //플레이어 위치(x,y)
    private Vector2 startPoint; //시작지점
    private Vector2 endPoint; //종료지점(보스 타일)

    public List<MapTile> tileMap = new List<MapTile>();

    GameObject player;
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    public static int tileCount = 1;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        setupMap();
    }

    //타일 생성 및 플레이어 생성
    void setupMap()
    {
        moveCardDraw = true;
        for (int i = 0; i<20; i++)
        {
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;
            tile.SetTile( DataManager.instance.AllTileDatas // 무작위 타일 데이터 설정
                [ DataManager.instance.AllTileList[ Random.Range(0, DataManager.instance.AllTileList.Count) ]]);

            if (i == 0) tile.transform.position = new Vector3(-5, -3, 0);
            else
            {
                var lastTilePosition = tileMap[tileMap.Count-1].transform.position;
                tile.transform.position = new Vector3(
                    lastTilePosition.x + 3, //+ Random.Range(-1, 1f)
                    lastTilePosition.y + 2); //+ Random.Range(-1, 1f));
                //tile.transform.Rotate(new Vector3(0, 0, Random.Range(-5, 5f)));
                //일단 타일 배치는 일정하게 함
            }

            tileMap.Add(tile);
        }

        //플레이어 생성
        player = Instantiate(playerPrefab, tileMap[0].transform.position, tileMap[0].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
   
    }
    //플레이어 이동
    public void PlayerMove()
    {
        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform; //플레이어 위치
        endpos = tileMap[tileCount].transform; //이동할 타일 위치
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2); // 플레이어, 타일 중간 위치
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
        new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
        new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        //이동 경로(topPos.y + 값으로 점프 높이 조절)
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D);

    }
    //플레이어 위치 저장
    void SetPlayerPosition()
    {
        playerPosition = playerPrefab.transform.position;
    }

}
