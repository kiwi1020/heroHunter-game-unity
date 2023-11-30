using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;

    [SerializeField] TextMeshProUGUI title, des;
    [SerializeField] TextMeshProUGUI[] optionText;

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
