/// <summary>
/// インタラクト可能のオブジェクトに共通した処理を実装するためのインターフェース
/// 調べた時に呼ばれる処理(継承)と、カーソルが触れた時のカーソルアイコンの更新処理を持っている
/// </summary>
public interface IInteractiveObj
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
