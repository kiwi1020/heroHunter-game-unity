using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveCardHideButton : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    Image image;

    bool h = false;
    private void Awake()
    {
        image = GetComponent<Image>();   
    }

    public void ChaneImage()
    {
        if(h)
        {
            image.sprite = sprites[1];
            h = false;
        }
        else
        {
            image.sprite = sprites[0];
            h = true;
        }
    }

}
