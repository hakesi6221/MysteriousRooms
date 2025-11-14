using System;
using UnityEngine;

[Serializable]
public abstract class ItemBase : MonoBehaviour
{
    [SerializeField, Header("アイテムの名前")]
    private string _itemName = string.Empty;

    /// <summary>
    /// アイテムの名前
    /// </summary>
    public string ItemName { get { return _itemName; } }

    [SerializeField, Header("アイテムを調べた時のテキスト")]
    private EventSentences _itemSummury = null;

    /// <summary>
    /// アイテムを調べた時のテキスト
    /// </summary>
    public EventSentences ItemSummury { get { return _itemSummury; } }

    [SerializeField, Header("インベントリにアイコンとして表示するテクスチャ")]
    private Sprite _itemTexture = null;

    /// <summary>
    /// インベントリにアイコンとして表示するテクスチャ
    /// </summary>
    public Sprite ItemTexture { get { return _itemTexture; } }

    public abstract void OnPickUpEvent();

    public abstract void OnAfterCheckEvent();
}
