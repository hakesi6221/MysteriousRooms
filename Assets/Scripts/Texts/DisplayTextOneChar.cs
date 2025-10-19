using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using System;
using System.Threading;

public class DisplayTextOneChar
{
    // キャンセル受付開始文字数
    private const int CancelBeginChar = 2;

    // 一文字ずつ表示中か
    private bool _isAppearning = false;

    // キャンセルが呼ばれたか
    private bool _isCallCancel = false;

    // キャンセル可能か
    private bool _isAbleCancel = false;

    /// <summary>
    /// 一文字ずつ表示中か
    /// </summary>
    public bool IsAppearning { get { return _isAppearning; } }

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public void OnCancel()
    {
        if (!_isAppearning) return;
        if (!_isAbleCancel) return;
        _cts.Cancel();
        _isAbleCancel = false;
        _isCallCancel = true;
    }

    /// <summary>
    /// 一文字ずつテキストを表示する
    /// </summary>
    /// <param name="centence">表示したい文</param>
    /// <param name="tmp">表示する対象のTextMeshPro</param>
    /// <param name="waitTime">文字を表示する待機時間：秒</param>
    /// <returns></returns>
    public async void DisplayTextOneCharacter(string centence, TextMeshProUGUI tmp, float waitTime, CancellationToken token)
    {
        CancellationToken newToken = _cts.Token;

        CancellationTokenSource linkedSoures = CancellationTokenSource.CreateLinkedTokenSource(token, newToken);
        var linkedToken = linkedSoures.Token;
        _isCallCancel = false;

        // テキストの表示文字数を0に
        tmp.maxVisibleCharacters = 0;
        // テキストの内容を与えられた文章に決定
        tmp.text = centence;
        tmp.gameObject.SetActive(true);

        _isAppearning = true;
        while (centence.Length > tmp.maxVisibleCharacters)
        {
            if (_isCallCancel)
            {
                tmp.maxVisibleCharacters = centence.Length;
                break;
            }

            // 表示テキストを増やし、指定文字数出ていればキャンセルを可能にする
            if (++tmp.maxVisibleCharacters >= CancelBeginChar)
            {
                _isAbleCancel = true;
            }

            // 指定秒待って
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: linkedToken);
            }
            catch (OperationCanceledException)
            {
                _cts = new CancellationTokenSource();
                continue;
            }
        }
        _isAppearning = false;
    }

    /// <summary>
    /// 一文字ずつテキストを表示する
    /// 待機可能
    /// </summary>
    /// <param name="centence">表示したい文</param>
    /// <param name="tmp">表示する対象のTextMeshPro</param>
    /// <param name="waitTime">文字を表示する待機時間：秒</param>
    /// <returns></returns>
    public async UniTask IDisplayTextOneCharacter(string centence, TextMeshProUGUI tmp, float waitTime, CancellationToken token)
    {
        CancellationToken newToken = _cts.Token;

        CancellationTokenSource linkedSoures = CancellationTokenSource.CreateLinkedTokenSource(token, newToken);
        var linkedToken = linkedSoures.Token;
        _isCallCancel = false;

        // テキストの表示文字数を0に
        tmp.maxVisibleCharacters = 0;
        // テキストの内容を与えられた文章に決定
        tmp.text = centence;
        tmp.gameObject.SetActive(true);

        _isAppearning = true;
        while (centence.Length > tmp.maxVisibleCharacters)
        {
            if (_isCallCancel)
            {
                tmp.maxVisibleCharacters = centence.Length;
                break;
            }

            // 表示テキストを増やし、指定文字数出ていればキャンセルを可能にする
            if (++tmp.maxVisibleCharacters >= CancelBeginChar)
            {
                _isAbleCancel = true;
            }

            // 指定秒待って
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: linkedToken);
            }
            catch (OperationCanceledException)
            {
                _cts = new CancellationTokenSource();
                continue;
            }
        }
        _isAppearning = false;
    }
}
