using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TreeEditor;
using UnityEngine.SceneManagement;

public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;

    public TextMeshProUGUI DesName; // UI Des Text
    public TextMeshProUGUI resultText; // Result Text

    private bool isSpinning = false;

    public void SetEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);

        mapTile = _mapTile;

        SetEventUI(mapTile);
    }

    public void SetEventUI(MapTile _mapTile)
    {
        switch (_mapTile.tileData.name)
        {
            case "신비한 석상":                
                MapSystem.instance.selectEvent.DesName.text = mapTile.tileData.desc;
                GameObject op = GameObject.Find("SelectUI").transform.Find("Options").gameObject;              
                op.SetActive(true);

                break;
            case "재래시장":
                MapSystem.instance.selectEvent.DesName.text = mapTile.tileData.desc;
                GameObject Comp = GameObject.Find("SelectUI").transform.Find("Comp").gameObject;
                Comp.SetActive(true);

                GameObject cardObjects = GameObject.Find("BattleCard3");

                for (int i = 0; i < 3; i++)
                {
                    GameObject getCard = cardObjects.transform.GetChild(i).gameObject;

                    string[] BattleCardNames = new string[] { "갈라치기", "급소 찌르기", "뒤통수치기", "뺨때리기", "성급한판단", "속사", "비열한 찌르기" };

                    // 랜덤으로 BattleCardNames에서 카드 이름을 선택
                    string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

                    // 선택한 카드 이름으로 실제 데이터를 얻어옴
                    BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];

                    // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
                    var battleCard = getCard.GetComponent<GetBattleCard>();
                    battleCard.SetCard(card);

                    PlayerData.playerBattleCardDeck.Add(card);

                }

                break;
            case "도박장":
                MapSystem.instance.selectEvent.DesName.text = mapTile.tileData.desc;               
                break;
            case "행운":
                MapSystem.instance.gainEvent.DesName.text = mapTile.tileData.desc;
                break;
            case "낭떠러지":
                MapSystem.instance.gainEvent.DesName.text = mapTile.tileData.desc;
                break;
        }
    }

    public void SelectEvent()
    {
       switch (mapTile.tileData.name)
        {
            case "신비한 석상":
                
                break;
            case "재래시장":
                
                break;
            case "도박장":
                
                break;
        }
    }
    public void GainEvent()
    {
        switch (mapTile.tileData.name)
        {
            case "행운":
                GetBattleCard();
                break;
            case "낭떠러지":             
                DeleteBattleCard();
                break;
        }
    }

    public void EndEvent()
    {
        gameObject.SetActive(false);
        MapSystem.instance.moveCardDraw = true;
    }

    public void GetBattleCard()
    {
        GameObject cardObjects = GameObject.Find("BattleCard");

        for (int i = 0; i < 2; i++)
        {
            GameObject getCard = cardObjects.transform.GetChild(i).gameObject;

            string[] BattleCardNames = new string[] { "갈라치기", "급소 찌르기", "뒤통수치기", "뺨때리기", "성급한판단", "속사", "비열한 찌르기" };

            // 랜덤으로 BattleCardNames에서 카드 이름을 선택
            string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

            // 선택한 카드 이름으로 실제 데이터를 얻어옴
            BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];           

            // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            PlayerData.playerBattleCardDeck.Add(card);

        }
    }
    public void DeleteBattleCard()
    {      
        if (PlayerData.playerBattleCardDeck.Count != 0)
        {
            GameObject cardObjects = GameObject.Find("BattleCard2");

            GameObject getCard = cardObjects.transform.GetChild(0).gameObject;

            var randInt = Random.Range(0, PlayerData.playerBattleCardDeck.Count);

            BattleCardData card = PlayerData.playerBattleCardDeck[randInt];
            
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            PlayerData.playerBattleCardDeck.RemoveAt(randInt);

        }
    }
    public void TestofStone()
    {
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        string BtnText = ClickButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

        if(BtnText == "도전")
        {
            PlayManager.instance.curTile = DataManager.instance.AllTileDatas["무덤"]; //임시로 설정
            SceneManager.LoadScene("PlayScene");

            SceneManager.sceneLoaded += OnSceneLoaded;

            GameObject options = GameObject.Find("Options");
            options.SetActive(false);

            PlayManager.instance.iscomp = true;
        }
        else if(BtnText == "도망")
        {
            EndEvent();
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name =="MoveScene" && PlayManager.instance.iscomp) 
        {
            MapSystem.instance.selectEvent.gameObject.SetActive(true);


            GameObject Comp = GameObject.Find("SelectUI").transform.Find("Comp").gameObject;
            Comp.SetActive(true);

            GameObject cardObjects = GameObject.Find("BattleCard3");

            for (int i = 0; i < 3; i++)
            {
                GameObject getCard = cardObjects.transform.GetChild(i).gameObject;

                string[] BattleCardNames = new string[] { "갈라치기", "급소 찌르기", "뒤통수치기", "뺨때리기", "성급한판단", "속사", "비열한 찌르기" };

                // 랜덤으로 BattleCardNames에서 카드 이름을 선택
                string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

                // 선택한 카드 이름으로 실제 데이터를 얻어옴
                BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];

                // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
                var battleCard = getCard.GetComponent<GetBattleCard>();
                battleCard.SetCard(card);

                PlayerData.playerBattleCardDeck.Add(card);

            }

            PlayManager.instance.iscomp=false;
        }
    }

    //홀, 짝 게임
    public void OddEvenGame()
    {
        //클릭한 버튼의 오브젝트 가져오기
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;

        //버튼의 text 가져오기
        string BtnText = ClickButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;

        //버튼 클릭시 버튼의 색을 바꿈(UI바꾸면 삭제 예정)
        Image BtnImg = ClickButton.GetComponent<Image>();
        BtnImg.color = Color.red;

        if (!isSpinning)
        {
            StartCoroutine(SpinResult(BtnText));
        }
    }
    private IEnumerator SpinResult(string expectedResult)
    {
        isSpinning = true;

        float spinTime = 1.5f;
        float startTime = 0f;

        while (startTime < spinTime)
        {
            MapSystem.instance.selectEvent.resultText.text = Random.Range(0, 2) == 0 ? "홀" : "짝";

            startTime += Time.deltaTime;
            yield return new WaitForSeconds(0.05f); // 1번 바뀔 때마다 속도 조절(수정 예정)
        }

        // 결과가 결정됨
        isSpinning = false;

        MapSystem.instance.selectEvent.resultText.color = Color.red;
        
        // 결과에 따라 승리 또는 패배 텍스트 표시
        if (MapSystem.instance.selectEvent.resultText.text == expectedResult)
        {
            // 다이스 추가 
        }
        else
        {
            DeleteBattleCard();
        }
        
    }

}
