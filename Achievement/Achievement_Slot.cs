using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_Slot : UI_Base
{
    string _title = "???";
    string _description = "???";
    Sprite _symbol;
    bool _achieve = false;

    public string Title { get { return _title; } set { _title = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public Sprite Symbol { get { return _symbol; } set { _symbol = value; } }
    public bool Achieve { get { return _achieve; } set { _achieve = value; } }
    public enum Texts
    {
        AchievementTitleText,
        AchievementDescriptionText,
    }
    public enum Images
    {
        AchievementSymbol,
    }

    public void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void ChangeSlotInfo()
    {
        Get<TextMeshProUGUI>((int)Texts.AchievementTitleText).text = _title;
        Get<TextMeshProUGUI>((int)Texts.AchievementDescriptionText).text = _description;
        Get<Image>((int)Images.AchievementSymbol).sprite = _symbol;

        if (Achieve)
            GetComponent<Image>().color = Color.white;
    }
}
