using UnityEngine;

/// <summary>
/// 仮想オブジェクトあり
/// インタラクト時、指定したフラグ状況に応じたテキストを表示するタイプのオブジェクト制御クラス
/// InteractiveObjTextBaseと同じようなつくりになっているが、Inspectorで指定したフラグに応じて再生するテキストが切り替わる
/// フラグがオンになった後のテキストは、一度のみ再生されるかどうかを設定可能
/// </summary>
public class InteractiveObjText_WithFlagBase : MonoBehaviour, IInteractiveObj
{
    /// <summary>
    /// テキスト終了後にイベントが発生するか
    /// デフォルトではfalse
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual bool _isAfterEvent{ get; private set; } = false;

    [SerializeField, Header("フラグオン後のテキストは一度のみか")]
    private bool _isOnce = false;

    [SerializeField, Header("取得したいフラグ")]
    private Flags _flagType;

    [SerializeField, Header("表示するイベントテキスト情報")]
    protected EventSentences _sentences = null;

    [SerializeField, Header("特定のフラグがオンの時に表示するテキスト")]
    private EventSentences _onFlagSentence = null;

    // すでに一度フラグオンの状態を見ているか
    private bool _hasOn = false;


    /// <summary>
    /// フラグオフ時、テキスト表示後に呼ばれる処理
    /// デフォルトでは処理がない
    /// 継承して実装する必要あり
    /// </summary>

    protected virtual void TextAfterEvent()
    {
        Debug.Log("処理なし");
        return;
    }

    /// <summary>
    /// フラグオン時、テキスト表示後に呼ばれる処理
    /// デフォルトでは処理がない
    /// 継承して実装する必要あり
    /// </summary>

    protected virtual void OnFlagTextAfterEvent()
    {
        Debug.Log("処理なし");
        return;
    }

    public void OnIntractEvent()
    {
        // フラグに応じて、再生するテキストを変更
        // 再生部分はInteractiveObjTextBaseと同じ
        if (!FlagManager.Instance.Flags.GetFlagValue(_flagType) || (_isOnce && _hasOn))
        {
            if (_isAfterEvent)
                EventManager.Instance.OnStartEventText(_sentences, TextAfterEvent);
            else
                EventManager.Instance.OnStartEventText(_sentences);
        }
        else
        {
            _hasOn = true;
            if (_isAfterEvent)
                EventManager.Instance.OnStartEventText(_onFlagSentence, OnFlagTextAfterEvent);
            else
                EventManager.Instance.OnStartEventText(_onFlagSentence);
        }
    }

    public void UpdateCursor()
    {
        CursorManager.Instance?.OnChangeEventCursor();
    }
}
