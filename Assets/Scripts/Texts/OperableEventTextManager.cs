using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class OperableEventTextManager : MonoBehaviour
{
    [SerializeField, Header("�e�L�X�g�E�B���h�E�Ǘ��X�N���v�g")]
    private EventTextWindow _textWindow = null;

    // ���݂̃e�L�X�g�\���Ǘ��X�N���v�g
    private DisplayTextOneChar _currentOperateTexts = null;

    // ���݂̕\���\�蕶
    private EventSentences _currentCentences = null;

    // ���݂̕\���\�蕶�̒��́A���ݕ\�����Ă��镶�̔ԍ�
    private int _currentCentenceIndex = 0;

    // ���݂̏o�͐��TMP
    private TextMeshProUGUI _currentTMP = null;

    // ���݂̃e�L�X�g�\�����I�������ɋN����C�x���g
    private Action _currentAfterEvent = null;

    /// <summary>
    /// �e�L�X�g�𐶐�
    /// </summary>
    /// <returns></returns>
    private async void GenerateTexts()
    {
        _textWindow.SetActiveTextFinishIcon(false);
        var token = this.GetCancellationTokenOnDestroy();
        try
        {
            // �e�L�X�g��\���J�n
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
    /// ����\�ȃC�x���g�e�L�X�g�̕\�����J�n���A�����؂�ւ���
    /// �e�L�X�g�I����ɔ�������C�x���g��ݒ�
    /// </summary>
    /// <param name="centences"></param>
    /// <param name="tmp"></param>
    /// <param name="speed"></param>
    /// <param name="afterEvent">�e�L�X�g�\���I����̃C�x���g</param>
    public async void OnStartOperateTextWithEvent(EventSentences centences, TextMeshProUGUI tmp, Action afterEvent)
    {
        // �ғ������̐���
        if (MaingameManager.Instance.CurrentOperate == OperateState.Text) return;
        if (_currentOperateTexts != null) return;
        if (centences == null)
        {
            Debug.LogWarning("�e�L�X�g���w�肳��Ă��܂���");
            return;
        }

        // �e�L�X�g�\���Ǘ��N���X���쐬
        DisplayTextOneChar displayTxt = new DisplayTextOneChar();

        // ���݂̊e�����i�[
        _currentCentences = centences;
        _currentCentenceIndex = 0;
        _currentTMP = tmp;
        _currentTMP.fontSize = _currentCentences.Sentences[_currentCentenceIndex].TextFontSize;
        _currentOperateTexts = displayTxt;
        _currentAfterEvent = afterEvent;

        // ���샂�[�h���e�L�X�g�ɕύX
        MaingameManager.Instance.ChangeOperate(OperateState.Text);

        // �e�L�X�g�\���J�n
        GenerateTexts();
    }

    /// <summary>
    /// ����\�ȃC�x���g�e�L�X�g�̕\�����J�n���A�����؂�ւ���
    /// </summary>
    /// <param name="centences"></param>
    /// <param name="tmp"></param>
    /// <param name="speed"></param>
    public async void OnStartOperateText(EventSentences centences, TextMeshProUGUI tmp)
    {
        // �ғ������̐���
        if (MaingameManager.Instance.CurrentOperate == OperateState.Text) return;
        if (_currentOperateTexts != null) return;
        if (centences == null)
        {
            Debug.LogWarning("�e�L�X�g���w�肳��Ă��܂���");
            return;
        }

        // �e�L�X�g�\���Ǘ��N���X���쐬
        DisplayTextOneChar displayTxt = new DisplayTextOneChar();

        // ���݂̊e�����i�[
        _currentCentences = centences;
        _currentCentenceIndex = 0;
        _currentTMP = tmp;
        _currentTMP.fontSize = _currentCentences.Sentences[_currentCentenceIndex].TextFontSize;
        _currentOperateTexts = displayTxt;
        _currentAfterEvent = null;

        // ���샂�[�h���e�L�X�g�ɕύX
        MaingameManager.Instance.ChangeOperate(OperateState.Text);

        // �e�L�X�g�\���J�n
        GenerateTexts();
    }

    /// <summary>
    /// �e�L�X�g��\�����ŁA���ݑ��쒆�̃C�x���g�e�L�X�g������Ȃ�A���̃e�L�X�g�Ɉړ�
    /// �����Ō�̃e�L�X�g��������A�e�L�X�g�̕\�����I�����đ�����I��
    /// </summary>
    /// <returns>�e�L�X�g�I����ɔ��΂��鏈��</returns>
    public Action OnNextCentence()
    {
        // �ғ������̐���
        if (MaingameManager.Instance.CurrentOperate != OperateState.Text) return null;
        if (_currentOperateTexts == null) return null;

        // �܂�1�������\�����������ꍇ�A�L�����Z������
        if (_currentOperateTexts.IsAppearning)
        {
            _currentOperateTexts.OnCancel();
            return null;
        }
        // �S�����̕\�����I����Ă����ꍇ�A���͂��c���Ă��邩�ŏ������ς��
        else
        {
            // �Ō�̕��͂������ꍇ�A�e�L�X�g�\�����I�����đ�����I��
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
            // �܂����͂��c���Ă����ꍇ�A���̕��͂ֈڍs
            else
            {
                _currentCentenceIndex++;
                // �e�L�X�g�\���J�n
                GenerateTexts();
                return null;
            }
        }
    }

    /// <summary>
    /// �e�L�X�g�\���̏I��
    /// �I����̃C�x���g������ꍇ�A������Đ�
    /// </summary>
    public void OnFinishEventCentence()
    {
        // �ғ������̐���
        if (MaingameManager.Instance.CurrentOperate != OperateState.Text) return;
        if (_currentOperateTexts == null) return;

        Debug.Log("�e�L�X�g�\���I��");
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
