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

    Unit unit;

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

}
