using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUI_AchievementEvent : UI_Popup
{
    public enum Texts
    {
        AchievementEvent_TitleText,
    }

    public void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void TextReceiver(Achievement achv)
    {
        Get<TextMeshProUGUI>((int)Texts.AchievementEvent_TitleText).text = achv.AchievementTitle;
    }
}
