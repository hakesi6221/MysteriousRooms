using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// タイトル画面を管理するクラス
/// 画面にあるボタンに設定する関数を持っていたり、画面遷移時の初期化や操作可否の切り替えを行う
/// </summary>
public class TitleSceneManager : MonoBehaviour
{

    [SerializeField, Header("始めるボタン")]
    private Button _startButton = null;

    [SerializeField, Header("ライセンスボタン")]
    private Button _licenseButton = null;

    [SerializeField, Header("設定ボタン")]
    private Button _settingButton = null;

    [SerializeField, Header("終了ボタン")]
    private Button _exitButton = null;

    [SerializeField, Header("ライセンスから戻るボタン")]
    private Button _returnButton = null;

    [SerializeField, Header("ライセンスUI")]
    private CanvasGroup _licenseUI = null;

    [SerializeField, Header("移動するシーン")]
    private string _moevScene = "MainScene";

    /// <summary>
    /// タイトルの状態を初期化
    /// </summary>
    public void InitializeTitleScene()
    {
        _returnButton.interactable = false;
        _licenseUI.gameObject.SetActive(false);

        _startButton.interactable = true;
        _licenseButton.interactable = true;
        _settingButton.interactable = true;
        _exitButton.interactable = true;
        SoundManager.Instance.PlayBGMWithFadeIn(0);
    }

    /// <summary>
    /// シーン移動処理
    /// </summary>
    public void OnMoveScene()
    {
        SoundManager.Instance.StopBGM(0);
        _startButton.interactable = false;
        _licenseButton.interactable = false;
        _settingButton.interactable = false;
        _exitButton.interactable = false;
        FadeManager.Instance.CallScene(_moevScene).Forget();
    }

    /// <summary>
    /// ライセンス画面を開く
    /// </summary>
    public void OpenLicense()
    {
        _startButton.interactable = false;
        _licenseButton.interactable = false;
        _settingButton.interactable = false;
        _exitButton.interactable = false;

        _licenseUI.gameObject.SetActive(true);
        _returnButton.interactable = true;
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void OnFinishGame()
    {
        _startButton.interactable = false;
        _licenseButton.interactable = false;
        _settingButton.interactable = false;
        _exitButton.interactable = false;

        // エディター上での操作であればプレイモード終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // そうでないならアプリを落とす
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 決定音を鳴らす
    /// </summary>
    public void OnDecisionSE()
    {
        SoundManager.Instance.PlaySE(0);
    }

    /// <summary>
    /// キャンセル音を鳴らす
    /// </summary>
    public void OnCancelSE()
    {
        SoundManager.Instance.PlaySE(1);
    }

    async void Start()
    {
        var token = this.GetCancellationTokenOnDestroy();

        // フェード中はタイトル画面での操作ができないようにする
        _startButton.interactable = false;
        _licenseButton.interactable = false;
        _settingButton.interactable = false;
        _exitButton.interactable = false;
        _returnButton.interactable = false;
        _licenseUI.gameObject.SetActive(false);
        FadeManager.Instance.FadeIn().Forget();

        try
        {
            await UniTask.WaitUntil(() => FadeManager.Instance.IsFade is false, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("画面フェード待機中に終了しました");
            return;
        }

        // フェードが終了した後にタイトル画面の操作開始時の初期化を行う
        InitializeTitleScene();
    }
}
