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
    public int curDiceCount; // 해당턴의 다이스 개수, 사용하면 줄어드는 거

    public BattleState state;
    public Targeter targeter;

    public playerSkill skill;
    public BattleCard battlecard;

    public List<BattleCardData> curUsedBattleCardDatas = new List<BattleCardData>();


    //public Animator 

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

    #region BattleSetting

    public void StartBattle()
    {
    }

    void SetupBattle()
    {
        foreach (Unit i in units) i.gameObject.SetActive(false);
        foreach (BattleHUD i in unitHUDs) i.gameObject.SetActive(false);

        #region Unit Setting

        var tileData = (BattleTile)PlayManager.instance.curTile;

        for (int i = 0; i < tileData.enemies.Count + 1; i++)
        {
            units[i].gameObject.SetActive(true);
            unitHUDs[i].gameObject.SetActive(true);
            //enemyUnit.SetUnit(tileData.enemies[0]);
            //enemyUnit.SetUnit();

            if (i == 0)
            {
                units[i].SetUnit();
                unitHUDs[i].SetHUD(units[i]);
            }
            else
            {
                units[i].SetUnit(tileData.enemies[i - 1]);
                unitHUDs[i].SetHUD(units[i]);
            }
        }

        curDiceCount = PlayerData.diceCount;

        #endregion

        //start Player Turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();

        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().BgSoundPlay(1);
    }

    #endregion

    #region PlayerTurn

    void PlayerTurn()
    {
        curDiceCount = PlayerData.diceCount;
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(4);
    }

    public void UseBattleCard(BattleCardData _battleCardData)
    {
        //StartCoroutine(PlayerAttack());

        Act_PlayerAnimation(0); // 스킬별로 타입이 있도록 하기
    }

    public void Act_PlayerAnimation(int _type)
    {
        units[0].animator.SetInteger("type", _type);
        units[0].animator.SetTrigger("attack");
    }

    //Unit -> Effect_PlayerAnimation 및 Finish_PlayerAnimation 작동

    IEnumerator PlayerAttack()
    {
        bool isDead = units[1].Takedamage(units[0].damage);

        unitHUDs[1].SetHP();

        yield return new WaitForSeconds(2f);
        EfterPlayerTurn();
        if (isDead)
        {
            state = BattleState.WON;
            Destroy(units[1]);
            EndBattle();
        }
    }

    public void EfterPlayerTurn()
    {
        if (battleCardDeck.curHandCardCount > 0) return;

        StartCoroutine(EnemyTurn());
        state = BattleState.ENEMYTURN;
    }

    #endregion

    #region EnemyTurn

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
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(4);
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            SceneManager.LoadScene("MoveScene");
            GameObject AudioManager = GameObject.Find("AudioManager");
            AudioManager.GetComponent<SoundManager>().BgSoundPlay(0);
        }
        else if (state == BattleState.LOST)
        {
        }
    }

    #endregion

    #region TEST

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

        //카드들 used false로 다 바구기
    }

    #endregion
}
