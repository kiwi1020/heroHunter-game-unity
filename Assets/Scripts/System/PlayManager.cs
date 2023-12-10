using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public static int maxHP;
    public static int currentHP;

    public static List<BattleCardData> playerBattleCardDeck = new List<BattleCardData>();
    public static List<MoveCardData> playerMoveCardDeck = new List<MoveCardData>();
    public static List<LostItem> playerLostItems = new List<LostItem>();

    public static int diceCount = 2;
    public static int handCount = 3;

    public static bool CheckLostItem(string _name)
    {
        return playerLostItems.Contains(DataManager.instance.AllLostItemDatas[_name]);
    }

    public static void GainMoveCard(string _name)
    {
        playerMoveCardDeck.Add(DataManager.instance.AllMoveCardDatas[_name]);
    }

    public static void GainCard(string _name)
    {
        playerBattleCardDeck.Add(DataManager.instance.AllBattleCardDatas[_name]);
    }

    public static void DeleteCard(string _name)
    {
        PlayerData.playerBattleCardDeck.Remove(DataManager.instance.AllBattleCardDatas[_name]);
    }

    public static void GainLostItem(string _lostItem)
    {
        playerLostItems.Add(DataManager.instance.AllLostItemDatas[ _lostItem]);
        if (MapSystem.instance != null) MapSystem.instance.lostItems.SetLostItems();

        if(_lostItem == "고기")
        {
            maxHP += 50;
        }
        if(_lostItem == "극한에 몰린 자")
        {
            for(int i = 0; i<10; i++) GainCard("목숨 구걸");
            GainCard("최후의 일격");
        }
        if (_lostItem == "음침한 마도서")
        {
            handCount -= 1;
        }
        if (_lostItem == "듀얼 디스크")
        {
            handCount += 1;
        }
        if (_lostItem == "도박왕의 주사위")
        {
            diceCount += 2;
        }
        if (_lostItem == "가시박힌 검")
        {
            maxHP -= 50;
        }
    }

    public static void GainDice(int _count)
    {
        diceCount += _count;
        if (BattleSystem.instance != null) BattleSystem.instance.diceLook.SetDicePool();
        if (MapSystem.instance != null) MapSystem.instance.diceLook.SetDicePool();
    }

    public static void ClearCard()
    {
        playerBattleCardDeck = new List<BattleCardData>();
        playerMoveCardDeck = new List<MoveCardData>();
        playerLostItems = new List<LostItem>();
        diceCount = 2;
    }
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    public PlayerData playerData;

    public TileData curTile;
    public int curTileNum = 0;
    public bool IsFirst = true;

    public List<TileData> tileMapData = new List<TileData>();
    public WeightRandomPick<string> wrPicker = new WeightRandomPick<string>();

    public bool isStone=false;
    public bool startWeigtTile = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    void Start()
    {
        SetStart();
        if (MapSystem.instance != null) MapSystem.instance.PlayerDataSetting();
    }

    public void SetStart()
    {
        PlayerData.maxHP = 100;
        PlayerData.currentHP = PlayerData.maxHP;

        for(int i = 0; i<10; i++) // 임시
        {
            //PlayerData.GainCard(DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardList.Count)]); // <==   PlayerData.playerBattleCardDeck.Add(DataManager.instance.AllBattleCardDatas["갈라치기"]);
            
            PlayerData.GainCard("최후의 일격");
            PlayerData.GainCard("후려치기");
        }

        for (int i = 0; i < DataManager.instance.AllMonsterList.Count; i++)
            PlayerData.GainMoveCard(DataManager.instance.AllMoveCardList[i]);

        PlayerData.GainLostItem("독성 발톱");

        PlayerData.diceCount = 3;
        PlayerData.handCount = 3;
    }

}