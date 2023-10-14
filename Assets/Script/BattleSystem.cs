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
        //플레이어턴 시작
        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        //적에게 데미지를 입히기
        bool isDead = enemyUnit.Takedamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        //dialogueText.text = "공격 성공!"

        yield return new WaitForSeconds(2f);

        //적이 죽었는지를 확인
        if (isDead)
        {
            //상태확인 후 턴의 상태를 변화시킴
            state = BattleState.WON;
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        //dialogueText.text = enemyUnit.unitName + "공격!"

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.Takedamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            //dialogueText.text = "승리!";
        }
        else if (state == BattleState.LOST)
        {
            //dialogueText.text = "패배..."
        }
    }

    void PlayerTurn()
    {
        //dialogueText.text = "카드를 선택하십시오";
    }
    //일단 공격버튼이 있다는 가정하에 만듬
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnFinishButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
}
