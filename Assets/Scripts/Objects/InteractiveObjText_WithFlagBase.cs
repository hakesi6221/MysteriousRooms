using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObjText_WithFlagBase : MonoBehaviour, IInteractiveObjBase
{
    // イベントを渡すか
    virtual protected bool _isAfterEvent{ get; private set; } = false;

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

    virtual protected void TextAfterEvent()
    {
        Debug.Log("処理なし");
        return;
    }

    protected virtual void OnFlagTextAfterEvent()
    {
        Debug.Log("処理なし");
        return;
    }

    public void OnIntractEvent()
    {
        // フラグがオンじゃない場合、元のテキストを再生
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
