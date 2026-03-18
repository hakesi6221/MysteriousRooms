using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// コンフィグ画面の制御を担当するクラス
/// コンフィグ画面関連のボタンを押されたときの処理を持っている
/// Inspectorにて、各ボタンのイベントに設定している
/// </summary>
public class ConfigManager : MonoBehaviour
{
    [SerializeField, Header("設定UIの親")]
    private GameObject _configUI = null;

    [SerializeField, Header("タイトル画面のシーン名")]
    private string _titleSceneName = "TitleScene";

    // コンフィグ画面を開く直線の操作状態
    private OperateState _lastState = OperateState.None;

    /// <summary>
    /// コンフィグ画面を開く
    /// </summary>
    public void OnOpenConfig()
    {
        // メイン画面だった場合、通常状態とフォーカス状態以外では開けない
        if (MaingameManager.Instance != null)
            if (MaingameManager.Instance.CurrentOperate != OperateState.Common && MaingameManager.Instance.CurrentOperate != OperateState.Focus) return;

        SoundManager.Instance?.PlaySE(0);
        // メイン画面だった場合、直前の操作状態を保存しておく
        if (MaingameManager.Instance != null)
            _lastState = MaingameManager.Instance.CurrentOperate;
        _configUI.SetActive(true);
        // メイン画面だった場合、コンフィグ操作状態に変更
        if (MaingameManager.Instance != null)
            MaingameManager.Instance.ChangeOperate(OperateState.Config);
    }

    /// <summary>
    /// コンフィグ画面を閉じる
    /// </summary>
    public void OnCloseConfig()
    {
        // メイン画面だった場合、コンフィグ画面以外で呼ばれた場合無効
        if (MaingameManager.Instance != null)
            if (MaingameManager.Instance.CurrentOperate != OperateState.Config) return;
        SoundManager.Instance.PlaySE(1);
        _configUI.SetActive(false);
        // メイン画面だった場合、直前の操作状態に戻す
        if (MaingameManager.Instance != null)
            MaingameManager.Instance.ChangeOperate(_lastState);
        _lastState = OperateState.None;
    }

    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    public void ReturnTitle()
    {
        // メイン画面だった場合、コンフィグ操作状態でないなら無効
        if (MaingameManager.Instance != null)
            if (MaingameManager.Instance.CurrentOperate != OperateState.Config) return;
        SoundManager.Instance.StopBGM(1);
        SoundManager.Instance.PlaySE(1);
        FadeManager.Instance.CallScene(_titleSceneName).Forget();
    }
}
