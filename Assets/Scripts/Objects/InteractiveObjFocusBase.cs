using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仮想オブジェクトあり
/// インタラクト時にカメラが移動し、そのオブジェクトに注目するタイプのオブジェクト用のクラス
/// 注目時、余計なものが反応しないように注目状態の切り替え時にInspectorで指定したコライダーのアクティブも同時に切り替えることができる
/// 注目したとき、それを終了したときにイベントが発生するかどうかの継承可能プロパティがある
/// 上記プロパティをtrueで継承し、各関数を継承し実装した場合、実行される
/// </summary>
public class InteractiveObjFocusBase : MonoBehaviour, IInteractiveObj
{
    /// <summary>
    ///  注目開始時にイベントを発生させるか
    /// デフォルトではfalse
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual bool _hasStartFocusEvent { get; private set; } = false;

    /// <summary>
    /// 注目終了時にイベントを発生させるか
    /// デフォルトではfalse
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual bool _hasFinishFocusEvent { get; private set; } = false;

    [SerializeField, Header("カメラを移動させるTransform")]
    private Transform _focusTarget = null;

    [SerializeField, Header("注目時だけオンにしたいコライダー")]
    private List<Collider> _interactiveObj = new List<Collider>();

    [SerializeField, Header("自分自身のコライダー")]
    private List<Collider> _thisObjColliders = new List<Collider>();

    void Start()
    {
        // 操作対象のコライダーの状態の初期化
        foreach (Collider thisObj in _thisObjColliders)
        {
            thisObj.enabled = true;
        }
        foreach (Collider interactive in _interactiveObj)
        {
            interactive.enabled = false;
        }
    }

    /// <summary>
    /// 注目開始時に呼ばれるイベント
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual void OnStartFocusEvent()
    {
        return;
    }

    /// <summary>
    /// 注目開始時に呼ばれるイベント
    /// 継承して実装する必要あり
    /// </summary>
    protected virtual void OnFinishFocusEvent()
    {
        return;
    }

    public void OnIntractEvent()
    {
        // コライダーの切り替え
        foreach (Collider thisObj in _thisObjColliders)
        {
            thisObj.enabled = false;
        }
        foreach (Collider interactive in _interactiveObj)
        {
            interactive.enabled = true;
        }

        // EventManagerで注目処理を実行
        EventManager.Instance.OnFocusToObject(_focusTarget, OnStopFocusEvent);
        // イベント発生フラグがオンなら発生させる
        if (_hasStartFocusEvent)
            OnStartFocusEvent();
    }

    /// <summary>
    /// 注目を終了するときの関数
    /// 注目する処理を呼ぶときに、これを渡して、eEentManager側で終了時に呼んでもらう
    /// </summary>
    public void OnStopFocusEvent()
    {
        foreach (Collider thisObj in _thisObjColliders)
        {
            thisObj.enabled = true;
        }
        foreach (Collider interactive in _interactiveObj)
        {
            interactive.enabled = false;
        }
        if (_hasFinishFocusEvent)
            // イベント発生フラグがオンなら発生させる
            OnFinishFocusEvent();
    }

    public void UpdateCursor()
    {
        CursorManager.Instance?.OnChangeFocusCursor();
    }
}
