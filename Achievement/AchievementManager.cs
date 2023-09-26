using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    static AchievementManager _instance;

    public static AchievementManager Achievements { get {return _instance;}}

    AchievementsDatabase _db = new AchievementsDatabase();

    public AchievementsDatabase AchievementDB { get { return _db; } }

    Queue<Achievement> _successAchievementQueue = new Queue<Achievement>();
    AudioClip _achievementSuccessSound;

    public void Awake()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find($"@Achievement");

            if (go == null)
                go = new GameObject { name = "@Achievement" };

            _instance = go.GetOrAddComponent<AchievementManager>();

            DontDestroyOnLoad(go);
        }
        else
            Destroy(gameObject);

        _achievementSuccessSound = Resources.Load<AudioClip>("Sound/SFX/AchievementSuccessSound");
    }

    public void Update()
    {
        if (DataManager.Data.SceneState == Define.SceneState.BattleScene)
            return;

        if (_successAchievementQueue.Count <= 0)
            return;

        StartCoroutine(PopSuccessAchievements());
    }

    public void Enqueue(string id)
    {
        if (!AchievementDB.Data.ContainsKey(id))
            return;
        
        if (DataManager.Data.NowData.Achievements[id])
            return;

        Achievement achv = AchievementDB.Data[id];
        DataManager.Data.NowData.Achievements[id] = true;
        DataManager.Data.NowData._achieveCount++;
        _successAchievementQueue.Enqueue(achv);
    }

    // SuccessQueueø° µ•¿Ã≈Õ∞° ¡∏¿Á«“ ∞ÊøÏ Ω««‡ 
    public IEnumerator PopSuccessAchievements()
    {
        int length = _successAchievementQueue.Count;
        Achievement[] achvs = new Achievement[length];

        for(int i = 0; i < length; i++)
        {
            achvs[i] = _successAchievementQueue.Dequeue();
        }

        for(int i = 0; i < length; i++)
        {
            PopUI_AchievementEvent achvevent = UIManager.UI.LoadPopupUI("PopUI_AchievementEvent").GetComponent<PopUI_AchievementEvent>();            
            achvevent.TextReceiver(achvs[i]);
            SoundManager.Sound.PlaySFX(_achievementSuccessSound);
            yield return new WaitForSeconds(1.5f);
            achvevent.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    public void CheckSkillAchievement(string name)
    {
        switch (name)
        {
            case "≈∏∫Ò¿« ≥™ƒßπ›":
                AchievementManager.Achievements.Enqueue("VeteranTraveler");
                break;
            case "TNT":
                AchievementManager.Achievements.Enqueue("ArtBoom");
                break;
            case "±´Ωƒ":
                AchievementManager.Achievements.Enqueue("TrashFood");
                break;
            case "∞ÌµÂ∏ß":
                AchievementManager.Achievements.Enqueue("GOATIce");
                break;
            case "¿Á∞£µ’¿Ã":
                AchievementManager.Achievements.Enqueue("DontStopSiro");
                break;
            case "»˜≥™ƒ⁄∆–Ω∫":
                AchievementManager.Achievements.Enqueue("Huhuhu");
                break;
            case "ƒ‹ÆèÆè":
                AchievementManager.Achievements.Enqueue("TomatoJuice");
                break;
        }
    }       

    public void CheckMonsterAchievement(int num)
    {
        switch (num)
        {
            case 0:
                AchievementManager.Achievements.Enqueue("DontHuntArnyang");
                break;
            case 1:
                AchievementManager.Achievements.Enqueue("DontHuntBineul");
                break;
            case 2:
                AchievementManager.Achievements.Enqueue("DontHuntPienna");
                break;
            case 3:
                AchievementManager.Achievements.Enqueue("DontHuntHaedoong");
                break;
            case 4:
                AchievementManager.Achievements.Enqueue("DontHuntMaro");
                break;
            case 5:
                AchievementManager.Achievements.Enqueue("DontHuntBboongdang");
                break;
        }
    }

}
