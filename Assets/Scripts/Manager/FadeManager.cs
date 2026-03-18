using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// 画面のフェードを行うシングルトンクラス
/// CanvasとImageを用いたシンプルなフェードアウトインを行う
/// フェードの色と時間を引数で指定できる。しない場合、Inspectorで指定したデフォルトのもので行われる
/// シーン移動を待機するために、UniTaskを採用
/// </summary>
[DefaultExecutionOrder(-1000)]
[RequireComponent(typeof(CanvasGroup))]
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    // 非破壊オブジェクトに設定
    protected override bool dontDestroyOnLoad => true;

    [SerializeField, Header("デフォルトのフェード時間")]
    private float _fadeDuration = 2.0f;

    [SerializeField, Header("デフォルトのフェードカラー")]
    private Color _fadeColor = Color.black;

    [SerializeField, Header("Image")]
    private Image _fadePanel;

    // CanvasGroup
    private CanvasGroup _cg = null;

    public bool IsFade{ get; private set; } = false;
    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask CallScene(string sceneName, bool isAutoFadeIn = true)
    {
        try
        {
            await FadeOut(_fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        if (isAutoFadeIn is false) return;
        try
        {
            await FadeIn(_fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask CallScene(float fadeDuration, string sceneName, bool isAutoFadeIn = true)
    {
        try
        {
            await FadeOut(fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }


        if (isAutoFadeIn is false) return;
        try
        {
            await FadeIn(fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask CallScene(Color fadeColor, string sceneName, bool isAutoFadeIn = true)
    {
        try
        {
            await FadeOut(_fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }


        if (isAutoFadeIn is false) return;
        try
        {
            await FadeIn(_fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：フェードアウトインを行い、その合間でシーンのロードを行う
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <returns></returns>
    public async UniTask CallScene(float fadeDuration, Color fadeColor, string sceneName, bool isAutoFadeIn = true)
    {
        try
        {
            await FadeOut(fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
        catch (OperationCanceledException)
        {
            return;
        }


        if (isAutoFadeIn is false) return;
        try
        {
            await FadeIn(fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <returns></returns>
    public async UniTask FadeOut()
    {
        try
        {
            await FadeOut(_fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <returns></returns>
    public async UniTask FadeIn()
    {
        try
        {
            await FadeIn(_fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <returns></returns>
    public async UniTask FadeOut(float fadeDuration)
    {
        try
        {
            await FadeOut(fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }


    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間</param>
    /// <returns></returns>
    public async UniTask FadeIn(float fadeDuration)
    {
        try
        {
            await FadeIn(fadeDuration, _fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeOut(Color fadeColor)
    {
        try
        {
            await FadeOut(_fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeIn(Color fadeColor)
    {
        try
        {
            await FadeIn(_fadeDuration, fadeColor);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードアウトを行う
    /// 全体を覆うImageが透明度0から1に向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeOut(float fadeDuration, Color fadeColor)
    {
        if (_fadePanel == null)
        {
            Debug.LogWarning($"{this.name}：フェード対象のImageがアタッチされていません");
        }
        if (_cg == null)
        {
            Debug.LogWarning($"{this.name}：フェード対象のCanvasGroupがアタッチされていません");
        }
        var token = this.GetCancellationTokenOnDestroy();
        IsFade = true;
        float _timeCount = 0.0f;
        _fadePanel.color = fadeColor;
        _fadePanel.enabled = true;

        // CanvasGroupのAlphaをTweenすることでフェードを行う
        while (_timeCount < fadeDuration)
        {
            _timeCount += Time.deltaTime;
            float t = Mathf.Clamp01(_timeCount / fadeDuration);
            _cg.alpha = Mathf.Lerp(0.0f, 1.0f, t);
            try
            {
                await UniTask.Yield(cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        _cg.alpha = 1.0f;
        IsFade = false;

    }

    /// <summary>
    /// 待機可能：画面全体のシンプルなフェードインを行う
    /// 全体を覆うImageが透明度1fから0fに向かう
    /// </summary>
    /// <param name="fadeDuration">フェードの所要時間：秒</param>
    /// <param name="fadeColor">フェード時のImageの色</param>
    /// <returns></returns>
    public async UniTask FadeIn(float fadeDuration, Color fadeColor)
    {
        if (_fadePanel == null)
        {
            Debug.LogWarning($"{this.name}：フェード対象のImageがアタッチされていません");
        }
        if (_cg == null)
        {
            Debug.LogWarning($"{this.name}：フェード対象のCanvasGroupがアタッチされていません");
        }
        var token = this.GetCancellationTokenOnDestroy();
        IsFade = true;
        float _timeCount = 0.0f;
        _fadePanel.color = fadeColor;
        _fadePanel.enabled = true;

        // CanvasGroupのAlphaをTweenすることでフェードを行う
        while (_timeCount < fadeDuration)
        {
            _timeCount += Time.deltaTime;
            float t = Mathf.Clamp01(_timeCount / fadeDuration);
            _cg.alpha = Mathf.Lerp(1.0f, 0.0f, t);
            try
            {
                await UniTask.Yield(cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        _cg.alpha = 0.0f;
        _fadePanel.enabled = false;
        IsFade = false;
    }

    void Start()
    {
        // RequireComponentで強制的にアタッチしている
        // CanvasGroupを取得
        _cg = GetComponent<CanvasGroup>();
    }

    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}

