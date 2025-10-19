using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using System;
using System.Threading;

public class DisplayTextOneChar
{
    // �L�����Z����t�J�n������
    private const int CancelBeginChar = 2;

    // �ꕶ�����\������
    private bool _isAppearning = false;

    // �L�����Z�����Ă΂ꂽ��
    private bool _isCallCancel = false;

    // �L�����Z���\��
    private bool _isAbleCancel = false;

    /// <summary>
    /// �ꕶ�����\������
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
    /// �ꕶ�����e�L�X�g��\������
    /// </summary>
    /// <param name="centence">�\����������</param>
    /// <param name="tmp">�\������Ώۂ�TextMeshPro</param>
    /// <param name="waitTime">������\������ҋ@���ԁF�b</param>
    /// <returns></returns>
    public async void DisplayTextOneCharacter(string centence, TextMeshProUGUI tmp, float waitTime, CancellationToken token)
    {
        CancellationToken newToken = _cts.Token;

        CancellationTokenSource linkedSoures = CancellationTokenSource.CreateLinkedTokenSource(token, newToken);
        var linkedToken = linkedSoures.Token;
        _isCallCancel = false;

        // �e�L�X�g�̕\����������0��
        tmp.maxVisibleCharacters = 0;
        // �e�L�X�g�̓��e��^����ꂽ���͂Ɍ���
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

            // �\���e�L�X�g�𑝂₵�A�w�蕶�����o�Ă���΃L�����Z�����\�ɂ���
            if (++tmp.maxVisibleCharacters >= CancelBeginChar)
            {
                _isAbleCancel = true;
            }

            // �w��b�҂���
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
    /// �ꕶ�����e�L�X�g��\������
    /// �ҋ@�\
    /// </summary>
    /// <param name="centence">�\����������</param>
    /// <param name="tmp">�\������Ώۂ�TextMeshPro</param>
    /// <param name="waitTime">������\������ҋ@���ԁF�b</param>
    /// <returns></returns>
    public async UniTask IDisplayTextOneCharacter(string centence, TextMeshProUGUI tmp, float waitTime, CancellationToken token)
    {
        CancellationToken newToken = _cts.Token;

        CancellationTokenSource linkedSoures = CancellationTokenSource.CreateLinkedTokenSource(token, newToken);
        var linkedToken = linkedSoures.Token;
        _isCallCancel = false;

        // �e�L�X�g�̕\����������0��
        tmp.maxVisibleCharacters = 0;
        // �e�L�X�g�̓��e��^����ꂽ���͂Ɍ���
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

            // �\���e�L�X�g�𑝂₵�A�w�蕶�����o�Ă���΃L�����Z�����\�ɂ���
            if (++tmp.maxVisibleCharacters >= CancelBeginChar)
            {
                _isAbleCancel = true;
            }

            // �w��b�҂���
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
