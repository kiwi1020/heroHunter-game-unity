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
    public Unit playerSkillTarget;

    public playerSkill skill;
    public BattleCard battlecard;

    public List<BattleCardData> usedBattleCardQueue = new List<BattleCardData>(); // 사용 대기중인 배틀 카드

    bool cardActing = false;

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

            if (i == 0) // Player
            {
                units[i].SetUnit();
                unitHUDs[i].SetHUD(units[i]);
            }
            else // Enemy
            {
                SetEnemyUnit(units[i], unitHUDs[i], tileData.enemies[i - 1]);
            }
        }

        curDiceCount = PlayerData.diceCount;

        #endregion

        //start Player Turn
        state = BattleState.PLAYERTURN;

        battleCardDeck.SetBattleCardDeck();
        PlayerTurn();

        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().BgSoundPlay(1);
    }

    void SetEnemyUnit(Unit _unit, BattleHUD _hud, MonsterData _data)
    {
        _unit.SetUnit(_data);
        _unit.GetComponent<EnemyUnitSkin>().ChangeSkin(DataManager.instance.AllEnemySnAs[_unit.unitName].skinNames);
        _hud.SetHUD(_unit);
    }

    #endregion

    #region PlayerTurn

    void PlayerTurn()
    {
        curDiceCount = PlayerData.diceCount;
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(4);

        battleCardDeck.SetPlayerTurn();
    }

    public void UseBattleCard(BattleCardData _battleCardData)
    {
        usedBattleCardQueue.Add(_battleCardData); //사용한 카드 스킬 대기열에 올림

        ActBattleCardSkill();
    }

    void ActBattleCardSkill()
    {
        if (cardActing || usedBattleCardQueue.Count < 1) return;

        cardActing = true;

        Act_PlayerAnimation(0); // 스킬별로 타입이 있도록 하기
    }

    void Act_PlayerAnimation(int _type)
    {
        units[0].animator.SetInteger("type", _type);
        units[0].animator.SetTrigger("attack");
    }

    //Unit -> Effect_PlayerAnimation 및 Finish_PlayerAnimation 작동

    public void EffectBattleCard()
    {
        units[1].animator.SetInteger("type", 2);
        units[1].animator.SetInteger("job", 0);
        units[1].animator.SetTrigger("change");


        foreach(string i in usedBattleCardQueue[0].skillData.effects)
        {
            var eft = i.Split(':');

            if (eft.Length > 2 && Random.Range(0, 1f) > float.Parse(eft[2])) return;

            switch (eft[0])
            {
                case "물리피해":
                    SkillUseSystem.Damage(playerSkillTarget, int.Parse(eft[1]));
                    break;
                case "관통피해":
                    SkillUseSystem.PiercingDamage(playerSkillTarget, int.Parse(eft[1]));
                    break;
                case "지속피해":
                    int Turn = 3;
                    SkillUseSystem.DotDamage(playerSkillTarget, int.Parse(eft[1]), Turn);
                    break;
            }
        }

        unitHUDs[1].SetHP();

        /*
                bool isDead = units[1].Takedamage(units[0].damage);

                if (isDead)
                {
                    state = BattleState.WON;
                    Destroy(units[1]);
                    EndBattle();
                }
        */
    }

    public void EfterPlayerTurn()
    {
        usedBattleCardQueue.RemoveAt(0);

        cardActing = false;

        ActBattleCardSkill();

        //모든 카드를 썻으면 자동으로 적 턴

        if (battleCardDeck.curHandCardCount > 0 || usedBattleCardQueue.Count > 0) return;

        EnemyTurn();
        state = BattleState.ENEMYTURN;
    }

    #endregion
    
    #region EnemyTurn

    void EnemyTurn()
    {
        Act_EnemyAnimation();
    }

    void Act_EnemyAnimation()
    {
        //job은 미리 설정
        units[1].animator.SetInteger("type", 1);
        units[1].animator.SetTrigger("change");
    }

    public void EffectEnemySkill()
    {
        unitHUDs[0].SetHP();

        /*
        bool isDead = units[0].Takedamage(units[1].damage);


        if (isDead)
        {
            state = BattleState.LOST;
            Destroy(units[0]);
            EndBattle();
        }
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(4);
        */
    }

    public void EfterEnemyTurn()
    {
        state = BattleState.PLAYERTURN;
        PlayerTurn();
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

        EffectBattleCard();
    }

    public void OnFinishButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        EnemyTurn();

        //카드들 used false로 다 바구기
    }

    #endregion


}

public class SkillUseSystem
{

    public static void Damage(Unit _target, int _damage)
    {
        int remainDamage = 0;

        if(_target.shield > 0)
        {
            remainDamage = _target.shield < _damage ? _damage - _target.shield : 0; // 보호막보다 큰 데미지는 체력을 까도록
            _target.shield = _target.shield < _damage ? 0 : _target.shield - _damage; //보호막 데미지
        }
        else
        {
            remainDamage = _damage;
        }

        _target.currentHP -= remainDamage; // 체력 데미지
    }

    public static void PiercingDamage(Unit _target, int _damage)
    {
        _target.currentHP -= _damage;
    }

    public static void DotDamage(Unit _target, int _damage, int remainTurn)
    {
        int remainDamage = 0;
        if (remainTurn >= 1) {
            if (_target.shield > 0)
            {
                remainDamage = _target.shield < _damage ? _damage - _target.shield : 0; // 보호막보다 큰 데미지는 체력을 까도록
                _target.shield = _target.shield < _damage ? 0 : _target.shield - _damage; //보호막 데미지
            }
            else
            {
                remainDamage = _damage;
            }
            _target.currentHP -= remainDamage; // 체력 데미지
        }
    }

    public static void IncreaseDamage()
    {

    }

    public static void ExplosiveDamage()
    {

    }

    //

    public static void Stun()
    {

    }

    public static void Heal()
    {

    }

    public static void Shield()
    {

    }

    public static void Evade()
    {

    }

    public static void Cleanse()
    {

    }

    public static void Resist()
    {

    }

    //

    public static void Reroll()
    {

    }

    public static void Copy()
    {

    }

}