using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class ResultFloater : MonoBehaviour
{
    [SerializeField] 
    Image image;
    [SerializeField]
    TextMeshProUGUI text;

    public void SetResult(string _text, string _for)
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        text.color = new Color(1,1,1,0);
        image.color = new Color(0, 0, 0, 0);
        text.text = _text;
        text.DOFade(1, 1).OnComplete(()=> text.DOFade(0, 1));
        image.DOFade(1f, 1.8f).OnComplete(() => SceneManager.LoadScene(_for));
    }
    
}
