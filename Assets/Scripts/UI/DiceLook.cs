using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLook : MonoBehaviour
{
    [SerializeField] GameObject dicePre, diceLayout;

    List<GameObject> dicePool = new List<GameObject>();


    void OnEnable()
    {
        SetDicePool();
    }

    public void SetDicePool()
    {
        if(dicePool.Count < PlayerData.diceCount)
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
