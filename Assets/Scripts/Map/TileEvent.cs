using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;

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
        mapTile.EndTileEffect();
        gameObject.SetActive(false);
        MapSystem.moveCardDraw = true;
    }
}
