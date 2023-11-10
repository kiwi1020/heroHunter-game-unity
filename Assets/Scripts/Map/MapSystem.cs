using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    public static int tileCount = 0;

    public static bool moveCardDraw;
    public static bool jumpState = false;

    public SelectEvent selectEvent;

    public GameObject tilePrefab;
    public GameObject playerPrefab;

    [SerializeField] Camera mainCam;
    [SerializeField] GameObject background;
    [SerializeField] GameObject tileParents;

    public List<MapTile> tileMap = new List<MapTile>();

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
        for (int i = 0; i<20; i++)
        {
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;
            tile.SetTile( DataManager.instance.AllTileDatas 
                [ DataManager.instance.AllTileList[ Random.Range(0, DataManager.instance.AllTileList.Count) ]]);

            if (i == 0) tile.transform.position = new Vector3(-5, -3, 0);
            else
            {
                var lastTilePosition = tileMap[tileMap.Count-1].transform.position;
                tile.transform.position = new Vector3( lastTilePosition.x + 3.5f, lastTilePosition.y + 2.5f);
            }
            tileMap.Add(tile);
        }

        
        player = Instantiate(playerPrefab, tileMap[0].transform.position, tileMap[0].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
   
    }
    
    public void PlayerMove(int _n = 0)
    {
        if(0 < _n)
        {
            PlayerMoveFoward(_n);
        }
    }

    void PlayerMoveFoward(int _stack)
    {
        if (_stack == 0)
        {
            EndPlayerMove();
            return;
        }

        _stack--;
        tileCount++;

        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform;
        endpos = tileMap[tileCount].transform;
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2);
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
            new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
            new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMoveFoward(_stack));
    }


    void EndPlayerMove()
    {
        print("call");
        tileMap[tileCount].TileEffect();
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
