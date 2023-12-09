using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

    //public Text nameText;
    public Slider hpSlider, shieldSlider;
    public TextMeshProUGUI hpText;
    public GameObject[] sideEffects;// 도트뎀 3 2 1, 기절, 회피, 저항

    Unit unit;

    public void OffGameObj()
    {
        //서서히 사라지기
        gameObject.SetActive(false);
    }
    public void SetHUD(Unit _unit)
    {
        unit = _unit;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        if(unit.shield < unit.maxHP)
        {
            shieldSlider.maxValue = unit.maxHP;
            shieldSlider.value = unit.shield;
        }
        else
        {
            shieldSlider.maxValue = unit.shield;
            shieldSlider.value = unit.shield;
        }

        SetHP();
    }

    public void SetHP()
    {
        if (unit.shield > unit.maxHP) shieldSlider.maxValue = unit.maxHP;

        hpSlider.value = unit.currentHP;

        shieldSlider.value = unit.shield;

        hpText.text = $"{unit.currentHP}/{unit.maxHP} [{unit.shield}]";
    }

    public void SetSideEffect()
    {
        var _unit = unit;
        foreach (GameObject i in sideEffects) i.gameObject.SetActive(false);

        for(int i = 0; i<3; i++)
        {
            if (_unit.dotDamage[i] >= 1)
            {
                sideEffects[i].gameObject.SetActive(true);
                sideEffects[i].GetComponentInChildren<TextMeshProUGUI>().text = _unit.dotDamage[i].ToString();
                sideEffects[i].GetComponent<PopUpable>().des = _unit.dotDamage[i] + $" 지속피해({3-i}턴 남음)";
            }
        }

        if (_unit.stack[2] >= 1) // stun
        {
            sideEffects[3].gameObject.SetActive(true);
            sideEffects[3].GetComponentInChildren<TextMeshProUGUI>().text = ((int)_unit.stack[2]).ToString();
            sideEffects[3].GetComponent<PopUpable>().des = "기절 "+ _unit.stack[2];
        }
        if (_unit.stack[3] >= 1) // evade
        {
            sideEffects[4].gameObject.SetActive(true);
            sideEffects[4].GetComponentInChildren<TextMeshProUGUI>().text = ((int)_unit.stack[3]).ToString();
            sideEffects[4].GetComponent<PopUpable>().des = "회피 " + _unit.stack[3];
        }
        if (_unit.stack[5] >= 1) // resisit
        {
            sideEffects[5].gameObject.SetActive(true);
            sideEffects[5].GetComponentInChildren<TextMeshProUGUI>().text = ((int)_unit.stack[5]).ToString();
            sideEffects[5].GetComponent<PopUpable>().des = "저항 " + _unit.stack[5];
        }
    }
}
