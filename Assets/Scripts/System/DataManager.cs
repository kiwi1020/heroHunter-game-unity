using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class SkillData
{
    public string name;
    public List<string> effects = new List<string>(), enforcedEffects = new List<string>();


    public SkillData(string _name)
    {
        name = _name;
    }
    public void AddEffect(string _name)
    {
        if (_name == "없음") return;
        effects.Add(_name);
    }
    public void AddEnforcedEffect(string _name)
    {
        if (_name == "없음") return;
        enforcedEffects.Add(_name);
    }
}
public class TileData
{
    public string name, type, effect;

    public TileData(string _name, string _type, string _effect)
    {
        name = _name;
        type = _type;
        effect = _effect;
    }
}

public class MoveCardData
{
    public string name;
    public List<string> effects = new List<string>();

    public MoveCardData(string _name)
    {
        name = _name;
    }
    public void AddEffect(string _name)
    {
        if (_name == "없음") return;
        effects.Add(_name);
    }
}


public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    //Dic 
    public Dictionary<string, TileData> AllTileDatas = new Dictionary<string, TileData>();
    public Dictionary<string, MoveCardData> AllMoveCardDatas = new Dictionary<string, MoveCardData>();
    public Dictionary<string, SkillData> AllSkillDatas = new Dictionary<string, SkillData>();


    //List
    public List<string> AllTileList = new List<string>();
    public List<string> AllMoveCardList = new List<string>();
    public List<string> AllSkillList = new List<string>();
    
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


        #region Data Enter

        // 1. TileData
        string[] line = TextData[0].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var tileData = new TileData(e[0], e[1], e[2]);

            AllTileDatas.Add(e[0], tileData);
            AllTileList.Add(e[0]);
        }

        // 2. MoveCardData
        line = TextData[1].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            if (!AllMoveCardDatas.ContainsKey(e[0]))
            {
                var moveCardData = new MoveCardData(e[0]);
                AllMoveCardDatas.Add(e[0], moveCardData);
                AllMoveCardList.Add(e[0]);
            }
            
            AllMoveCardDatas[e[0]].AddEffect(e[1]);
        }

        // 3. SkillData
        line = TextData[2].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');


            if (!AllSkillDatas.ContainsKey(e[0]))
            {
                var skillData = new SkillData(e[0]);
                AllSkillDatas.Add(e[0], skillData);
                AllSkillList.Add(e[0]);
            }

            AllSkillDatas[e[0]].AddEffect(e[1]);
            AllSkillDatas[e[0]].AddEnforcedEffect(e[2]);

        }
        #endregion
    }

    public string[] TextData = new string[13];


    #region Data Load

    const string tileURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=845719806";
    const string moveCardURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=1292713227";
    const string skillURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=706366216";

    [ContextMenu("Load Data")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(tileURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 0);

        www = UnityWebRequest.Get(moveCardURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 1);

        www = UnityWebRequest.Get(skillURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 2);

        Debug.Log("Success Load");
    }

    void SetDataList(string tsv, int i)
    {
        TextData[i] = tsv;
    }

    #endregion
}