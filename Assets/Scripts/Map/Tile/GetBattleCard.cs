using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetBattleCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;
    [SerializeField] Image illust;
    BattleCardData battleCardData;
    // Start is called before the first frame update
    public void SetCard(BattleCardData _battleCardData)
    {
        battleCardData = _battleCardData;
        cardNameText.text = _battleCardData.name;
        cardNameText.color = Color.white;
        cardDesText.text = _battleCardData.skillData.effects[0];

        illust.sprite = DataManager.instance.AlllBattleCardIllusts.Find(x => x.name == battleCardData.name).sprite;
    }
}
