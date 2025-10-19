using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;



// ���ݑ��삵�Ă�����̂�\���񋓌^
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

    [SerializeField, Header("���C���V�[���n�܂������ɕ\�������e�L�X�g")]
    private EventSentences _startSentence = null;

    // ���ݑ��삵�Ă������
    private OperateState _operateState = OperateState.None;

    // ���O���삵�Ă�������
    private OperateState _lastOperate = OperateState.None;

    // �����Ɏc��Ȃ��ύX���s���Ă��邩
    private bool _isChangeStateAbs = false;

    /// <summary>
    /// ���ݑ��삵�Ă������
    /// </summary>
    public OperateState CurrentOperate { get { return _operateState; } }

    /// <summary>
    /// ����Ώۂ�ύX
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
    /// �ύX��̏�Ԃ�ۑ������ɑ���Ώۂ�ύX
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeOperateWithoutLog(OperateState newState)
    {
        _isChangeStateAbs = true;
        _lastOperate = _operateState;
        _operateState = newState;
    }

    // ���݂̏�Ԃ𗚗��Ɏc�����A����Ώۂ�ύX
    public void ChangeOperateAbs(OperateState newState)
    {
        _operateState = newState;
    }

    /// <summary>
    /// ���O�̑���Ώۂɖ߂�
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
        // �J�n���ɕ\������e�L�X�g������Ε\������
        if (_startSentence == null || _startSentence.Sentences.Count != 0)
            EventManager.Instance.OnStartEventText(_startSentence, () => ChangeOperateAbs(OperateState.Common));
        // �Ȃ��Ȃ畁�ʂɊJ�n
        else
            ChangeOperateAbs(OperateState.Common);
    }
}
