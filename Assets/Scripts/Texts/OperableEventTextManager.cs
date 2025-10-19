using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class OperableEventTextManager : MonoBehaviour
{
    [SerializeField, Header("テキストウィンドウ管理スクリプト")]
    private EventTextWindow _textWindow = null;

    // 現在のテキスト表示管理スクリプト
    private DisplayTextOneChar _currentOperateTexts = null;

    // 現在の表示予定文
    private EventSentences _currentCentences = null;

    // 現在の表示予定文の中の、現在表示している文の番号
    private int _currentCentenceIndex = 0;

    // 現在の出力先のTMP
    private TextMeshProUGUI _currentTMP = null;

    // 現在のテキスト表示が終わった後に起こるイベント
    private Action _currentAfterEvent = null;

    /// <summary>
    /// テキストを生成
    /// </summary>
    /// <returns></returns>
    private async void GenerateTexts()
    {
        _textWindow.SetActiveTextFinishIcon(false);
        var token = this.GetCancellationTokenOnDestroy();
        try
        {
            // テキストを表示開始
            await _currentOperateTexts.IDisplayTextOneCharacter(
                        _currentCentences.Sentences[_currentCentenceIndex].Sentence
                        , _currentTMP
                        , _currentCentences.Sentences[_currentCentenceIndex].TextDuration
                        , token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        _textWindow.SetActiveTextFinishIcon(true);
    }

    /// <summary>
    /// 操作可能なイベントテキストの表示を開始し、操作を切り替える
    /// テキスト終了後に発生するイベントを設定
    /// </summary>
    /// <param name="centences"></param>
    /// <param name="tmp"></param>
    /// <param name="speed"></param>
    /// <param name="afterEvent">テキスト表示終了後のイベント</param>
    public async void OnStartOperateTextWithEvent(EventSentences centences, TextMeshProUGUI tmp, Action afterEvent)
    {
        // 稼働条件の制限
        if (MaingameManager.Instance.CurrentOperate == OperateState.Text) return;
        if (_currentOperateTexts != null) return;
        if (centences == null)
        {
            Debug.LogWarning("テキストが指定されていません");
            return;
        }

        // テキスト表示管理クラスを作成
        DisplayTextOneChar displayTxt = new DisplayTextOneChar();

        // 現在の各情報を格納
        _currentCentences = centences;
        _currentCentenceIndex = 0;
        _currentTMP = tmp;
        _currentTMP.fontSize = _currentCentences.Sentences[_currentCentenceIndex].TextFontSize;
        _currentOperateTexts = displayTxt;
        _currentAfterEvent = afterEvent;

        // 操作モードをテキストに変更
        MaingameManager.Instance.ChangeOperate(OperateState.Text);

        // テキスト表示開始
        GenerateTexts();
    }

    /// <summary>
    /// 操作可能なイベントテキストの表示を開始し、操作を切り替える
    /// </summary>
    /// <param name="centences"></param>
    /// <param name="tmp"></param>
    /// <param name="speed"></param>
    public async void OnStartOperateText(EventSentences centences, TextMeshProUGUI tmp)
    {
        // 稼働条件の制限
        if (MaingameManager.Instance.CurrentOperate == OperateState.Text) return;
        if (_currentOperateTexts != null) return;
        if (centences == null)
        {
            Debug.LogWarning("テキストが指定されていません");
            return;
        }

        // テキスト表示管理クラスを作成
        DisplayTextOneChar displayTxt = new DisplayTextOneChar();

        // 現在の各情報を格納
        _currentCentences = centences;
        _currentCentenceIndex = 0;
        _currentTMP = tmp;
        _currentTMP.fontSize = _currentCentences.Sentences[_currentCentenceIndex].TextFontSize;
        _currentOperateTexts = displayTxt;
        _currentAfterEvent = null;

        // 操作モードをテキストに変更
        MaingameManager.Instance.ChangeOperate(OperateState.Text);

        // テキスト表示開始
        GenerateTexts();
    }

    /// <summary>
    /// テキストを表示中で、現在操作中のイベントテキストがあるなら、次のテキストに移動
    /// もし最後のテキストだったら、テキストの表示を終了して操作も終了
    /// </summary>
    /// <returns>テキスト終了後に発火する処理</returns>
    public Action OnNextCentence()
    {
        // 稼働条件の制限
        if (MaingameManager.Instance.CurrentOperate != OperateState.Text) return null;
        if (_currentOperateTexts == null) return null;

        // まだ1文字ずつ表示中だった場合、キャンセル処理
        if (_currentOperateTexts.IsAppearning)
        {
            _currentOperateTexts.OnCancel();
            return null;
        }
        // 全文字の表示が終わっていた場合、文章が残っているかで処理が変わる
        else
        {
            // 最後の文章だった場合、テキスト表示を終了して操作を終了
            if (_currentCentenceIndex >= _currentCentences.Sentences.Count - 1)
            {
                Action afterEvent = _currentAfterEvent;
                if (afterEvent == null)
                {
                    return OnFinishEventCentence;
                }
                else
                {
                    return OnFinishEventCentence + afterEvent;
                }
            }
            // まだ文章が残っていた場合、次の文章へ移行
            else
            {
                _currentCentenceIndex++;
                // テキスト表示開始
                GenerateTexts();
                return null;
            }
        }
    }

    /// <summary>
    /// テキスト表示の終了
    /// 終了後のイベントがある場合、それを再生
    /// </summary>
    public void OnFinishEventCentence()
    {
        // 稼働条件の制限
        if (MaingameManager.Instance.CurrentOperate != OperateState.Text) return;
        if (_currentOperateTexts == null) return;

        Debug.Log("テキスト表示終了");
        _textWindow.SetActiveTextFinishIcon(false);
        _currentTMP.gameObject.SetActive(false);
        _currentCentences.Sentences.Clear();
        _currentCentenceIndex = 0;
        _currentTMP.text = string.Empty;
        _currentTMP = null;
        _currentOperateTexts = null;
        _currentAfterEvent = null;

        MaingameManager.Instance.ReturnOperate();
    }
}
