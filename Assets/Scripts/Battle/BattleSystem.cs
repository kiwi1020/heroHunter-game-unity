using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//전투 상태 종류들
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleManager battleManager;

    public Unit[] units; // 0 플레이어 // 1 이상 적
    public BattleHUD[] unitHUDs;

    public BattleState state;

    void Start()
    {  
        state = BattleState.START;
        SetupBattle();
    }

    public void StartBattle()
    {
    }

    void SetupBattle()
    {
        //Unit, HUD 배열 타입으로 통합
        foreach (Unit i in units) i.gameObject.SetActive(false);
        foreach (BattleHUD i in unitHUDs) i.gameObject.SetActive(false);

        #region Unit Setting

        var tileData = (BattleTile)PlayManager.instance.curTile;

        for(int i = 0; i< tileData.enemies.Count+1; i++)
        {
            units[i].gameObject.SetActive(true);
            unitHUDs[i].gameObject.SetActive(true);

            if(i == 0)
            {
                units[i].SetUnit(PlayManager.instance.playerData);
                unitHUDs[i].SetHUD(units[i]);
            }
            else
            {
                units[i].SetUnit(tileData.enemies[i-1]);
                unitHUDs[i].SetHUD(units[i]);
            }
        }

        #endregion

        //start Player Turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        print("플레이어 공격");
        //적에게 데미지를 입히기
        bool isDead = units[1].Takedamage(units[0].damage);

        unitHUDs[1].SetHP();
        //dialogueText.text = "공격 성공!"

        yield return new WaitForSeconds(2f);

        //적이 죽었는지를 확인
        if (isDead)
        {
            //상태확인 후 턴의 상태를 변화시킴
            state = BattleState.WON;
            Destroy(units[1]);
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        print("적 공격");
        //dialogueText.text = enemyUnit.unitName + "공격!"

        yield return new WaitForSeconds(1f);

        bool isDead = units[0].Takedamage(units[1].damage);

        unitHUDs[0].SetHP();

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            Destroy(units[0]);
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
            SceneManager.LoadScene("MoveScene");
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
