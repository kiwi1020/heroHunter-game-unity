using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//전투 상태 종류들
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    //플레이어와 적의 프리펩
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    //플레이어와 적이 나타날때 바닥에 있는 발판. 불필요시 삭제가능
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;
    //텍스트 필요할시 주석해제
    //public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {  
        //전투 시작
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        //전투 시작시 플레이어와 적을 화면에 나타냄.
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        //적 조우시 텍스트 출력
        //dialogueText.text = "...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
    }
}
