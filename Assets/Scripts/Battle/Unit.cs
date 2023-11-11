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
    //public int barrier;

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
}
