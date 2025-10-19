using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
    protected override bool dontDestroyOnLoad => false;

    [SerializeField, Header("注視するオブジェクトのカーソル")]
    private Texture2D _focusTexture = null;

    [SerializeField, Header("イベントオブジェクトのカーソル")]
    private Texture2D _eventTexture = null;

    [SerializeField, Header("テキスト中のカーソル")]
    private Texture2D _textTexture = null;

    /// <summary>
    /// マウスカーソルをデフォルトにする
    /// </summary>
    public void OnChangeCommonCurSor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// マウスカーソルを注視オブジェクト用にする
    /// </summary>
    public void OnChangeFocusCursor()
    {
        Cursor.SetCursor(_focusTexture, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// マウスカーソルをテキスト発生用にする
    /// </summary>
    public void OnChangeEventCursor()
    {
        Cursor.SetCursor(_eventTexture, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// マウスカーソルをテキスト中用にする
    /// </summary>
    public void OnChangeTextCursor()
    {
        Cursor.SetCursor(_textTexture, Vector2.zero, CursorMode.Auto);
    }
}
