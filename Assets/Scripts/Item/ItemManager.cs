using System.Collections.Generic;
using UnityEngine;

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
    /// アイテムの入手、リストへの追加
    /// </summary>
    /// <param name="item">アイテム情報</param>
    public void AddItem(ItemBase item)
    {
        if (item == null)
        {
            Debug.LogError("{this.name}:追加アイテムが正しく渡されませんでした。");
            return;
        }
        _havingItemList?.Add(item);
    }

    /// <summary>
    /// 配列の番号から所持済みのアイテムの情報を取得する
    /// </summary>
    /// <param name="index">情報を取得したいアイテムのインデックス</param>
    /// <returns></returns>
    public ItemBase GetHavingItemByIndex(int index)
    {
        if (index < 0
            || _havingItemList.Count <= index)
        {
            Debug.LogError($"{this.name}:配列の範囲外が指定されました。");
            return null;
        }

        return _havingItemList[index];
    }

    /// <summary>
    /// 所持アイテムの数
    /// </summary>
    public int ItemCount => _havingItemList.Count;
}
