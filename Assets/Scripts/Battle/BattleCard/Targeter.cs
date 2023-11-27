using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    BattleCardData curBattleCardData;
    public bool useMode = false, isTargeting = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("EnemyMan")) isTargeting = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("EnemyMan")) isTargeting = false;
    }

    public void SetPosition(Vector3 _pos, BattleCardData _battleCardData)
    {
        curBattleCardData = _battleCardData;
        transform.position = _pos;
    }

    public void SetUseMode(bool _t)
    {
        useMode = _t;
    }
}
