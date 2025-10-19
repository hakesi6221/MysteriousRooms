using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjText_WithFlag_Door : InteractiveObjText_WithFlagBase
{
    [SerializeField, Header("クリアシーンの名前")]
    private string _clearSceneName = "ClearScene";

    protected override bool _isAfterEvent => true;

    protected override void OnFlagTextAfterEvent()
    {
        SoundManager.Instance.StopBGMWithFadeOut(1);
        SoundManager.Instance.PlaySE(2);
        MaingameManager.Instance.ChangeOperate(OperateState.None);
        FadeManager.Instance.CallScene(_clearSceneName);
    }
}
