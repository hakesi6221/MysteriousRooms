using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;



// 現在操作しているものを表す列挙型
public enum OperateState
{
    None,
    Common,
    Focus,
    Text,
    Inventry,
    Direction,
    Config,
}
public class MaingameManager : SingletonMonoBehaviour<MaingameManager>
{
    protected override bool dontDestroyOnLoad => false;

    [SerializeField, Header("メインシーン始まった時に表示されるテキスト")]
    private EventSentences _startSentence = null;

    // 現在操作しているもの
    private OperateState _operateState = OperateState.None;

    // 直前操作していたもの
    private OperateState _lastOperate = OperateState.None;

    // 履歴に残らない変更を行っているか
    private bool _isChangeStateAbs = false;

    /// <summary>
    /// 現在操作しているもの
    /// </summary>
    public OperateState CurrentOperate { get { return _operateState; } }

    /// <summary>
    /// 操作対象を変更
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeOperate(OperateState newState)
    {
        if (!_isChangeStateAbs)
        {
            _lastOperate = _operateState;
            _operateState = newState;
        }
        else
        {
            _isChangeStateAbs = false;
            _operateState = newState;
        }
    }

    /// <summary>
    /// 変更後の状態を保存せずに操作対象を変更
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeOperateWithoutLog(OperateState newState)
    {
        _isChangeStateAbs = true;
        _lastOperate = _operateState;
        _operateState = newState;
    }

    // 現在の状態を履歴に残さず、操作対象を変更
    public void ChangeOperateAbs(OperateState newState)
    {
        _operateState = newState;
    }

    /// <summary>
    /// 直前の操作対象に戻す
    /// </summary>
    public void ReturnOperate()
    {
        if (_lastOperate == OperateState.None) return;

        _isChangeStateAbs = false;
        _operateState = _lastOperate;
    }

    // Start is called before the first frame update
    async void Start()
    {
        _operateState = OperateState.None;
        try
        {
            await UniTask.WaitUntil(() => !FadeManager.Instance.IsFade, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        catch (OperationCanceledException)
        {
            return;
        }

        SoundManager.Instance.PlayBGMWithFadeIn(1);
        // 開始時に表示するテキストがあれば表示する
        if (_startSentence == null || _startSentence.Sentences.Count != 0)
            EventManager.Instance.OnStartEventText(_startSentence, () => ChangeOperateAbs(OperateState.Common));
        // ないなら普通に開始
        else
            ChangeOperateAbs(OperateState.Common);
    }
}
