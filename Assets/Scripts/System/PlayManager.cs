using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData // 플레이어 진행 상황 저장
{
    //현재 체력, 보호막, 소지 전투 카드, 소지 이동 카드, 소지 유실물, 주사위

    public int maxHP;
    public int currentHP;
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    public PlayerData playerData;
    public TileData curTile;
    public int curTileNum = 0;

    public List<TileData> tileMapData = new List<TileData>();

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
        playerData = new PlayerData();

        playerData.maxHP = 100;
        playerData.currentHP = playerData.maxHP;
    }

}