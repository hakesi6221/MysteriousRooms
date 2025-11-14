using UnityEngine;

public class Item_TVRemote : ItemBase
{
    [SerializeField, Header("モニターをつけるときのテキスト")]
    private EventSentences _monitorTexts = null;

    [SerializeField, Header("モニターをつけるフラグタイプ")]
    private Flags _monitorFlagType;

    [SerializeField, Header("モニターに移すキャンバス")]
    private GameObject _monitor = null;

    [SerializeField, Header("このオブジェクトのフラグタイプ")]
    private Flags _flagType;

    public override void OnPickUpEvent()
    {
        FlagManager.Instance.Flags.SetFlagValue(_flagType, true);
        ItemManager.Instance.HavingItemList.Add(this);
    }

    public override void OnAfterCheckEvent()
    {
        if (_monitor == null) return;

        if (_monitor.activeSelf) return;
        EventManager.Instance.OnStartEventText(_monitorTexts, TurnOnMonitor);
    }

    private void TurnOnMonitor()
    {
        SoundManager.Instance.PlaySE(7);
        FlagManager.Instance.Flags.SetFlagValue(_monitorFlagType, true);
        _monitor.SetActive(true);
    }
}
