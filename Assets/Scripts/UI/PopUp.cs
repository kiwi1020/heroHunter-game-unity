using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PopUp : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    RectTransform rect;

    void OnEnable()
    {
        rect = GetComponent<RectTransform>();
    }

    public void TextChange(string _text)
    {
        text.text = _text;
        rect.sizeDelta = new Vector2(text.preferredWidth + 100, text.preferredHeight + 100);
    }


}
