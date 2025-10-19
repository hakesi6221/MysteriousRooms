using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;
using TMPro;

public class EventTextWindow : MonoBehaviour
{
    [SerializeField, Header("テキストウインドウの親")]
    private Image _windowParent = null;

    [SerializeField, Header("テキスト本体")]
    private TextMeshProUGUI _text = null;

    [SerializeField, Header("テキスト表示終了時のアイコン")]
    private PointerMoveWithConstAccel _textFinishIcon = null;

    // マテリアルインスタンス
    private Material _material = null;

    // マテリアルのパラメーター名
    private const string MATERIAL_PARAMETER = "_DissolveAmount";

    /// <summary>
    /// マテリアルのパラメーターを更新
    /// </summary>
    /// <param name="parameter">更新する値</param>
    private void UpdateMaterial(float parameter)
    {
        _material?.SetFloat(MATERIAL_PARAMETER, parameter);
        if (_windowParent != null) _windowParent.material = _material;
    }

    /// <summary>
    /// テキストウィンドウを出す(フェードインあり)
    /// </summary>
    /// <param name="fadeTime">フェード時間：秒</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask OnFadeInTextWindow(float fadeTime, CancellationToken token)
    {
        _windowParent.gameObject.SetActive(true);

        float timeFramePerSec = 0f;

        UpdateMaterial(1f);
        while (timeFramePerSec < fadeTime)
        {
            UpdateMaterial(1f - (timeFramePerSec / fadeTime));
            timeFramePerSec += Time.deltaTime;
            try
            {
                await UniTask.Yield(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        UpdateMaterial(0f);
        _text.gameObject.SetActive(true);
    }

    /// <summary>
    /// テキストウィンドウを消す(フェードインアウト)
    /// </summary>
    /// <param name="fadeTime">フェード時間：秒</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask OnFadeOutTextWindow(float fadeTime, CancellationToken token)
    {
        SetActiveTextFinishIcon(false);
        _text.gameObject.SetActive(false);
        float timeFramePerSec = 0f;

        UpdateMaterial(0f);
        while (timeFramePerSec < fadeTime)
        {
            UpdateMaterial(timeFramePerSec / fadeTime);
            timeFramePerSec += Time.deltaTime;
            try
            {
                await UniTask.Yield(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        UpdateMaterial(1f);
        _windowParent.gameObject.SetActive(false);
    }

    /// <summary>
    /// テキスト表示終了時に出すアイコンの表示非表示を切り替える
    /// </summary>
    /// <param name="active">アクティブしたいかどうか</param>
    public void SetActiveTextFinishIcon(bool active)
    {
        if (_textFinishIcon.gameObject.activeSelf == active) return;
        _textFinishIcon.gameObject.SetActive(active);
        if (!active)
            _textFinishIcon.Initialize();
    }

    void Start()
    {
        if (_windowParent != null)
            _material = Instantiate(_windowParent.material);
    }

    void OnDestroy()
    {
        Destroy(_material);
    }
}
