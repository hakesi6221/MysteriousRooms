using System.Collections.Generic;

/// <summary>
/// 現在所持しているアイテムを補完するシングルトンクラス
/// 補完用のリストと、そのリストの参照プロパティを持っている
/// </summary>
public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    protected override bool dontDestroyOnLoad => false;

    // 持っているアイテムのリスト
    private List<ItemBase> _havingItemList = new List<ItemBase>();

    /// <summary>
    /// 持っているアイテムのリスト
    /// </summary>
    public List<ItemBase> HavingItemList { get { return _havingItemList; } }
}
