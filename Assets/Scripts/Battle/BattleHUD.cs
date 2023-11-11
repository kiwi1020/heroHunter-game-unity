using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

    //public Text nameText;
    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    Unit unit;

    public void SetHUD(Unit _unit)
    {
        unit = _unit;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        SetHP();
    }

    public void SetHP()
    {
        hpSlider.value = unit.currentHP;
        hpText.text = $"{unit.maxHP}/{unit.currentHP}" ;
    }
}
