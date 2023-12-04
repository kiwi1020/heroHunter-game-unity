using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class GetBattleCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;
    [SerializeField] Image illust;
    [SerializeField] Outline outline;
    RectTransform rect;
    BattleCardData battleCardData;

    public int ClickCount;
    public  bool isSelect=false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetCard(BattleCardData _battleCardData)
    {
        battleCardData = _battleCardData;
        cardNameText.text = _battleCardData.name;
        cardNameText.color = Color.white;
        cardDesText.text = _battleCardData.skillData.effects[0];

        illust.sprite = DataManager.instance.AlllBattleCardIllusts.Find(x => x.name == battleCardData.name).sprite;
    }

    public void Zoom(bool _zoom)
    {
        if (!isSelect) {
            if (_zoom)
            {
                rect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            }
            else
            {
                rect.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            }
        }
    }
    public void SelectCard()
    {
        ClickCount++;
        if(ClickCount % 2 == 0)
        {
            isSelect = false;
            TileEvent.SelectCardCount--;
            outline.enabled=false;
            TileEvent.getbattleCardDatas.Remove(DataManager.instance.AllBattleCardDatas[cardNameText.text]);
        }
        else if(ClickCount % 2 == 1)
        {
            rect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            isSelect = true;
            TileEvent.SelectCardCount++;
            outline.enabled = true;

            TileEvent.getbattleCardDatas.Add(DataManager.instance.AllBattleCardDatas[cardNameText.text]);
        }
    }

}
