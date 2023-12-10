using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    public bool moveCardDraw;
    public bool allowEffect = true;

    public static int readyCount = 0;

    public TileEvent tileEffect_UI;

    public GameObject tilePrefab, playerPrefab;
    public GameObject cardHideButton;

    [SerializeField] Camera mainCam;
    [SerializeField] GameObject background;
    [SerializeField] GameObject tileParents;
    [SerializeField] TextMeshProUGUI ReadyCountText;
    [SerializeField] GameObject EndingUI;

    public DiceLook diceLook;
    public LostItems lostItems;

    public PopUp popUpObj;

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
        if (PlayManager.instance.IsFirst == false) { 
            ResetTileMap();
            ResetWeight();
            PlayManager.instance.IsFirst = true;
            ReadyCountText.text = "0";
        }
        setTileWeight();
        setupMap();
        ReadyCountText.text = readyCount.ToString();
        if(curTileNum != 0) GainBattlePrize();
    }

    void GainBattlePrize()
    {
        if (PlayManager.instance.isClear)
        {
            EndingUI.SetActive(true);
            PlayManager.instance.isClear = false;
            PlayManager.instance.ResetPlayerData();
        }
        else if (PlayManager.instance.IsFirst)
        {
            if (PlayManager.instance.isStone)
            {
                tileEffect_UI.gameObject.SetActive(true);
                tileEffect_UI.GainTreasure(3);
            }
            else
            {
                tileEffect_UI.gameObject.SetActive(true);
                tileEffect_UI.GainTreasure(1);
                //PlayerData.diceCount++; 다이스 개수는 유실물 얻어서 늘리기
            }
        }
    }
    #region Reset
    void ResetTileMap()
    {
        curTileNum = 0;
        PlayManager.instance.tileMapData = new List<TileData>();
        foreach (Transform child in tileParents.transform)
        {
            Destroy(child.gameObject);
        }
        tileMap = new List<MapTile>();
    }

    void ResetWeight()
    {
        string[] TileName = new string[] {"낭떠러지","신비한 석상", "떠돌이 상인", "도박장", "행운","늪지대","풀숲","정글",
            "호수","뒷산","동굴","숲 입구","깊은 숲속"};
        foreach (string name in TileName)
        {
            PlayManager.instance.wrPicker.Remove(name);
        }
        PlayManager.instance.startWeigtTile = true;

    }   
    #endregion
    public void PlayerDataSetting()
    {
        diceLook.SetDicePool();
        lostItems.SetLostItems();
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
    void SetTileMapData()
    {
        if(PlayManager.instance.tileMapData.Count < tileMap.Count)
        {

            int stack = 0;

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
                else
                {
                    var tileData = DataManager.instance.AllTileDatas[GetRandomTile()];

                    do
                    {
                        if (PlayManager.instance.tileMapData[i - 1].type != tileData.type) stack = 0;
                        else tileData = DataManager.instance.AllTileDatas[GetRandomTile()];
                    }
                    while (stack >= 2);
                    PlayManager.instance.tileMapData.Add(tileData);

                    if (PlayManager.instance.tileMapData[i - 1].type == tileData.type) stack += 1;

                    UpdateWeightTile();
                    
                }

            }

        }

        //저장한 타일 데이터를 무므씬 타일에 가져오기
        for (int i = 0; i < tileMap.Count; i++) tileMap[i].SetTile(PlayManager.instance.tileMapData[i]);
    }
    #region TileWeight
    void setTileWeight()
    {
        if (PlayManager.instance.startWeigtTile)
        {
            string[] TileName = new string[] { "낭떠러지","신비한 석상", "떠돌이 상인", "도박장", "행운","늪지대","풀숲","정글",
            "호수","뒷산","동굴","숲 입구","깊은 숲속"};
            foreach (string name in TileName)
            {
                int TileWeight = int.Parse(DataManager.instance.AllTileDatas[name].weight);
                PlayManager.instance.wrPicker.Add(name, TileWeight);
            }
            PlayManager.instance.startWeigtTile = false;
        }
    }

    public void UpdateWeightTile()
    {
        string[] TileName = new string[] { "낭떠러지","신비한 석상", "떠돌이 상인", "도박장", "행운","늪지대","풀숲","정글",
            "호수","뒷산","동굴","숲 입구","깊은 숲속"};
        foreach (string name in TileName)
        {
            string tileRate = DataManager.instance.AllTileDatas[name].rate;
            double tileWeight = PlayManager.instance.wrPicker.GetWeight(name);
            switch (tileRate) 
            {
                //가중치 증가폭은 밸런스 수정
                case "A":
                    PlayManager.instance.wrPicker.ModifyWeight(name, tileWeight + 60);
                    break;
                case "B":
                    PlayManager.instance.wrPicker.ModifyWeight(name, tileWeight + 50);
                    break;
                case "C":
                    PlayManager.instance.wrPicker.ModifyWeight(name, tileWeight - 15);
                    break;
                case "D":
                    PlayManager.instance.wrPicker.ModifyWeight(name, tileWeight - 30);
                    break;
                default: 
                    break;
            }
        }
    }
    string GetRandomTile()
    {       
        return PlayManager.instance.wrPicker.GetRandomPick(); ;
    }
    #endregion

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
                    ReadyCountText.text = "0";
                    PlayerMove(moveValue + readyCount,_moveCard);                   
                }
                else
                {
                    var moveValue = int.Parse(_eft[1]);
                    ReadyCountText.text = "0";
                    PlayerMove(moveValue + readyCount, _moveCard);               
                }
                break;

            case "추격":
                if (PlayManager.instance.tileMapData[curTileNum].name == "보스" ||
                    PlayManager.instance.tileMapData[curTileNum].name == "보스")
                {
                    _moveCard.MoveEffect();
                }
                else
                {
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
                        PlayerMove(1, _moveCard);
                    }
                    else
                    {
                        _moveCard.MoveEffect();
                    }
                }
                break;
            // 수정 필요함
            case "준비":
                readyCount = int.Parse(_eft[1]);
                ReadyCountText.text = readyCount.ToString();
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

            default:
                break;
        }
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(2);

    }

    #region PlayerMove
    public void PlayerMove(int _n, MoveCard _moveCard)
    {
        if (PlayManager.instance.tileMapData[curTileNum].name == "보스")
        {
            _moveCard.MoveEffect();
        }        
        else if (_n >0)
        {
            PlayerMoveFoward(_n, _moveCard);
        }
        else if(_n < 0)
        {
            if (PlayManager.instance.tileMapData[curTileNum].name == "시작")
            {
                _moveCard.MoveEffect();
            }
            else
            {
                PlayerMoveBack(_n, _moveCard);
            }
        }
        else 
        {
            if (readyCount != 0) readyCount = 0;
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
    #endregion
    public void EndCardEffect()
    {
        tileMap[curTileNum].TileEffect();     
    }
    void MoveCameraToTargetTile(MapTile _mapTile)
    {
        if (curTileNum < 2) return;
        Camera.main.transform.DOMove(new Vector3(_mapTile.transform.position.x, _mapTile.transform.position.y, Camera.main.transform.position.z), 1.5f);
    }
    public void ExitGame()
    {
        PlayManager.instance.IsFirst = false;   
        SceneManager.LoadScene("StartScene");
    }
}
