using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// カーソルを合わせたオブジェクトを調べる
    /// </summary>
    /// <param name="context"></param>
    public void OnCheckObject(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        // 操作ステートでの操作管理
        if (MaingameManager.Instance.CurrentOperate != OperateState.Common && MaingameManager.Instance.CurrentOperate != OperateState.Focus) return;
        // UIにカーソルがあっている場合、物は調べられない
        if (EventSystem.current.IsPointerOverGameObject()) return;
        // 現在のポインターを取得し、できなかったら実行しない
        Pointer pointer = Pointer.current;
        if (pointer == null) return;

        Vector3 pointerPos = pointer.position.ReadValue();
        IInteractiveObjBase tapObj = EventManager.Instance.OnGetObjInfoByRay(pointerPos);

        if (tapObj == null) return;
        Debug.Log("イベントオブジェクトをタッチ");
        tapObj.OnIntractEvent();
        SoundManager.Instance.PlaySE(4);
    }

    /// <summary>
    /// テキストが表示中の場合、テキスト送りなどの処理を行う
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
    /// カメラを右に回転
    /// </summary>
    public void DirectionRight()
    {
        EventManager.Instance.OnCameraRotateToRight(this.GetCancellationTokenOnDestroy());
    }

    /// <summary>
    /// カメラを左に回転
    /// </summary>
    public void DirectionLeft()
    {
        EventManager.Instance.OnCameraRotateToLeft(this.GetCancellationTokenOnDestroy());
    }

    /// <summary>
    /// 現在のポインターの状態を取得し、それに合わせてカーソルを変更する
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

        // 現在のポインターを取得し、できなかったら実行しない
        Pointer pointer = Pointer.current;
        if (pointer == null) return;

        Vector3 pointerPos = pointer.position.ReadValue();
        IInteractiveObjBase tapObj = EventManager.Instance.OnGetObjInfoByRay(pointerPos);

        // カーソルを合わせているオブジェクトがないなら、カーソルはデフォルトに
        if (tapObj == null)
        {
            CursorManager.Instance.OnChangeCommonCurSor();
            return;
        }
        // UIにカーソルがあっているなら、デフォルトのカーソルに
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CursorManager.Instance.OnChangeCommonCurSor();
            return;
        }

        // カーソルを合わせたオブジェクトの種類によってカーソルを変える

        if (tapObj is InteractiveObjFocusBase)
            // 注視できるオブジェクトの場合
            CursorManager.Instance.OnChangeFocusCursor();
        else if (tapObj is InteractiveObjTextBase)
            // テキストを発生させるオブジェクトの場合
            CursorManager.Instance.OnChangeEventCursor();
        else if (tapObj is InteractiveObjText_WithFlagBase)
            // テキストを発生させる(フラグあり)オブジェクトの場合
            CursorManager.Instance.OnChangeEventCursor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckObjOnPointer();
    }
}
