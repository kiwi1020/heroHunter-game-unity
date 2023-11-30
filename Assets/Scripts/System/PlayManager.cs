using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public static int maxHP;
    public static int currentHP;

    public static List<BattleCardData> playerBattleCardDeck = new List<BattleCardData>();

    public static int diceCount = 2;
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    public PlayerData playerData;

    public TileData curTile;
    public int curTileNum = 0;

    public List<TileData> tileMapData = new List<TileData>();

    public bool iscomp=false;

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
    }

    void SetStart()
    {
        PlayerData.maxHP = 100;
        PlayerData.currentHP = PlayerData.maxHP;

        for(int i = 0; i<10; i++) // 임시
        {
            PlayerData.playerBattleCardDeck.Add(DataManager.instance.AllBattleCardDatas["갈라치기"]);
            PlayerData.playerBattleCardDeck.Add(DataManager.instance.AllBattleCardDatas["속사"]);
        }
    }

}