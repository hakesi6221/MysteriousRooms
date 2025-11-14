using UnityEngine;

public class Item_ScrewDriver : ItemBase
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
