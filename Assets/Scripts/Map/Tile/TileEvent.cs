using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;

public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;

    public TextMeshProUGUI DesName; // UI Des Text
    public TextMeshProUGUI resultText; // Result Text

    public void SetEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);

        mapTile = _mapTile;

    }


    public void SelectOption(int _n)
    {
        ActEvent();
    }

    public void ActEvent()
    {
        EndEvent();
    }

    void EndEvent()
    {
        gameObject.SetActive(false);
        MapSystem.instance.moveCardDraw = true;
    }
}
