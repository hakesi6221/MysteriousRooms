using UnityEngine;

/// <summary>
/// インタラクト時、テキストを再生後アイテムが入手できるタイプのオブジェクトを制御するクラス
/// InteractiveObjText_WithFlagBaseを継承しているため、フラグのオンオフによって、入手可能なアイテムを変えられる
/// </summary>
public class InteractiveObjTextWithFlagGetItem : InteractiveObjTextWithFlagBase
{
    protected override bool IsAfterEvent => true;

    [SerializeField, Header("フラグがオフの時の取得するアイテムのオブジェクト")]
    private GameObject _objOff = null;

    [SerializeField, Header("フラグがオフの時の取得するアイテムの情報")]
    private ItemBase _thisItemInfoOff = null;

    [SerializeField, Header("フラグがオンの時の取得するアイテムのオブジェクト")]
    private GameObject _objOn = null;

    [SerializeField, Header("フラグがオンの時の取得するアイテムの情報")]
    private ItemBase _thisItemInfoOn = null;

    protected override void TextAfterEvent()
    {
        if (_objOff != null)
        {
            _objOff.SetActive(false);
        }
        if (_thisItemInfoOff != null)
        {
            _thisItemInfoOff.OnPickUpEvent();
        }
    }

    protected override void OnFlagTextAfterEvent()
    {
        if (_objOn != null)
        {
            _objOn.SetActive(false);
        }
        if (_thisItemInfoOn != null)
        {
            _thisItemInfoOn.OnPickUpEvent();
        }
    }
}
