using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LostItemCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;
    [SerializeField] Image illust;
    [SerializeField] Outline outline;
    RectTransform rect;
    LostItem lostItem;

    public int ClickCount;
    public bool isSelect = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetCard(LostItem _lostItem)
    {
        lostItem = _lostItem;
        cardNameText.text = _lostItem.name;
        cardNameText.color = Color.white;
        cardDesText.text = _lostItem.des;
        illust.sprite = DataManager.instance.AllLostItemIllusts.Find(x => x.name == _lostItem.name).sprite;
    }
    public void Zoom(bool _zoom)
    {
        if (!isSelect)
        {
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
        if (ClickCount % 2 == 0)
        {
            isSelect = false;
            outline.enabled = false;
            TileEvent.getLostItmeCardDatas.Remove(DataManager.instance.AllLostItemDatas[cardNameText.text]);
        }
        else if (ClickCount % 2 == 1)
        {
            rect.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            isSelect = true;
            outline.enabled = true;

            TileEvent.getLostItmeCardDatas.Add(DataManager.instance.AllLostItemDatas[cardNameText.text]);
        }
    }
}
