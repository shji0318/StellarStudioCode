using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class PopUI_Achievement : UI_Popup
{    
    public enum Buttons
    {
        Achievement_ExitButton,
    }
    Dictionary<string, Achievement_Slot> _achievementSlotDic = new Dictionary<string, Achievement_Slot>();
    int _achieveCount;
    [SerializeField]
    Achievement_Slot _achievemetSlot;
    [SerializeField]
    Transform _slotRoot;

    [SerializeField]
    Sprite _successSymbol;
    [SerializeField]
    Sprite _failSymbol;
    public void Awake()
    {
        Init();
        MakeSlot();
    }

    public void OnEnable()
    {
        ChangeSuccessAchievement();
    }

    public void Init()
    {
        Bind<Button>(typeof(Buttons));
        _slotRoot = gameObject.Find<Transform>("Achievement_SlotLayout",true);
        _achievemetSlot = Util.LoadAssetAsync<GameObject>("Achievement_Slot").GetComponent<Achievement_Slot>();

        AddUIEvent(Get<Button>((int)Buttons.Achievement_ExitButton).gameObject,
            (PointerEventData data) => UIManager.UI.DestroyPopupUI(),
            UIEvent.Click);
    }

    public void MakeSlot()
    { 
        foreach(string key in DataManager.Data.NowData.Achievements.Keys)
        {
            if (_achievementSlotDic.ContainsKey(key))
                continue;

            Achievement_Slot achv = GameObject.Instantiate<Achievement_Slot>(_achievemetSlot);
            achv.transform.SetParent(_slotRoot);
            _achievementSlotDic.Add(key, achv);
        }
    }

    public void CheckSlotCount()
    {
        if(_achievementSlotDic.Count > DataManager.Data.NowData.Achievements.Count)
        {
            foreach(string key in DataManager.Data.NowData.Achievements.Keys)
            {
                if (_achievementSlotDic.ContainsKey(key))
                    continue;

                _achievementSlotDic.Remove(key);
            }
        }

        if (_achievementSlotDic.Count < DataManager.Data.NowData.Achievements.Count)
        {
            foreach (string key in DataManager.Data.NowData.Achievements.Keys)
            {
                if (_achievementSlotDic.ContainsKey(key))
                    continue;

                Achievement_Slot achv = GameObject.Instantiate<Achievement_Slot>(_achievemetSlot);
                achv.transform.SetParent(_slotRoot);
                _achievementSlotDic.Add(key, achv);
            }
        }
    }   

    public void ChangeSuccessAchievement()
    {
        //추가적으로 달성한 업적이 없다면 return
        if (_achieveCount == DataManager.Data.NowData._achieveCount)
            return;

        foreach(KeyValuePair<string,bool> kv in DataManager.Data.NowData.Achievements)
        {
            //달성하지 못했으면 continue;
            if (kv.Value == false)
                continue;
            //현재 슬롯에 정상적으로 업적들이 적용되어있는지 확인
            //해당 업적이 없다면 체크
            if (!_achievementSlotDic.ContainsKey(kv.Key))
                CheckSlotCount();

            Achievement achv = AchievementManager.Achievements.AchievementDB.Data[kv.Key];
            Achievement_Slot achvSlot =  _achievementSlotDic[kv.Key];
            achvSlot.Title = achv.AchievementTitle;
            achvSlot.Description = achv.AchievementDescription;
            achvSlot.Achieve = true;
            achvSlot.Symbol = _successSymbol;
            achvSlot.ChangeSlotInfo();
            _achieveCount++;
        }
        
    }
}
