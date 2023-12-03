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

    #region SubEffect

    public int[] dotDamage = new int[3] { 0, 0, 0 }; // 3턴으로 시작해서 3개짜리

    public float[] stack = new float[6] { 0,0,0 ,0,0,0 }; // increase / exp / stun / evade / clean /resist
    public int[] sideEffect = new int[6] { 0,0,0 ,0,0,0 }; // increase / exp / stun / evade / clean /resist

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #region Setting

    public void SetUnit(MonsterData _monsterData)
    {
        unitName = _monsterData.name;
        unitType = _monsterData.type;

        maxHP = _monsterData.hp[0];
        currentHP = maxHP;

        shield = _monsterData.hp[1];
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
        BattleSystem.instance.EffectEnemySkill();
    }

    public void Finish_EnemyAnimation()
    {
        BattleSystem.instance.EfterEnemyTurn();
    }

    #endregion

    public void Hit()
    {
        //피격 모션
        //피해량
    }

    public void ActSideEffect()
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
    public void Takedamage(int _damage, bool _p = false)
    {
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

        currentHP -= remainDamage; // 체력 데미지

        BattleSystem.instance.FloatText(battleHUD.gameObject, "-" + _damage);

        battleHUD.SetHP();

    }


}
