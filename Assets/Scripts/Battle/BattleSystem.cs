using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;

    public BattleManager battleManager;

    public Unit[] units; // 플레이어, 적, 적, 적 
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
        for(int i = 0; i < units.Length; i++) units[i].battleHUD = unitHUDs[i];
        //
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

        SkillUseSystem.Divide_Target(playerSkillTarget, units[0], usedBattleCardQueue[0].skillData);

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
        print("1");
        ActEnemySideEffect();
        EnemyTurn();
        state = BattleState.ENEMYTURN;
    }

    public void ActEnemySideEffect()
    {
        print("2");
        for (int i = 1; i<units.Length; i++)
        {
            if(units[i].gameObject.activeSelf) units[i].ActSideEffect();
        }
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

    public static void Divide_Target(Unit _target, Unit _caster, SkillData _skillData)
    {


        foreach (string i in _skillData.effects)
        {
            var eft = i.Split(':');

            if (eft.Length > 2 && Random.Range(0, 1f) > float.Parse(eft[2])) return;


            var damage = float.Parse(eft[1]) + float.Parse(eft[1]) * _caster.stack[0];

            switch (eft[0])
            {
                case "물리피해":
                    Damage(_target, (int)damage);
                    _caster.stack[0] = 0;
                    break;
                case "관통피해":
                    PiercingDamage(_target, (int)damage);
                    _caster.stack[0] = 0;
                    break;
                case "지속피해":
                    DotDamage(_target, (int)damage);
                    _caster.stack[0] = 0;
                    break;
                case "추가피해":
                    IncreaseDamage(_caster, float.Parse(eft[1]));
                    break;
            }
        }
    }

    static void Damage(Unit _target, int _damage)
    {
        _target.Takedamage(_damage);
    }

    static void PiercingDamage(Unit _target, int _damage)
    {
        _target.Takedamage(_damage, true);
    }


    //유닛한테 쌓아야할듯? 스택을

    static void DotDamage(Unit _target, int _damage)
    {
        _target.dotDamage[0] += _damage;
    }
    //=> battleSystem의 efter turn에서 여기서 쌓인 도트데미지를 가하고 한 턴씩 미루는 메서드를 구현

    static void IncreaseDamage(Unit _target, float _damage)
    {
        _target.stack[0] += _damage;
    }

    static void ExplosiveDamage()
    {

    }

    //

    static void Stun()
    {

    }

    static void Heal()
    {

    }

    static void Shield()
    {

    }

    static void Evade()
    {

    }

    static void Cleanse()
    {

    }

    static void Resist()
    {

    }

    //

    static void Reroll()
    {

    }

    static void Copy()
    {

    }

}