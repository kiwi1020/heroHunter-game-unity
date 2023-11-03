using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollsion : MonoBehaviour
{
    private MapTile mapTile;
    // Start is called before the first frame update
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Tile")
        {
            mapTile = collision.gameObject.GetComponent<MapTile>();
            //충돌 시 타일타입 설정 후 타일 이름(Text) 활성화
            if (mapTile != null)
            {
                mapTile.TileEffect();
                //mapTile.SetTileName();
                mapTile.tileName.gameObject.SetActive(true);
            }

        }
    }
}
