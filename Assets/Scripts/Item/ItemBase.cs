using System;
using UnityEngine;

/// <summary>
/// アイテムの情報を格納するクラスを作成するための基底クラス
/// これを継承してアイテムのクラスを作成する
/// 情報はアタッチしたオブジェクトのInspectorで入力する形で、prefabを作っていく想定
///
/// 名前、インタラクト時のテキスト、アイコン用のテクスチャの情報と、
/// 入手後イベント、インベントリでのインタラクト後イベントの関数を持っている
/// </summary>
public abstract class ItemBase : ScriptableObject
{
    [SerializeField, Header("アイテムの名前")]
    private string _itemName = string.Empty;

    /// <summary>
    /// アイテムの名前
    /// </summary>
    public string ItemName { get { return _itemName; } }

    [SerializeField, Header("アイテムを調べた時のテキスト")]
    private EventSentences _itemSummary = null;

    /// <summary>
    /// アイテムを調べた時のテキスト
    /// </summary>
    public EventSentences ItemSummary { get { return _itemSummary; } }

    [SerializeField, Header("インベントリにアイコンとして表示するテクスチャ")]
    private Sprite _itemTexture = null;

    /// <summary>
    /// インベントリにアイコンとして表示するテクスチャ
    /// </summary>
    public Sprite ItemTexture { get { return _itemTexture; } }

    /// <summary>
    /// このアイテムを入手した後に発生するイベント
    /// </summary>
    public abstract void OnPickUpEvent();

    /// <summary>
    /// このアイテムをインベントリで調べた時に発生するイベント
    /// </summary>
    public virtual void OnAfterCheckEvent()
    {
        // 元は処理なし
    }
}
