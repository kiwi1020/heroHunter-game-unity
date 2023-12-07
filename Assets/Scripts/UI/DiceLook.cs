using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceLook : MonoBehaviour
{
    [SerializeField] GameObject dicePre, diceLayout;
    [SerializeField] TextMeshProUGUI countText;

    List<GameObject> dicePool = new List<GameObject>();

    public bool isBattle = false;

    void OnEnable()
    {
        SetDicePool();
    }

    public void SetDicePool()
    {
        if(isBattle)
        {
            countText.text = PlayerData.diceCount.ToString();
        }
        else
        {
            if (dicePool.Count < PlayerData.diceCount)
            {
                var tmpCount = PlayerData.diceCount - dicePool.Count;
                for (int i = 0; i < tmpCount; i++)
                {
                    var tmpDice = Instantiate(dicePre, diceLayout.transform);
                    dicePool.Add(tmpDice);
                }
            }

            if (dicePool.Count > 0) foreach (GameObject i in dicePool) i.SetActive(false);

            for (int i = 0; i < dicePool.Count; i++) dicePool[i].SetActive(true);
        }
    }
}
