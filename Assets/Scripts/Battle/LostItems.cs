using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LostItems : MonoBehaviour
{
    [SerializeField] Image[] images;

    void OnEnable()
    {
        SetLostItems();
    }
    void SetLostItems()
    {
        foreach(Image i in images)
        {
            i.gameObject.SetActive(false);
        }
        for(int i = 0; i< PlayerData.playerLostItems.Count; i++)
        {
            images[i].gameObject.SetActive(true);
            images[i].sprite = DataManager.instance.AllLostItemIllusts.Find(x => x.name == PlayerData.playerLostItems[i].name).sprite;
        }
    }
}
