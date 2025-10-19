using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjText_GetItem : InteractiveObjTextBase
{
    protected override bool _isAfterEvent => true;

    [SerializeField, Header("取得するアイテムのオブジェクト")]
    private GameObject _obj = null;

    [SerializeField, Header("取得するアイテムの情報")]
    private ItemBase _thisItemInfo = null;

    protected override void TextAfterEvent()
    {
        if (_obj != null)
        {
            _obj.SetActive(false);
        }
        if (_thisItemInfo != null)
        {
            _thisItemInfo.OnPickUpEvent();
        }
        SoundManager.Instance.PlaySE(5);
    }
}
