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

    [SerializeField] List<TextMeshProUGUI> UIText;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject OddEvenGame;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Option;
    [SerializeField] GameObject MoveBar;

    public List<GetBattleCard> getBattleCards;
    public static List<BattleCardData> getbattleCardDatas = new List<BattleCardData>();

    private bool isSpinning = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

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
                SetGetBattleCard(_mapTile.tileData.cardCount[1]);
                break;
            case "도박장":
                OddEvenGame.SetActive(true);
                break;
            case "행운":
                SetGetBattleCard(_mapTile.tileData.cardCount[1]);
                break;
            case "낭떠러지":
                SetGetBattleCard(_mapTile.tileData.cardCount[1]);
                break;
        }
    }
    public void SetGetBattleCard(int _n)
    {
        // 카드 뿌리기
        for (int i = 0; i < _n; i++)
        {
            getBattleCards[i].gameObject.SetActive(true);
            float xOffset = CalculateXOffset(_n, i);
            getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

            //배틀 카드 설정
            GameObject getCard = getBattleCards[i].gameObject;


            string[] BattleCardNames = new string[] { "갈라치기", "뒤통수치기", "급소 찌르기","성급한 판단", "속사", "비열한 찌르기" };
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

            //획득한 카드 데이터를 플레이어 데이터에 추가. 장태규 추가. 12-07
            PlayerData.GainCard(card.name);
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

    public void TestofStone()
    {
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        string BtnText = ClickButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

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
        string BtnText = ClickButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;

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
            SetGetBattleCard(mapTile.tileData.cardCount[1]);
        }
        else
        {
            UIText[1].text = "패배";
            _clickBtn.GetComponent<Image>().color = Color.white;
            OddEvenGame.SetActive(false);
            mapTile.tileData.GetOrDelete = "제거";
            SetGetBattleCard(1);
        }
        
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "MoveScene" && PlayManager.instance.isStone)
        {
            MapSystem.instance.tileEffect_UI.gameObject.SetActive(true);
            SetGetBattleCard(mapTile.tileData.cardCount[1]);
            PlayManager.instance.isStone = false;
        }
        else if (scene.name == "MoveScene" && !PlayManager.instance.isStone)
        {
            SetGetBattleCard(mapTile.tileData.cardCount[1]);
        }
    }

    public void EndEvent()
    {

        if (getbattleCardDatas.Count > 0 && getbattleCardDatas.Count <= mapTile.tileData.cardCount[0])
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
    }
    public void resetTileEvent()
    {
        getbattleCardDatas.Clear();
        resultText.color = Color.black;
        foreach (var card in getBattleCards)
        {
            card.gameObject.GetComponent<Outline>().enabled = false;
            card.transform.transform.localScale = new Vector3(1, 1, 1);
            card.isSelect = false;
            card.ClickCount = 0;
            card.gameObject.SetActive(false);
        }
        MapSystem.instance.moveCardDraw = true;
    }
}
