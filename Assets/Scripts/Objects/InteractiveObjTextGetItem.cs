using UnityEngine;

/// <summary>
/// インタラクト時アイテムを入手できるタイプのオブジェクトクラス
/// InteractiveObjTextBaseを継承
/// テキスト終了後イベントとして、Inspectorで指定したアイテムを入手することができる
/// </summary>
public class InteractiveObjTextGetItem : InteractiveObjTextBase
{
    protected override bool _isAfterEvent => true;

    [SerializeField, Header("取得するアイテムのオブジェクト")]
    private GameObject _obj = null;

    [SerializeField, Header("取得するアイテムの情報")]
    private ItemBase _thisItemInfo = null;

    protected override void TextAfterEvent()
    {
        // 取得するアイテムのオブジェクトが指定されていたなら、
        // そのままオブジェクトとして配置されているものとしてアクティブをオフにする
        if (_obj != null)
        {
            _obj.SetActive(false);
        }
        if (_thisItemInfo != null)
        {
            _thisItemInfo.OnPickUpEvent();
        }
        else
        {
            Debug.LogError($"{this.name}：入手するアイテムが指定されていません。Inspectorを確認してください。");
        }
        SoundManager.Instance.PlaySE(5);
    }
}
