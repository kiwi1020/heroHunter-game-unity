using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Linq;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    public bool moveCardDraw;
    public bool allowHealing = true;
    public bool allowEffect = true;

    public static int readyCount;

    public TileEvent tileEffect_UI;

    public GameObject tilePrefab, playerPrefab;
    public GameObject cardHideButton;

    [SerializeField] Camera mainCam;
    [SerializeField] GameObject background;
    [SerializeField] GameObject tileParents;


    public List<MapTile> tileMap = new List<MapTile>();
   
    public static int curTileNum
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
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #region player
    GameObject player;
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    #endregion

    void Start()
    {
        setupMap();       
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
        //처음 1회만 실행하여 타일 데이터를 생성
        //수정(시작,보스, 조건)
        if(PlayManager.instance.tileMapData.Count < tileMap.Count)
        {
            //string previousTileType;
            //int BattleCount = 0; 


            for(int i = 0; i< tileMap.Count; i++)
            {              
                
                //처음: 시작, 끝: 보스 타일로 고정
                if (i == 0)
                {
                    var tileData = DataManager.instance.AllTileDatas["시작"];
                    PlayManager.instance.tileMapData.Add(tileData);
                }
                else if (i == tileMap.Count - 1)
                {
                    var tileData = DataManager.instance.AllTileDatas["보스"];
                    PlayManager.instance.tileMapData.Add(tileData);
                }
                //동일한 유형(타입)의 연속 타일 방지(완료)
                //->
                //전투는 연속 타일 가능(3번까지만, 완료)
                //->
                //음..
                else
                {
                    //타일 이벤트 확인할 때 사용(삭제 예정)
                    var tileData = DataManager.instance.AllTileDatas["경비대"];
                    PlayManager.instance.tileMapData.Add(tileData);
                    
                    /*
                    previousTileType = PlayManager.instance.tileMapData[i-1].type;
                    do
                    {
                       
                        var tileData = DataManager.instance.AllTileDatas[GetRandomTile()];
                        string currentTileType = tileData.type;
                       
                        if (previousTileType == "전투" && currentTileType == "전투")
                        {
                            BattleCount++;                           
                            if (BattleCount > 2)
                            {                                                                
                                continue;
                            }
                            else { 
                                PlayManager.instance.tileMapData.Add (tileData);
                                break;
                            }
                        }
                        
                        else if(previousTileType != currentTileType)
                        {
                            PlayManager.instance.tileMapData.Add (tileData);
                            BattleCount = 0;
                            break;
                        }
                    }
                    while (true);  
                    */
                }

            }
        }

        //저장한 타일 데이터를 무므씬 타일에 가져오기
        for (int i = 0; i < tileMap.Count; i++) tileMap[i].SetTile(PlayManager.instance.tileMapData[i]);
    }
    private string GetRandomTile()
    {
        string[] TileName = new string[] { "낭떠러지", "경비대", "신비한 석상", "무덤", "재래시장", "늪지대", "설산", "도적떼", "도박장", "행운" };

        var wrPicker = new WeightRandomPick<string>();

        foreach (string name in TileName)
        {
            int TileWeight = int.Parse(DataManager.instance.AllTileDatas[name].weight);
            wrPicker.Add(name, TileWeight);
        }

        return wrPicker.GetRandomPick(); ;
    }

    void GenerateTileObjects(int _count)
    {
        //무브씬에 타일 위치를 정하고 생성
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
            tileMap.Add(tile);
        }
    }

    public void ActMoveCardEffect(string[] _eft, MoveCard _moveCard)
    {
        cardHideButton.SetActive(false);

        switch (_eft[0])
        {
            case "이동":

                if (_eft[1].Contains('~'))
                {
                    var eftValue = _eft[1].Split('~').Select(x => int.Parse(x)).ToArray();
                    var moveValue = Random.Range(eftValue[0], eftValue[1] + 1);
                    PlayerMove(moveValue + readyCount,_moveCard);                   
                }
                else
                {
                    var moveValue = int.Parse(_eft[1]);
                    PlayerMove(moveValue + readyCount, _moveCard);               
                }
                break;

            case "추격":
                                    
                if (PlayManager.instance.tileMapData[curTileNum - 1].type == "전투" &&
                        PlayManager.instance.tileMapData[curTileNum + 1].type == "전투")
                {
                    // 두 방향에 모두 전투 타일이 있는 경우 랜덤으로 왼쪽 또는 오른쪽으로 이동
                    PlayerMove(Random.Range(0, 2) == 0 ? -1 : 1, _moveCard);
                }
                else if (PlayManager.instance.tileMapData[curTileNum - 1].type == "전투")
                {
                    // 왼쪽에 전투 타일이 있는 경우 왼쪽으로 이동
                    PlayerMove(-1, _moveCard);
                }
                else if (PlayManager.instance.tileMapData[curTileNum + 1].type == "전투")
                {
                    // 오른쪽에 전투 타일이 있는 경우 오른쪽으로 이동
                    PlayerMove(1,_moveCard);
                }
                else
                {
                    _moveCard.MoveEffect();
                }
                
                break;

            case "회복":              
                if (PlayerData.currentHP < 100 && allowHealing)
                {
                    PlayerData.currentHP += 10;
                    _moveCard.MoveEffect();                
                }
                moveCardDraw = true;
                break;

            // 수정 필요함
            case "준비":
                readyCount = int.Parse(_eft[1]);
                moveCardDraw = true; 
                _moveCard.MoveEffect();
                break;
           // 수정 필요함
            case "무시":
                if (_moveCard.moveCardData.name == "조심스러운 발걸음")
                {
                    if(PlayManager.instance.tileMapData[curTileNum].type == "함정")
                    {                        
                        moveCardDraw = true;
                        allowEffect = false;
                    }
                    else
                    {                        
                        EndCardEffect();
                    }
                }
                else if (_moveCard.moveCardData.name == "전략적 후퇴")
                {
                    allowEffect = false;
                    moveCardDraw = true;
                }
                break;

            case "제약":
                allowHealing = false;            
                break;
            default:
                break;
        }
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(2);

    }
    
    public void PlayerMove(int _n, MoveCard _moveCard)
    {
        if (_n >0)
        {
            PlayerMoveFoward(_n, _moveCard);
        }
        else if(_n < 0)
        {
            PlayerMoveBack(_n, _moveCard);
        }
        else 
        {
            if (readyCount > 0) readyCount = 0;
            _moveCard.MoveEffect();         
        }
    }

    void PlayerMoveFoward(int _stack, MoveCard _moveCard)
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
        playerRb.DOPath(JumpPath, 1f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack, _moveCard));
    }

    void PlayerMoveBack(int _stack, MoveCard _moveCard)
    {
        _stack++;
        curTileNum--;
        MoveCameraToTargetTile(tileMap[curTileNum]);

        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform;
        endpos = tileMap[curTileNum].transform;
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2);
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
            new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
            new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        playerRb.DOPath(JumpPath, 1f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack, _moveCard));
    }

    public void EndCardEffect()
    {
        tileMap[curTileNum].TileEffect();     
    }

    void MoveCameraToTargetTile(MapTile _mapTile)
    {
        if (curTileNum < 2) return;
        Camera.main.transform.DOMove(new Vector3(_mapTile.transform.position.x, _mapTile.transform.position.y, Camera.main.transform.position.z), 1.5f);
    }
}
