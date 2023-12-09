using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TextSizer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    RectTransform rect;

    private void Awake()
    {

        rect = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        TextChange();
    }

    public void TextChange()
    {
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(text.preferredWidth , text.preferredHeight);
    }


}
