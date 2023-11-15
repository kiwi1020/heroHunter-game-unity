using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;

    public BattleManager battleManager;

    public Unit[] units; 
    public BattleHUD[] unitHUDs;

    public BattleCardDeck battleCardDeck;
    public int curDiceCount;

    public BattleState state;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
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
        foreach (Unit i in units) i.gameObject.SetActive(false);
        foreach (BattleHUD i in unitHUDs) i.gameObject.SetActive(false);

        #region Unit Setting

        var tileData = (BattleTile)PlayManager.instance.curTile;

        for(int i = 0; i< tileData.enemies.Count+1; i++)
        {
            units[i].gameObject.SetActive(true);
            unitHUDs[i].gameObject.SetActive(true);
            //enemyUnit.SetUnit(tileData.enemies[0]);
            //enemyUnit.SetUnit();

            if(i == 0)
            {
                units[i].SetUnit();
                unitHUDs[i].SetHUD(units[i]);
            }
            else
            {
                units[i].SetUnit(tileData.enemies[i-1]);
                unitHUDs[i].SetHUD(units[i]);
            }
        }

        curDiceCount = PlayerData.diceCount;

        #endregion

        //start Player Turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = units[1].Takedamage(units[0].damage);

        unitHUDs[1].SetHP();

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            Destroy(units[1]);
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {

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
            SceneManager.LoadScene("MoveScene");
        }
        else if (state == BattleState.LOST)
        {
        }
    }

    void PlayerTurn()
    {
        curDiceCount = PlayerData.diceCount;
    }
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
