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
    public static int tileCount = 1;

    /*
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    */

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        setupMap(); //스타트에서 빼고
    }

    //타일 생성 및 플레이어 생성
    void setupMap()
    {
        moveCardDraw = true;
        for (int i = 0; i<20; i++)
        {
            #region 맵 데이터 생성 생성
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;
            tile.SetTile( DataManager.instance.AllTileDatas // 무작위 타일 데이터 설정
                [ DataManager.instance.AllTileList[ Random.Range(0, DataManager.instance.AllTileList.Count) ]]);

            #endregion

            #region 맵 오브젝트 생성
            if (i == 0) tile.transform.position = new Vector3(-5, -3, 0);
            else
            {
                var lastTilePosition = tileMap[tileMap.Count-1].transform.position;
                tile.transform.position = new Vector3( lastTilePosition.x + 3.5f, lastTilePosition.y + 2.5f);
            }
            tileMap.Add(tile);
            #endregion
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
        player.transform.DOMoveX(tileMap[tileCount].transform.position.x, 1);
        player.transform.DOMoveY(tileMap[tileCount].transform.position.y, 1).SetEase(Ease.InOutBack).OnComplete(()=>EndPlayerMove());

        /*
        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform; //플레이어 위치
        endpos = tileMap[tileCount].transform; //이동할 타일 위치
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2); // 플레이어, 타일 중간 위치
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
        new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
        new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        //이동 경로(topPos.y + 값으로 점프 높이 조절)
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic);
        */
    }

    void EndPlayerMove()
    {
        tileMap[tileCount].TileEffect();
    }


    //플레이어 위치 저장
    void SetPlayerPosition()
    {
        playerPosition = playerPrefab.transform.position;
    }

}
