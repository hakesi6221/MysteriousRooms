using System.Collections.Generic;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    protected override bool dontDestroyOnLoad => false;

    // 持っているアイテムのリスト
    private List<ItemBase> _havingItemList = new List<ItemBase>();

    public List<ItemBase> HavingItemList { get { return _havingItemList; } }
}
