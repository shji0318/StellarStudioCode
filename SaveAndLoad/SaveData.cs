using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //PlayerData
    public int _money = 0;
    public float _advertise = 1;
    public float _hp = 1;
    public float _attack = 1;
    public float _speed = 1;

    //OptionData
    public float _bgm = 1f;
    public float _sfx = 1f;

    //AchievementData
    public int _achieveCount = 0;    
    [SerializeField]
    SerializableDictionary<string, bool> _achievements = new SerializableDictionary<string, bool>();

    public SerializableDictionary<string, bool> Achievements { get { return _achievements; } set { _achievements = value; } }
}


