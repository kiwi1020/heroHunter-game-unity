using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;
    private string BtnText;
    #region SerializeFiled
    [SerializeField] List<TextMeshProUGUI> UIText;
    [SerializeField] TextMeshProUGUI resultText; 
    [SerializeField] GameObject OddEvenGame;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Option;
    [SerializeField] GameObject MoveBar;
    #endregion
    #region List<>
    public List<GetBattleCard> getBattleCards;
    public List<LostItemCard> lostItems;
    public static List<BattleCardData> getbattleCardDatas = new List<BattleCardData>();
    public static List<LostItem> getLostItmeCardDatas = new List<LostItem>();
    #endregion
    #region Bool
    private bool isSpinning = false;
    private bool isLostItem = false;
    #endregion
    #region Awake()
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }
    #endregion
    #region TileEvent Setting

    public void SetEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);
        mapTile = _mapTile;
        SetEventUI(mapTile);
    }

    public void SetEventUI(MapTile _mapTile)
    {
        UIText[0].text = _mapTile.tileData.title;
        UIText[1].text = _mapTile.tileData.desc;

        switch (_mapTile.tileData.name)
        {
            case "신비한 석상":
                Options.SetActive(true);
                break;
            case "재래시장":
                SetGetCard(_mapTile.tileData.cardCount[1]);
                break;
            case "도박장":
                OddEvenGame.SetActive(true);
                break;
            case "행운":
                SetGetCard(_mapTile.tileData.cardCount[1]);
                break;
            case "낭떠러지":
                SetGetCard(_mapTile.tileData.cardCount[1]);
                break;
        }
    }
    public void SetGetCard(int _n)
    {
        // 카드 뿌리기
        for (int i = 0; i < _n; i++)
        {
            //재래시장
            if (mapTile.tileData.name == "재래시장")
            {
                lostItems[i].gameObject.SetActive(true);
                float xOffset = CalculateXOffset(_n, i);
                lostItems[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

                GameObject getLostItemcard = lostItems[i].gameObject;
                LostItem card;

                string randomName = DataManager.instance.AllLostItemList[Random.Range(0, DataManager.instance.AllLostItemDatas.Count)];
                card = DataManager.instance.AllLostItemDatas[randomName];

                // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
                var lostcard = getLostItemcard.GetComponent<LostItemCard>();
                lostcard.SetCard(card);

                isLostItem = true;
            }
            //석상의 시험, 동작 테스트 못함(전투 종료 구현하면 테스트)
            else if (PlayManager.instance.isStone)
            {
                if (i == 3)
                {
                    lostItems[i].gameObject.SetActive(true);
                    float xOffset = CalculateXOffset(_n, i);
                    lostItems[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

                    GameObject getLostItemcard = lostItems[i].gameObject;
                    LostItem card;

                    string randomName = DataManager.instance.AllLostItemList[Random.Range(0, DataManager.instance.AllLostItemDatas.Count)];
                    card = DataManager.instance.AllLostItemDatas[randomName];

                    // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
                    var lostcard = getLostItemcard.GetComponent<LostItemCard>();
                    lostcard.SetCard(card);
                }
                else
                {
                    getBattleCards[i].gameObject.SetActive(true);
                    float xOffset = CalculateXOffset(_n, i);
                    getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

                    //배틀 카드 설정
                    GameObject getCard = getBattleCards[i].gameObject;


                    string[] BattleCardNames = new string[] { "갈라치기", "뒤통수치기", "급소 찌르기", "성급한 판단", "속사", "비열한 찌르기" };
                    // 랜덤으로 BattleCardNames에서 카드 이름을 선택
                    string randomName;
                    BattleCardData card;

                    //배틀카드 sprite 완성 시 변경
                    //randomName = DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardDatas.Count)];
                    randomName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];
                    getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = true;
                    card = DataManager.instance.AllBattleCardDatas[randomName];
                    
                    // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
                    var battleCard = getCard.GetComponent<GetBattleCard>();
                    battleCard.SetCard(card);
                }
            }
            //행운, 낭떠러지, 도박장
            else
            {
                getBattleCards[i].gameObject.SetActive(true);
                float xOffset = CalculateXOffset(_n, i);
                getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

                //배틀 카드 설정
                GameObject getCard = getBattleCards[i].gameObject;


                string[] BattleCardNames = new string[] { "갈라치기", "뒤통수치기", "급소 찌르기", "성급한 판단", "속사", "비열한 찌르기" };
                // 랜덤으로 BattleCardNames에서 카드 이름을 선택
                string randomName;
                BattleCardData card;
                if (mapTile.tileData.GetOrDelete == "획득")
                {
                    //배틀카드 sprite 완성 시 변경
                    //randomName = DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardDatas.Count)];
                    randomName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];
                    getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = true;
                    card = DataManager.instance.AllBattleCardDatas[randomName];
                }
                else
                {
                    randomName = PlayerData.playerBattleCardDeck[Random.Range(0, PlayerData.playerBattleCardDeck.Count)].name;
                    getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = false;
                    card = DataManager.instance.AllBattleCardDatas[randomName];
                    getbattleCardDatas.Add(card);
                }

                // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
                var battleCard = getCard.GetComponent<GetBattleCard>();
                battleCard.SetCard(card);

            }
        }
        Option.SetActive(true);
    }

    private float CalculateXOffset(int n, int i)
    {
        if (n == 2)
            return i == 1 ? 200 : -200;
        if (n == 3 && i > 0)
            return i == 2 ? 310 : -310;
        return 0;
    }
    #endregion
    #region TileEvents
    public void TestofStone()
    {
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        BtnText = ClickButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

        if(BtnText == "도전")
        {
            PlayManager.instance.curTile = DataManager.instance.AllTileDatas["무덤"]; //임시로 설정
            SceneManager.LoadScene("PlayScene");
            Options.SetActive(false);
            PlayManager.instance.isStone = true;
        }
        else if(BtnText == "도망")
        {
            EndEvent();
        }
    }
    
    //홀, 짝 게임
    public void OddEvenGames()
    {
        //클릭한 버튼의 오브젝트 가져오기
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        //버튼의 text 가져오기
        BtnText = ClickButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        //버튼 클릭시 버튼의 색을 바꿈(UI바꾸면 삭제 예정)
        Image BtnImg = ClickButton.GetComponent<Image>();
        BtnImg.color = Color.green;

        if (!isSpinning)
        {
            StartCoroutine(SpinResult(BtnText,ClickButton));
        }
    }

    private IEnumerator SpinResult(string expectedResult, GameObject _clickBtn)
    {
        isSpinning = true;
        float startTime = 0f;
        RectTransform MoveBarRect = MoveBar.GetComponent<RectTransform>();
        while(true)

        {
            string rantext = Random.Range(0, 2) == 0 ? "홀" : "짝";
            MoveBarRect.DOAnchorPosY(94f, 0.2f);
            resultText.text = rantext;
            startTime += Time.deltaTime;

            print(startTime);
            if(startTime>=0.45f)
            {
                MoveBarRect.DOAnchorPosY(0f, 0.2f);               
                yield return new WaitForSeconds(1f);
                break;
            }
            yield return new WaitForSeconds(0.3f);

            MoveBar.transform.localPosition = new Vector3(0, -98f, 0);
        }

        // 결과가 결정됨
        isSpinning = false;

        MapSystem.instance.tileEffect_UI.resultText.color = Color.red;
        
        // 결과에 따라 승리 또는 패배 텍스트 표시
        if (MapSystem.instance.tileEffect_UI.resultText.text == expectedResult)
        {
            UIText[1].text = "승리";
            _clickBtn.GetComponent<Image>().color = Color.white;
            OddEvenGame.SetActive(false);
            SetGetCard(mapTile.tileData.cardCount[1]);
        }
        else
        {
            UIText[1].text = "패배";
            _clickBtn.GetComponent<Image>().color = Color.white;
            OddEvenGame.SetActive(false);
            mapTile.tileData.GetOrDelete = "제거";
            SetGetCard(1);
        }
        
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "MoveScene" && PlayManager.instance.isStone)
        {
            MapSystem.instance.tileEffect_UI.gameObject.SetActive(true);
            SetGetCard(mapTile.tileData.cardCount[1]);
            PlayManager.instance.isStone = false;
        }
        else if (scene.name == "MoveScene" && !PlayManager.instance.isStone)
        {
            SetGetCard(mapTile.tileData.cardCount[1]);
            PlayerData.diceCount++;
        }
    }
    #endregion
    #region TileEvent END
    public void EndEvent()
    {

        if (!isLostItem && PlayManager.instance.isStone && getbattleCardDatas.Count <= mapTile.tileData.cardCount[0])
        {
            foreach (var card in getbattleCardDatas)
            {
                if (mapTile.tileData.GetOrDelete == "획득")
                    PlayerData.GainCard(card.name);
                else if (mapTile.tileData.GetOrDelete == "제거")
                    PlayerData.DeleteCard(card.name);
            }
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
        else if (!isLostItem && PlayManager.instance.isStone && getbattleCardDatas.Count + getLostItmeCardDatas.Count <= mapTile.tileData.cardCount[0])
        {
            foreach (var card in getbattleCardDatas)
            {
                PlayerData.GainCard(card.name);         
            }
            foreach (var card in getLostItmeCardDatas)
            {
                PlayerData.GainLostItem(card.name);
            }
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
        else if (isLostItem && getLostItmeCardDatas.Count <= mapTile.tileData.cardCount[0])
        {
            foreach (var card in getLostItmeCardDatas)
            {              
                PlayerData.GainLostItem(card.name);
            }
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
        else if(BtnText == "도망")
        {
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
    }
    public void resetTileEvent()
    {
        getbattleCardDatas.Clear();
        getLostItmeCardDatas.Clear();
        resultText.color = Color.black; // Missing 떠서 비워놓음
        isLostItem = false;
        foreach (var card in getBattleCards)
        {
            card.gameObject.GetComponent<Outline>().enabled = false;
            card.transform.transform.localScale = new Vector3(1, 1, 1);
            card.isSelect = false;
            card.ClickCount = 0;
            card.gameObject.SetActive(false);
        }
        foreach (var card in lostItems)
        {
            card.gameObject.GetComponent<Outline>().enabled = false;
            card.transform.transform.localScale = new Vector3(1, 1, 1);
            card.isSelect = false;
            card.ClickCount = 0;
            card.gameObject.SetActive(false);
        }
        MapSystem.instance.moveCardDraw = true;
    }
    #endregion
}
