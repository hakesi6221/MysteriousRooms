using UnityEngine;

/// <summary>
/// 汎用的なアイテム用の継承クラス
/// アイテムを拾ったときに、指定したフラグをオンにするようになっている
/// インベントリで調べた時のイベントはなし
/// </summary>
public class Item_Common : ItemBase
{
    [SerializeField, Header("このオブジェクトのフラグタイプ")]
    private Flags _flagType;

    public override void OnPickUpEvent()
    {
        FlagManager.Instance.Flags.SetFlagValue(_flagType, true);
        ItemManager.Instance.HavingItemList.Add(this);
    }

    public override void OnAfterCheckEvent()
    {

    }
}
