using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    public static int tileCount = 1;

    public static bool moveCardDraw;
    public static bool jumpState = false;

    public SelectEvent selectEvent;

    public GameObject tilePrefab;
    public GameObject playerPrefab;

    [SerializeField] Camera mainCam;
    [SerializeField] GameObject background;
    [SerializeField] GameObject tileParents;

    public List<MapTile> tileMap = new List<MapTile>();
   
    public int curTileNum
    {
        get
        {
            return PlayManager.instance.curTileNum;
        }
        set
        {
            PlayManager.instance.curTileNum = value;
        }
    }

    #region player
    GameObject player;
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    Vector3 playerPosition;
    #endregion

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

    void Update()
    {
        if (tileMap[tileCount].transform.position == player.transform.position)
        {
            jumpState = false;
        }
    }
    
    void setupMap()
    {
        moveCardDraw = true;

        GenerateTileObjects(20);
        SetTileMapData();
        MoveCameraToTargetTile(tileMap[curTileNum]);

        player = Instantiate(playerPrefab, tileMap[curTileNum].transform.position, tileMap[curTileNum].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
    }

    void SetTileMapData()
    {
        //없는 경우

        if(PlayManager.instance.tileMapData.Count < tileMap.Count)
        {
            for(int i = 0; i< tileMap.Count; i++)
            {
                if (i < PlayManager.instance.tileMapData.Count) continue;

                var tileData = DataManager.instance.AllTileDatas[DataManager.instance.AllTileList[Random.Range(0, DataManager.instance.AllTileList.Count)]];
                PlayManager.instance.tileMapData.Add(tileData);
            }
        }

        for (int i = 0; i < tileMap.Count; i++) tileMap[i].SetTile(PlayManager.instance.tileMapData[i]);
    }

    void GenerateTileObjects(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;

            if (i == 0)
            {
                tile.transform.position = new Vector3(-5, -3, 0);
            }
            else
            {
                var lastTilePosition = tileMap[tileMap.Count - 1].transform.position;
                tile.transform.position = new Vector3(lastTilePosition.x + 3.5f, lastTilePosition.y + 2.5f);
            }

            tile.SetTile(DataManager.instance.AllTileDatas
                [DataManager.instance.AllTileList[Random.Range(0, DataManager.instance.AllTileList.Count)]]);
            tileMap.Add(tile);
        }
    }
    
    public void PlayerMove(int _n = 0)
    {
        if (0 < _n)
        {
            PlayerMoveFoward(_n);
        }
        else if(_n < 0)
        {

        }
        else
        {
            EndPlayerMove();
        }
    }

    void PlayerMoveFoward(int _stack)
    {

        _stack--;
        curTileNum++;
        MoveCameraToTargetTile(tileMap[curTileNum]);

        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform;
        endpos = tileMap[curTileNum].transform;
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2);
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
            new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
            new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack));
    }


    void EndPlayerMove()
    {
        tileMap[curTileNum].TileEffect();
    }

    void MoveCameraToTargetTile(MapTile _mapTile)
    {
        if (curTileNum < 2) return;
        Camera.main.transform.DOMove(new Vector3(_mapTile.transform.position.x, _mapTile.transform.position.y, Camera.main.transform.position.z), 1.5f);
    }

    void SetPlayerPosition()
    {
        {
            playerPosition = playerPrefab.transform.position;
        }

        Vector3 camtargetPos =  mainCam.transform.position + new Vector3(3, 2, 0); 
        Vector3 bgtargetPos = background.transform.position + new Vector3(3, 2, 0); 

        mainCam.transform.DOMove(camtargetPos, 1); 
        background.transform.DOMove(bgtargetPos, 1);    
     
    }
    
    
}
