using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

#region 데이터 정의
[System.Serializable]
public class SpriteAndName
{
    public string name;
    public Sprite sprite;
}

public class SkillData
{
    public string name;

    public List<string> effects = new List<string>();
    public List<string> enforcedEffects = new List<string>();

    public SkillData(string _name, string[] _effects, string[] _enforcedEffects)
    {
        name = _name;
        effects = _effects.ToList();
        enforcedEffects = _enforcedEffects.ToList();
    }
}
public class BattleCardData
{
    public string name, diceCondition, targetingMode;
    public SkillData skillData;

    public BattleCardData(string _name, SkillData _skillData, string _diceCondition, string _targetingMode)
    {
        name = _name;
        skillData = _skillData;
        diceCondition = _diceCondition;
        targetingMode = _targetingMode;
    }
}

public class TileData
{
    public string name, type, desc, weight, title, GetOrDelete,unitCount,rate;

    public int[] cardCount; //0은 획득 또는 제거 가능한 카드 수, 1은 UI에 나타날 카드 수
    public TileData(string _name, string _type, string _title, string _desc,string _unitCount, string _GetOrDelete,int[] _cardCount,string _weight,string _rate)
    {
        name = _name;
        type = _type;
        weight = _weight; 
        cardCount = _cardCount;
        unitCount = _unitCount;
        GetOrDelete = _GetOrDelete; 
        desc = _desc;
        title = _title;
        rate = _rate;
    }  
}
public class BattleTile : TileData
{
    public List<MonsterData> enemies = new List<MonsterData>();

    public BattleTile(string _name, string _type, string _effect, string _title, string _desc, string _unitCount,string _GetOrDelete, int[] _cardCount, string _weight, string _rate) : base(_name,  _type, _title, _desc, _unitCount,_GetOrDelete, _cardCount, _weight,_rate)
    {
        name = _name;
        type = _type;
        weight = _weight;
        enemies = _effect.Split(',').Select(x=> DataManager.instance.AllMonsterDatas[x]).ToList();
        desc  = _desc;
        title= _title;
        unitCount = _unitCount;
        cardCount= _cardCount;
        GetOrDelete = _GetOrDelete; 
        rate = _rate;
    }

    //적에 대한 정보
}

public class MoveCardData
{
    public string name;
    public string weight;
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
    //가중치(확률) 
    public void SetRandomWeight(string _weight)
    {
        if (_weight == "같음") return;
        weight = _weight;
    }
   
}
public class MonsterData 
{
    public string name, type;
    public int[] hp; // 0은 체력 1은 보호막

    public List<string[]> patterns = new List<string[]>(); //패턴 = 스킬 이름 배열, 순서대로 사용

    public MonsterData(string _name, string _type, int[] _hp)
    {
        name = _name;
        type = _type;
        hp = _hp;
    }

    public void AddPattern(string[] _pattern)
    {
        patterns.Add(_pattern);
    }
}
public class EnemySnA
{
    public string name, job, type;
    public List<string> skinNames; // 0 head 1 body 2 gloves 3 pants 4 shoes 5 weapon
    public EnemySnA(string _name, string _job, string _type, List<string> _skinNames)
    {
        name = _name;
        job = _job;
        type = _type;
        skinNames = _skinNames;
    }
}

public class LostItem
{
    public string name, condition, des;

    public LostItem(string _name, string _condition, string _des)
    {
        name = _name;
        condition = _condition;
        des = _des;
    }
}

#endregion
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    //Dic : 이름을 통해 찾을 때 사용
    public Dictionary<string, TileData> AllTileDatas = new Dictionary<string, TileData>();
    public Dictionary<string, MoveCardData> AllMoveCardDatas = new Dictionary<string, MoveCardData>();
    public Dictionary<string, SkillData> AllSkillDatas = new Dictionary<string, SkillData>();
    public Dictionary<string, MonsterData> AllMonsterDatas = new Dictionary<string, MonsterData>();
    public Dictionary<string, EnemySnA> AllEnemySnAs = new Dictionary<string, EnemySnA>();
    public Dictionary<string, BattleCardData> AllBattleCardDatas = new Dictionary<string, BattleCardData>();
    public Dictionary<string, LostItem> AllLostItemDatas = new Dictionary<string, LostItem>();

    //List : 랜덤으로 추출 시 사용
    public List<string> AllTileList = new List<string>();
    public List<string> AllMoveCardList = new List<string>();
    public List<string> AllSkillList = new List<string>();
    public List<string> AllMonsterList = new List<string>();
    public List<string> AllBattleCardList = new List<string>();
    public List<string> AllLostItemList = new List<string>();
    //
    public List<SpriteAndName> AllustList = new List<SpriteAndName>();
    public List<SpriteAndName> AlllMoveCardIllusts = new List<SpriteAndName>();
    public List<SpriteAndName> AlllBattleCardIllusts = new List<SpriteAndName>();
    public List<SpriteAndName> AllLostItemIllusts = new List<SpriteAndName>();


    public string[] TextData = new string[13];
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        #region 데이터 입력

        // 0. MoveCardData
        string[]  line = TextData[1].Split('\n');
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
            AllMoveCardDatas[e[0]].SetRandomWeight(e[2]);
        }
        // 1. SkillData
        line = TextData[2].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            var skillData = new SkillData(e[0],e[1].Split(','), e[2].Split(','));
            AllSkillDatas.Add(e[0], skillData);
            AllSkillList.Add(e[0]);

        }

        // 2. MonsterData
        line = TextData[3].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            if (!AllMonsterDatas.ContainsKey(e[0]))
            {
                var monsterData = new MonsterData(e[0], e[1], e[2].Split('/').Select(i => int.Parse(i)).ToArray());
                AllMonsterDatas.Add(e[0], monsterData);
                AllMonsterList.Add(e[0]);
            }

            AllMonsterDatas[e[0]].AddPattern(e[3].Split(','));
        }

        // 3. TileData
        line = TextData[0].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            TileData tileData = null;

            switch (e[1])
            {
                case "함정":
                    tileData = new TileData(e[0], e[1], e[2], e[3], e[5],e[6], e[7].Split('/').Select(i => int.Parse(i)).ToArray(), e[8], e[9]);
                    break;
                case "전투":
                    tileData = new BattleTile(e[0], e[1], e[4], e[2], e[3], e[5], e[6], e[7].Split('/').Select(i => int.Parse(i)).ToArray(), e[8], e[9]);
                    break;
                case "선택":
                    tileData = new TileData(e[0], e[1], e[2], e[3], e[5], e[6], e[7].Split('/').Select(i => int.Parse(i)).ToArray(), e[8], e[9]);
                    break;
                default:
                    tileData = new TileData(e[0], e[1], e[2], e[3], e[5], e[6], e[7].Split('/').Select(i => int.Parse(i)).ToArray(), e[8], e[9]);
                    break;
            }
            AllTileDatas.Add(e[0], tileData);           
            AllTileList.Add(e[0]);
        }

        // 4. EnemySnA
        line = TextData[4].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            AllEnemySnAs.Add(e[0], new EnemySnA(e[0], e[1], e[2], new List<string>() { e[3], e[4], e[5], e[6], e[7], e[8] }));
        }

        // 5. BattleCard
        line = TextData[5].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            AllBattleCardDatas.Add(e[0], new BattleCardData(e[0], AllSkillDatas[e[1]], e[2], e[3]));
            AllBattleCardList.Add(e[0]);
        }

        //6. LostItem
        line = TextData[6].Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            AllLostItemDatas.Add(e[0], new LostItem(e[0], e[1], e[2]));
            AllLostItemList.Add(e[0]);
        }


        #endregion
    }
    #region 데이터 불러오기

    const string tileURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=845719806";
    const string moveCardURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=1292713227";
    const string skillURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=706366216";
    const string monsterURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=1086732162";
    const string enemySnAURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=980303362";
    const string battleCardURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=0";
    const string lostItemURL = "https://docs.google.com/spreadsheets/d/1V-RFPD30T6GFYOq0CRrGiLMbPt8uypmf1JnjxoRg2go/export?format=tsv&gid=418439008";

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

        www = UnityWebRequest.Get(monsterURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 3);

        www = UnityWebRequest.Get(enemySnAURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 4);

        www = UnityWebRequest.Get(battleCardURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 5);

        www = UnityWebRequest.Get(lostItemURL);
        yield return www.SendWebRequest();
        SetDataList(www.downloadHandler.text, 6);

        Debug.Log("Success Load");
    }

    void SetDataList(string tsv, int i)
    {
        TextData[i] = tsv;
    }

    #endregion
}