using UnityEngine;

public class InteractiveObjText_WithFlag_GetItem : InteractiveObjText_WithFlagBase
{
    protected override bool _isAfterEvent => true;

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
