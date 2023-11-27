using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetBattleCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;

    BattleCardData battleCardData;
    // Start is called before the first frame update
    public void SetCard(BattleCardData _battleCardData)
    {
        battleCardData = _battleCardData;
        cardNameText.text = _battleCardData.name;
        cardNameText.color = Color.white;
        cardDesText.text = _battleCardData.skillData.effects[0];
    }
}
