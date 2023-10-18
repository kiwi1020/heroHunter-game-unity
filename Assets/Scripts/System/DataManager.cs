using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;


public class TileData
{
    string name, type;

    public TileData(string _name, string _type)
    {
        name = _name;
        type = _type;
    }
}


public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public Dictionary<string, TileData> AllDatas = new Dictionary<string, TileData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        #region 데이터 입력

        // 1. 조합 데이터 입력
        string[] line = TextData[1].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            AllDatas.Add(e[0], new TileData(e[0], e[1]));
        }

        #endregion
    }

    public string[] TextData = new string[13];
    public Dictionary<string, TileData> AllTileDatas = new Dictionary<string, TileData>();


    #region 데이터 가져오기

    const string matterURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=0";

    [ContextMenu("데이터 가져오기")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(matterURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 0);

        Debug.Log("데이터 가져오기 성공");
    }

    void SetDataList(string tsv, int i)
    {
        TextData[i] = tsv;
    }

    #endregion
}