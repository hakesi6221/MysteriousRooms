using UnityEngine;
using UnityEngine.UI;

public class ItemUIIcon : MonoBehaviour
{
    [SerializeField, Header("アイコンimage")]
    private Image _iconImage = null;

    /// <summary>
    /// アイコンimage
    /// </summary>
    public Image IconImage => _iconImage;

    [SerializeField, Header("アイコンbutton")]
    private Button _iconButton = null;

    /// <summary>
    /// アイコンbutton
    /// </summary>
    public Button IconButton => _iconButton;

    // 現在このアイコンで表示しているアイテム
    private ItemBase _thisItem = null;

    /// <summary>
    /// 現在このアイコンで表示しているアイテム
    /// </summary>
    public ItemBase ThisItem => _thisItem;

    /// <summary>
    /// アイコンのボタンを押したときに発生する処理
    /// アイテムの説明を表示し、その後の処理を再生
    /// </summary>
    public void OnCheckItem()
    {
        if (_thisItem == null) return;
        if (MaingameManager.Instance.CurrentOperate != OperateState.Inventry) return;

        SoundManager.Instance.PlaySE(0);
        EventManager.Instance.OnStartEventText(_thisItem.ItemSummury, _thisItem.OnAfterCheckEvent);
    }

    /// <summary>
    /// アイコンのアイテムを空にする
    /// </summary>
    public void SetEmptyIconItem()
    {
        _thisItem = null;
        _iconImage.sprite = null;
        _iconImage.enabled = false;
        _iconButton.interactable = false;
    }

    /// <summary>
    /// アイコンのアイテムを設定する
    /// </summary>
    /// <param name="item">設定するアイテム</param>
    public void SetIconItem(ItemBase item)
    {
        if (item == null) return;

        _thisItem = item;
        _iconImage.enabled = true;
        _iconImage.sprite = item.ItemTexture;
        _iconButton.interactable = true;
    }
}
