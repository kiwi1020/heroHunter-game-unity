using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActive : MonoBehaviour
{
    public Button button;
    public void ButtonFalse()
    {
        button.gameObject.SetActive(false);
    }
    public void ButtonTrue()
    {
        button.gameObject.SetActive(true);
    }
}
