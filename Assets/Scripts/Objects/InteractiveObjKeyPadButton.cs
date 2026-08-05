using NavKeypad;
using UnityEngine;

/// <summary>
/// インタラクト可能なオブジェクト：キーパッドを制御するクラス
/// インタラクト時、キーパッド側の操作を開始する
/// </summary>
public class InteractiveObjKeyPadButton : MonoBehaviour, IInteractiveObj
{
    [SerializeField, Header("このキーパッドボタン")]
    private KeypadButton _keyPadButton = null;

    public void OnInteractEvent()
    {
        _keyPadButton.PressButton();
    }

    public void UpdateCursor()
    {
        CursorManager.Instance?.OnChangeEventCursor();
    }
}
