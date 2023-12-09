using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public TextMeshPro tileName; //타일이름 Text
    public Button startButton; //배틀시작 버튼
    public bool isStepOn=false; //밟았던 타일인지 검사;
    public bool isTileDataUpdate=false;
    //타일이름 설정, 이름은 정한 후 수정
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;       
        //시작타일은 타일 이름이 없도록 수정함
        if(tileData.name == "시작")
        {
            tileName.text = "";
        }
        else
        {
            tileName.text = tileData.name;
        }
    }
    public void TileEffect()
    {     
        PlayManager.instance.curTile = tileData;
        switch (tileData.type)
        {

            case "전투":
                OnBattleStartButton();
                break;
            case "선택":
                MapSystem.instance.tileEffect_UI.SetEvent(this);
                break;
            case "함정":
                MapSystem.instance.tileEffect_UI.SetEvent(this);               
                break;

            case "보상":              
                MapSystem.instance.tileEffect_UI.SetEvent(this);                
                break;

            default:               
                break;

        }
        //GameObject AudioManager = GameObject.Find("AudioManager");
        //AudioManager.GetComponent<SoundManager>().UISfxPlay(18);
    }

    //배틀시작 버튼
    public void OnBattleStartButton()
    {
        //battleSystem.StartBattle();
        //tileName.gameObject.SetActive(false);
        //startButton.gameObject.SetActive(false);

        SceneManager.LoadScene("PlayScene"); // 임시로 바로 시작
    }


}
