using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageData 
{
    public float _playTime = 0f;
    public int _tookOutCount = 0;
    public int _coinCount = 0;
    public Dictionary<int, int> _tookOutMonsterDic = new Dictionary<int, int>();

    public StageData()
    {
        string[] names = typeof(MonsterType.MonsterTypes).GetEnumNames();

        for(int i = 0; i < names.Length; i++)
        {
            _tookOutMonsterDic.Add(i, 0);
        }
    }

    public string GetPlayTime()
    {
        int m = (int)_playTime;
        int h = m / 60;
        m = m % 60;

        return $"{h:00}:{m:00}";
    }

    public void CheckCount()
    {
        int[] monsterAchievementCount = new int[6] { 50, 50, 50, 20, 50, 50 };
        foreach(int key in _tookOutMonsterDic.Keys)
        {
            if (_tookOutMonsterDic[key] > monsterAchievementCount[key])
                AchievementManager.Achievements.CheckMonsterAchievement(key);
        }
    }
}
