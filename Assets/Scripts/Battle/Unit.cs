 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public BattleHUD battleHUD; 

    public string unitName, unitType;
    public int damage;

    public int maxHP;
    public int currentHP;

    public int shield;

    public Animator animator;

    #region EnemyInfo

    public MonsterData monsterData;

    public int skillOrder; // 적 스킬 패턴 속, 적이 사용할 스킬 인덱스

    #endregion

    #region SubEffect

    public int[] dotDamage = new int[3] { 0, 0, 0 }; // 3턴으로 시작해서 3개짜리

    public float[] stack = new float[6] { 0,0,0 ,0,0,0 }; // increase / exp / stun / evade / clean /resist

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #region Setting

    public void SetUnit(MonsterData _monsterData)
    {
        monsterData = _monsterData;

        unitName = monsterData.name;
        unitType = monsterData.type;

        maxHP = monsterData.hp[0];
        currentHP = maxHP;

        shield = monsterData.hp[1];

        battleHUD.SetHUD(this);
        /*
        int tmpJob = 0;

        switch (monsterData.type)
        {
            case "기사": tmpJob = 1; break;
            case "용병": tmpJob = 3; break;
            case "마법사": tmpJob = 2; break;
            case "영웅": tmpJob = 4; break;
            case "왕": tmpJob = 5; break;
            case "사냥꾼": tmpJob = 6; break;
        }
        */
        animator.SetInteger("job", Random.Range(1, 5));
        animator.SetInteger("type", 0);
        animator.SetTrigger("change");
    }

    public void SetUnit()
    {
        maxHP = PlayerData.maxHP;
        currentHP = maxHP;
    }

    #endregion

    #region Animation

    //플레이어
    public void Effect_PlayerAnimation()
    {
        BattleSystem.instance.EffectBattleCard();  
    }

    public void Finish_PlayerAnimation()
    {
        BattleSystem.instance.EfterPlayerTurn();
    }

    //적
    public void Effect_EnemyAnimation()
    {
        animator.SetInteger("job", Random.Range(1,5));
        BattleSystem.instance.EffectEnemySkill(skillOrder);
        if(skillOrder >= monsterData.patterns[0].Length - 1)
        {
            skillOrder = 0;
        }
        else
        {
            skillOrder += 1;
        }

        print(skillOrder);
    }

    public void Finish_EnemyAnimation()
    {
        BattleSystem.instance.EfterEnemyTurn();
    }

    void Hit_Animation()
    {
        //이건 적 전용 피격 애니메이션
        if (BattleSystem.instance.units[0] != this)
        {
            animator.SetInteger("type", 2);
            animator.SetInteger("job", Random.Range(1, 5));
            animator.SetTrigger("change");
        }
        else
        {
            animator.SetTrigger("hit");
        }
    }

    #endregion


    #region Acts
    void Hit()
    {
        Hit_Animation();
        //피격 모션
        //피해량
    }

    public void ActSideEffect(bool endOfPlayerTurn)
    {
        ActDotDamage();
    }
    void ActDotDamage()
    {
        foreach(int i in dotDamage)
        {
            Takedamage(i);
        }

        dotDamage[2] = dotDamage[1];
        dotDamage[1] = dotDamage[0];
        dotDamage[0] = 0;

    }

    public bool ActStun()
    {
        if (stack[2] >= 1) //기절 시 넘김
        {
            stack[2]--;
            BattleSystem.instance.FloatText(this.battleHUD.gameObject, "기절");
            battleHUD.SetSideEffect();
            Finish_EnemyAnimation();
            return true;
        }
        else return false;
    }

    public void Takedamage(int _damage, bool _p = false)
    {
        if(_damage <= 0) return;

        if(stack[3] >= 1)
        {
            stack[3]--;
            BattleSystem.instance.FloatText(this.battleHUD.gameObject, "회피!");
            return;
        }

        int remainDamage = 0;

        if (shield > 0 && _p == false)
        {
            remainDamage = shield < _damage ? _damage - shield : 0; // 보호막보다 큰 데미지는 체력을 까도록
            shield = shield < _damage ? 0 : shield - _damage; //보호막 데미지
        }
        else
        {
            remainDamage = _damage;
        }

        if (shield < 0) shield = 0;

        if (BattleSystem.instance.units[0] == this)
        {
            if (!_p && PlayerData.CheckLostItem("두꺼운 가죽"))
                remainDamage -= 5;
        }
        else
        {
            if (PlayerData.CheckLostItem("가시박힌 검"))
                remainDamage = (int)(remainDamage * 1.4f) ;
        }

        currentHP -= remainDamage; // 체력 데미지

        Hit();

        if(BattleSystem.instance.units[0] == this)
        {
            if (_p && PlayerData.CheckLostItem("막지 못하는 방패"))
                TakeShield(10);
            if (!_p && PlayerData.CheckLostItem("부서진 방패"))
                TakeShield(5);
        }


        if(battleHUD != null) BattleSystem.instance.FloatText(battleHUD.gameObject, "-" + _damage);

        battleHUD.SetHP();
        battleHUD.SetSideEffect();
    }

    public void TakeHeal(float _damage)
    {

        currentHP = currentHP + (int)_damage >= maxHP ? maxHP : currentHP + (int)_damage;
        BattleSystem.instance.FloatText(battleHUD.gameObject, "회복 +" + (int)_damage);
        battleHUD.SetHP();
    }

    public void TakeShield(float _damage)
    {
        shield += (int)_damage;

        if (shield > maxHP) battleHUD.SetHUD(this);

        BattleSystem.instance.FloatText(battleHUD.gameObject, "보호막 +" + (int)_damage);
        battleHUD.SetHP();
    }
    #endregion
}
