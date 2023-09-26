using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static readonly string PrivateKey = "01j0i6da2e07su4n6gn1an2sy";
    
    static DataManager _instance;
    public static DataManager Data { get { return _instance; } }

    float _time = 0f;    

    [SerializeField]
    StageData _stageData = null;
    SaveData _data;
    Define.SceneState _sceneState = Define.SceneState.None;    

    public StageData NowStageData { get { return _stageData; } set { _stageData = value; } }
    public SaveData NowData { get { return _data; } set { _data = value; } }

    public Define.SceneState SceneState { 
        get 
        { 
            return _sceneState; 
        } 
        set 
        { 
            _sceneState = value; 

            if(_sceneState == Define.SceneState.MainScene)
            {
                _stageData = null;
            }
            else if(_sceneState == Define.SceneState.BattleScene)
            {
                _stageData = new StageData();
            }
        } 
    }

    public void Awake()
    {
        Init();
    }

    public void Update()
    {
        AutoSave();
        UpdateStageData();
    }

    public void Init()
    {
        if(_instance == null)
        {
            GameObject go = GameObject.Find("@Data");

            if(go == null)
                go = new GameObject() { name = "@Data" };

            _instance = go.GetOrAddComponent<DataManager>();
            DontDestroyOnLoad(go);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Load();

        if(NowData == null)
            NowData = new SaveData();
     
        CheckAchievement();
    }

    public void CheckAchievement()
    {
        foreach(string key in AchievementManager.Achievements.AchievementDB.Data.Keys)
        {
            if (NowData.Achievements.ContainsKey(key))
                continue;

            NowData.Achievements.Add(key, false);
        }

        AddOrRemoveAchievementDic();
    }
   
    public void AddOrRemoveAchievementDic()
    {
        // 만약 현재 저장된 업적의 수가 DB 업적 수보다 적을 경우 추가하는 작업
        if(NowData.Achievements.Count < AchievementManager.Achievements.AchievementDB.Data.Count)
        {
            foreach(string key in AchievementManager.Achievements.AchievementDB.Data.Keys)
            {
                if (NowData.Achievements.ContainsKey(key))
                    continue;

                NowData.Achievements.Add(key, false);
            }
        }
        // 만약 현재 저장된 업적의 수가 DB 업적 수보다 많다면 필요없는 업적을 제거하는 작업
        else
        {
            foreach (string key in NowData.Achievements.Keys)
            {
                if (AchievementManager.Achievements.AchievementDB.Data.ContainsKey(key))
                    continue;

                NowData.Achievements.Remove(key);
            }
        }
    }

    public void Save()
    {
        string jsonString = JsonUtility.ToJson(NowData);
        string encryptString = Encrypt(jsonString);

        using (FileStream fs = new FileStream(GetPath(),FileMode.Create,FileAccess.Write))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(encryptString);

            fs.Write(bytes, 0, bytes.Length);
        }
    }

    public void AutoSave()
    {
        if (_sceneState == Define.SceneState.BattleScene)
            return;

        _time += Time.deltaTime;
        if (_time > 5.0f)
        {
            _time = 0f;
            Save();
        }
    }

    public void Load()
    {
        if(!File.Exists(GetPath()))            
            return;

        using (FileStream fs = new FileStream(GetPath(), FileMode.Open, FileAccess.Read))
        {
            int length = (int)fs.Length;
            byte[] bytes = new byte[length];

            fs.Read(bytes, 0, length);

            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            string decryptString = Decrypt(jsonString);

            NowData = JsonUtility.FromJson<SaveData>(decryptString);
        }
    }

    public void UpdateStageData()
    {
        if (_stageData == null)
            return;

        if (PlayerController.Player.DoSkill == true)
            return;

        _stageData._playTime += Time.deltaTime;
    }

    public void UpTookOutCount(int num)
    {
        if (_stageData == null)
            return;

        _stageData._tookOutCount++;
        _stageData._tookOutMonsterDic[num]++;
    }


    //===================RijndaelManaged를 통한 AES암호화=========================

    private string Encrypt(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(results, 0, results.Length);
    }

    private string Decrypt(string data)
    {
        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] resultArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(resultArray);
    }

    private RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(PrivateKey);
        RijndaelManaged result = new RijndaelManaged();

        byte[] newKeysArray = new byte[16];
        System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;
        return result;
    }
    //=============================================================================
    static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "save.abcd");
    }
}
