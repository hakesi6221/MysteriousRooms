using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// �J�[�\�������킹���I�u�W�F�N�g�𒲂ׂ�
    /// </summary>
    /// <param name="context"></param>
    public void OnCheckObject(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        // ����X�e�[�g�ł̑���Ǘ�
        if (MaingameManager.Instance.CurrentOperate != OperateState.Common && MaingameManager.Instance.CurrentOperate != OperateState.Focus) return;
        // UI�ɃJ�[�\���������Ă���ꍇ�A���͒��ׂ��Ȃ�
        if (EventSystem.current.IsPointerOverGameObject()) return;
        // ���݂̃|�C���^�[���擾���A�ł��Ȃ���������s���Ȃ�
        Pointer pointer = Pointer.current;
        if (pointer == null) return;

        Vector3 pointerPos = pointer.position.ReadValue();
        IInteractiveObjBase tapObj = EventManager.Instance.OnGetObjInfoByRay(pointerPos);

        if (tapObj == null) return;
        Debug.Log("�C�x���g�I�u�W�F�N�g���^�b�`");
        tapObj.OnIntractEvent();
        SoundManager.Instance.PlaySE(4);
    }

    /// <summary>
    /// �e�L�X�g���\�����̏ꍇ�A�e�L�X�g����Ȃǂ̏������s��
    /// </summary>
    /// <param name="context"></param>
    public void OnNextText(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) return;
        if (MaingameManager.Instance.CurrentOperate != OperateState.Text) return;

        EventManager.Instance.OnEventTextNextSentence();
        SoundManager.Instance.PlaySE(0);
    }

    /// <summary>
    /// �J�������E�ɉ�]
    /// </summary>
    public void DirectionRight()
    {
        EventManager.Instance.OnCameraRotateToRight(this.GetCancellationTokenOnDestroy());
    }

    /// <summary>
    /// �J���������ɉ�]
    /// </summary>
    public void DirectionLeft()
    {
        EventManager.Instance.OnCameraRotateToLeft(this.GetCancellationTokenOnDestroy());
    }

    /// <summary>
    /// ���݂̃|�C���^�[�̏�Ԃ��擾���A����ɍ��킹�ăJ�[�\����ύX����
    /// </summary>
    private void CheckObjOnPointer()
    {
        if (MaingameManager.Instance.CurrentOperate != OperateState.Common && MaingameManager.Instance.CurrentOperate != OperateState.Focus && MaingameManager.Instance.CurrentOperate != OperateState.Text)
        {
            CursorManager.Instance.OnChangeCommonCurSor();
            return;
        }

        if (MaingameManager.Instance.CurrentOperate == OperateState.Text)
        {
            CursorManager.Instance.OnChangeTextCursor();
            return;
        }

        // ���݂̃|�C���^�[���擾���A�ł��Ȃ���������s���Ȃ�
        Pointer pointer = Pointer.current;
        if (pointer == null) return;

        Vector3 pointerPos = pointer.position.ReadValue();
        IInteractiveObjBase tapObj = EventManager.Instance.OnGetObjInfoByRay(pointerPos);

        // �J�[�\�������킹�Ă���I�u�W�F�N�g���Ȃ��Ȃ�A�J�[�\���̓f�t�H���g��
        if (tapObj == null)
        {
            CursorManager.Instance.OnChangeCommonCurSor();
            return;
        }
        // UI�ɃJ�[�\���������Ă���Ȃ�A�f�t�H���g�̃J�[�\����
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CursorManager.Instance.OnChangeCommonCurSor();
            return;
        }

        // �J�[�\�������킹���I�u�W�F�N�g�̎�ނɂ���ăJ�[�\����ς���

        if (tapObj is InteractiveObjFocusBase)
            // �����ł���I�u�W�F�N�g�̏ꍇ
            CursorManager.Instance.OnChangeFocusCursor();
        else if (tapObj is InteractiveObjTextBase)
            // �e�L�X�g�𔭐�������I�u�W�F�N�g�̏ꍇ
            CursorManager.Instance.OnChangeEventCursor();
        else if (tapObj is InteractiveObjText_WithFlagBase)
            // �e�L�X�g�𔭐�������(�t���O����)�I�u�W�F�N�g�̏ꍇ
            CursorManager.Instance.OnChangeEventCursor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckObjOnPointer();
    }
}
