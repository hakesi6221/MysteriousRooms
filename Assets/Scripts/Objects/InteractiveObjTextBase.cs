using UnityEngine;

/// <summary>
/// 仮想オブジェクトあり
/// インタラクト時テキストを表示するタイプのオブジェクト制御クラス
/// テキスト発生後にイベントが発生するかを継承可能のプロパティで設定できる
/// 発生させる際には、TextAfterEventを継承して実装する必要あり
/// </summary>
public class InteractiveObjTextBase : MonoBehaviour, IInteractiveObj
{
    /// <summary>
    /// テキスト終了後にイベントが発生するか
    /// デフォルトではfalse
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual bool _isAfterEvent{ get; private set; } = false;

    [SerializeField, Header("表示するイベントテキスト情報")]
    protected EventSentences _sentences = null;

    /// <summary>
    /// テキスト表示後に呼ばれる処理
    /// デフォルトでは処理がない
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual void TextAfterEvent()
    {
        Debug.Log("処理なし");
        return;
    }

    public void OnIntractEvent()
    {
        // テキストを再生
        // テキスト後イベントを発生させるなら渡し、ないなら渡さない
        if (_isAfterEvent)
            EventManager.Instance.OnStartEventText(_sentences, TextAfterEvent);
        else
            EventManager.Instance.OnStartEventText(_sentences);
    }

    public void UpdateCursor()
    {
        CursorManager.Instance?.OnChangeEventCursor();
    }
}
