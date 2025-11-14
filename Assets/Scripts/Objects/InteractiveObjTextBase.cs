using UnityEngine;

public class InteractiveObjTextBase : MonoBehaviour, IInteractiveObjBase
{
    // イベントを渡すか
    virtual protected bool _isAfterEvent{ get; private set; } = false;

    [SerializeField, Header("表示するイベントテキスト情報")]
    protected EventSentences _sentences = null;

    virtual protected void TextAfterEvent()
    {
        Debug.Log("処理なし");
        return;
    }

    public void OnIntractEvent()
    {
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
