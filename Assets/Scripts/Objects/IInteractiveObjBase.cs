using UnityEngine;

public interface IInteractiveObjBase
{
    /// <summary>
    ///  オブジェクトを調べた時に呼ばれる処理
    /// </summary>
    public void OnIntractEvent();

    /// <summary>
    /// カーソルアイコンの更新
    /// </summary>
    public void UpdateCursor()
    {
        CursorManager.Instance?.OnChangeCommonCurSor();
    }
}
