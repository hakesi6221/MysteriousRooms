using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// クリアの条件となるゴールのドアを制御するクラス
/// InteractiveObjText_WithFlagBaseを継承
/// フラグがオンの時のみ、イベントが発生する
/// </summary>
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
        // クリアシーンに遷移
        FadeManager.Instance.CallScene(_clearSceneName).Forget();
    }
}
