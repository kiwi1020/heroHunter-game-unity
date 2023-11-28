 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] BattleHUD battleHUD; 

    public string unitName, unitType;
    public int damage;

    public int maxHP;
    public int currentHP;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void SetUnit(MonsterData _monsterData)
    {
        unitName = _monsterData.name;
        unitType = _monsterData.type;

        maxHP = _monsterData.hp[0];
        currentHP = maxHP;
    }

    public void SetUnit()
    {
        maxHP = PlayerData.maxHP;
        currentHP = maxHP;
    }

    public bool Takedamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Effect_PlayerAnimation()
    {
        BattleSystem.instance.EffectBattleCard();  
    }

    public void Finish_PlayerAnimation()
    {
        BattleSystem.instance.EfterPlayerTurn();
    }

    public void Effect_EnemyAnimation()
    {
        BattleSystem.instance.EffectEnemySkill();
    }

    public void Finish_EnemyAnimation()
    {
        BattleSystem.instance.EfterEnemyTurn();
    }
}
