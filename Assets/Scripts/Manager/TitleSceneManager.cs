using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

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
        FadeManager.Instance.CallScene(_moevScene);
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                    Application.Quit();//ゲームプレイ終了
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
        _startButton.interactable = false;
        _licenseButton.interactable = false;
        _settingButton.interactable = false;
        _exitButton.interactable = false;
        _returnButton.interactable = false;
        _licenseUI.gameObject.SetActive(false);
        FadeManager.Instance.FadeInDisplay();

        try
        {
            await UniTask.WaitUntil(() => FadeManager.Instance.IsFade is false, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        InitializeTitleScene();
    }
}
