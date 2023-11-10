using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    [SerializeField] GameObject tileParents;
    [SerializeField] GameObject background;
    [SerializeField] Camera mainCam;

    public GameObject playerPrefab; //�÷��̾� ������ ����
    public GameObject tilePrefab;

    public static bool moveCardDraw; // ī�� ��ο� ���� ���� 

    public List<MapTile> tileMap = new List<MapTile>(); 

    GameObject player;
    public static int tileCount = 1;

    
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    Vector3 playerPosition;

    public static bool jumpState = false;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        setupMap(); //��ŸƮ���� ����
    }
    private void Update()
    {
        if (tileMap[tileCount].transform.position == player.transform.position)
        {
            jumpState = false;
        }
    }
    //Ÿ�� ���� �� �÷��̾� ����
    void setupMap()
    {
        moveCardDraw = true;
        for (int i = 0; i<20; i++)
        {
            #region �� ������ ���� ����
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;
            tile.SetTile( DataManager.instance.AllTileDatas // ������ Ÿ�� ������ ����
                [ DataManager.instance.AllTileList[ Random.Range(0, DataManager.instance.AllTileList.Count) ]]);

            #endregion

            #region �� ������Ʈ ����
            if (i == 0) tile.transform.position = new Vector3(-5, -3, 0);
            else
            {
                var lastTilePosition = tileMap[tileMap.Count-1].transform.position;
                tile.transform.position = new Vector3( lastTilePosition.x + 3.5f, lastTilePosition.y + 2.5f);
            }
            tileMap.Add(tile);
            #endregion
        }

        //�÷��̾� ����
        player = Instantiate(playerPrefab, tileMap[0].transform.position, tileMap[0].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
   
    }
    //�÷��̾�, ī�޶�, �� �̵�
    public void PlayerMove()
    {

        //�÷��̾� �̵�
        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform; //�÷��̾� ��ġ
        endpos = tileMap[tileCount].transform; //�̵��� Ÿ�� ��ġ
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2); // �÷��̾�, Ÿ�� �߰� ��ġ
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
        new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
        new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        //�̵� ���(topPos.y + ������ ���� ���� ����)
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic);
        
    }

    void EndPlayerMove()
    {
        tileMap[tileCount].TileEffect();
    }


    //�÷��̾� ��ġ ����
    void SetPlayerPosition()
    {
    {
        playerPosition = playerPrefab.transform.position;
    }

        //��, canvas �̵�(�̵� ��ǥ ���� �ʿ�)
        Vector3 camtargetPos =  mainCam.transform.position + new Vector3(3, 2, 0); //ī�޶� �̵� ��ǥ
        Vector3 bgtargetPos = background.transform.position + new Vector3(3, 2, 0); //��� �̵� ��ǥ

        mainCam.transform.DOMove(camtargetPos, 1); //ī�޶� �̵�
        background.transform.DOMove(bgtargetPos, 1); //��� �̵�    
     
    }
    
    
}
