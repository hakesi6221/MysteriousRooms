using UnityEngine;

/// <summary>
/// テレビにリモコンのアイテムクラス
/// 調べた時にフラグがオンになるのは汎用クラスと共通
/// インベントリで調べた時に、テキスト再生後、
/// </summary>
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
        // 投影するモニターのCanvasがInspectorで設定されていない場合のnullチェック
        if (_monitor == null)
        {
            Debug.LogError($"{this.name}：モニターに投影するcanvasがアタッチされていません。Inspectorを確認してください。");
            return;
        }

        // すでにモニターをつけていれば、もう付けない
        if (_monitor.activeSelf) return;
        // テキストを再生後、モニターをつける
        EventManager.Instance.OnStartEventText(_monitorTexts, TurnOnMonitor);
    }

    /// <summary>
    /// モニターをつける処理
    /// モニターの画面にRenderTextureとして張り付けているcanvasのアクティブをオンにする
    /// </summary>
    private void TurnOnMonitor()
    {
        SoundManager.Instance.PlaySE(7);
        FlagManager.Instance.Flags.SetFlagValue(_monitorFlagType, true);
        _monitor.SetActive(true);
    }
}
