using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    [SerializeField] GameObject faderPrefab;
    Fader fader;

    void Awake()
    {
        #region 기타 시스템
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        #endregion

        #region Fade Object 생성
        var faderObj = Instantiate(faderPrefab);
        faderObj.transform.parent = transform;
        fader = faderObj.GetComponent<Fader>();
        #endregion
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fader.FadeOut();
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