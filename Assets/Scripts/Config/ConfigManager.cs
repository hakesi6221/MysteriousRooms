using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [SerializeField, Header("設定UIの親")]
    private GameObject _configUI = null;

    [SerializeField, Header("タイトル画面のシーン名")]
    private string _titleSceneName = "TitleScene";

    // インベントリを開く直線の操作状態
    private OperateState _lastState = OperateState.None;

    /// <summary>
    /// インベントリ画面を開く
    /// </summary>
    public void OnOpenConfig()
    {
        if (MaingameManager.Instance != null)
            if (MaingameManager.Instance.CurrentOperate != OperateState.Common && MaingameManager.Instance.CurrentOperate != OperateState.Focus) return;

        SoundManager.Instance?.PlaySE(0);
        if (MaingameManager.Instance != null)
            _lastState = MaingameManager.Instance.CurrentOperate;
        _configUI.SetActive(true);
        if (MaingameManager.Instance != null)
            MaingameManager.Instance.ChangeOperate(OperateState.Config);
    }

    /// <summary>
    /// インベントリ画面を閉じる
    /// </summary>
    public void OnCloseConfig()
    {
        if (MaingameManager.Instance != null)
            if (MaingameManager.Instance.CurrentOperate != OperateState.Config) return;
        SoundManager.Instance.PlaySE(1);
        _configUI.SetActive(false);
        if (MaingameManager.Instance != null)
            MaingameManager.Instance.ChangeOperate(_lastState);
        _lastState = OperateState.None;
    }

    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    public void ReturnTitle()
    {
        if (MaingameManager.Instance != null)
            if (MaingameManager.Instance.CurrentOperate != OperateState.Config) return;
        SoundManager.Instance.StopBGM(1);
        SoundManager.Instance.PlaySE(1);
        FadeManager.Instance.CallScene(_titleSceneName);
    }
}
