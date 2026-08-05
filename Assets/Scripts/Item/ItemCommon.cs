using UnityEngine;

/// <summary>
/// 汎用的なアイテム用の継承クラス
/// アイテムを拾ったときに、指定したフラグをオンにするようになっている
/// インベントリで調べた時のイベントはなし
/// </summary>
[CreateAssetMenu(fileName = "ItemCommon", menuName = "ScriptableObjects/CreateCommonItemData")]
public class ItemCommon : ItemBase
{
    [SerializeField, Header("このオブジェクトのフラグタイプ")]
    private Flags _flagType;

    public override void OnPickUpEvent()
    {
        FlagManager.Instance.Flags.SetFlagValue(_flagType, true);
        ItemManager.Instance.AddItem(this);
    }
}
