using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEvent : MonoBehaviour
{
    public MapTile mapTile;

    public void SetSelectEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);

        mapTile = _mapTile;
    }

    public void SelectOption(int _n)
    {
        ActEvent();
    }

    void ActEvent()
    {
        EndEvent();
    }

    void EndEvent()
    {
        mapTile.EndTileEffect();
        gameObject.SetActive(false);
    }
}
