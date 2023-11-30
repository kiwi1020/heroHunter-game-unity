using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public TextMeshPro tileName; //타일이름 Text
    public Button startButton; //배틀시작 버튼

    private bool isSpinning = false;
    //타일이름 설정, 이름은 정한 후 수정
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;       
        //시작타일은 타일 이름이 없도록 수정함
        if(tileData.name == "시작")
        {
            tileName.text = "";
        }
        else
        {
            tileName.text = tileData.name;
        }
    }
    public void TileEffect()
    {     
        PlayManager.instance.curTile = tileData;

        switch (tileData.type)
        {
            case "전투":
                OnBattleStartButton();
                break;

            case "선택":
                MapSystem.instance.selectEvent.SetEvent(this);
                SelectEvent();
                break;

            case "함정":
                MapSystem.instance.gainEvent.SetEvent(this);
                DeleteBattleCard();
                break;

            case "보상":              
                MapSystem.instance.gainEvent.SetEvent(this);
                GetBattleCard();

                break;

            default:               
                break;

        }
    }

    //배틀시작 버튼
    public void OnBattleStartButton()
    {
        //battleSystem.StartBattle();
        //tileName.gameObject.SetActive(false);
        //startButton.gameObject.SetActive(false);

        SceneManager.LoadScene("PlayScene"); // 임시로 바로 시작
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
            print(card.name);

            // GetBattleCard 컴포넌트를 얻어와서 카드를 설정
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            //PlayerData.playerBattleCardDeck.Add(card);
        }
    }
    public void DeleteBattleCard()
    {   
        if (PlayerData.playerBattleCardDeck.Count != 0)
        {
            PlayerData.playerBattleCardDeck.RemoveAt(
                Random.Range(0, PlayerData.playerBattleCardDeck.Count));
        }
    }
    public void SelectEvent()
    {
        switch (tileData.name)
        {
            case "신비한 석상":
                MapSystem.instance.selectEvent.DesName.text = tileData.desc;
                break;
            case "재래시장":
                MapSystem.instance.selectEvent.DesName.text = tileData.desc;
                break;
            case "도박장":
                MapSystem.instance.selectEvent.DesName.text = tileData.desc;
                break;
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
        /*
        // 결과에 따라 승리 또는 패배 텍스트 표시
        if (MapSystem.instance.selectEvent.resultText.text == expectedResult)
        {
            MapSystem.instance.selectEvent.resultText.text = "승리!";
        }
        else
        {
            MapSystem.instance.selectEvent.resultText.text = "패배!";
        }
        */
    }


}
