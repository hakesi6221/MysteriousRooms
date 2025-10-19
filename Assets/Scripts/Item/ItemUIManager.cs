using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    [SerializeField, Header("インベントリ画面の親")]
    private GameObject _inventryParent = null;

    [SerializeField, Header("アイテムアイコン表示ボックス")]
    private List<ItemUIIcon> _itemIcons = new List<ItemUIIcon>();

    [SerializeField, Header("インベントリUIを閉じる")]
    private Button _returnButton = null;

    // メインシーン管理クラス
    private MaingameManager _mainGameManager = null;

    // インベントリを開く直線の操作状態
    private OperateState _lastState = OperateState.None;

    // ボタンのインタラクト可能状態を更新するのを監視するための直前のOperateState
    private OperateState _previousState = OperateState.None;

    /// <summary>
    /// 各アイテムアイコンの状態を更新
    /// </summary>
    private void UpdateItemIcons()
    {
        for (int i = 0; i < _itemIcons.Count; i++)
        {
            // アイテムを所持している状態なら
            if (ItemManager.Instance.HavingItemList.Count > i)
            {
                _itemIcons[i].SetIconItem(ItemManager.Instance.HavingItemList[i]);
            }
            else
            {
                _itemIcons[i].SetEmptyIconItem();
            }
        }
    }

    /// <summary>
    /// インベントリ画面を開く
    /// </summary>
    public void OnOpenInventry()
    {
        if (_mainGameManager != null)
            if (_mainGameManager.CurrentOperate != OperateState.Common
            && _mainGameManager.CurrentOperate != OperateState.Focus) return;

        SoundManager.Instance.PlaySE(0);
        if (_mainGameManager != null)
            _lastState = _mainGameManager.CurrentOperate;
        UpdateItemIcons();
        _inventryParent.SetActive(true);
        _mainGameManager?.ChangeOperate(OperateState.Inventry);
    }

    /// <summary>
    /// インベントリ画面を閉じる
    /// </summary>
    public void OnCloseInventry()
    {
        if (_mainGameManager != null)
            if (_mainGameManager.CurrentOperate != OperateState.Inventry) return;
        SoundManager.Instance.PlaySE(1);
        _inventryParent.SetActive(false);
        _mainGameManager?.ChangeOperate(_lastState);
        _lastState = OperateState.None;
    }

    /// <summary>
    /// 引数で渡されたステートがInventryかどうかに応じてすべてのボタンのインタラクト可否を切り替える
    /// </summary>
    /// <param name="currentState">現在の操作ステート</param>
    private void UpdateButtonsInteractable(OperateState currentState)
    {
        if (currentState == OperateState.None) return;
        bool interactable = currentState.Equals(OperateState.Inventry);
        // 戻るボタンのインタラクト可否の管理
        if (_returnButton != null)
            _returnButton.interactable = interactable;
        // すべてのアイテムボタンのインタラクト可否の管理
        foreach (ItemUIIcon icon in _itemIcons)
        {
            if (icon == null) continue;
            // 何のアイテムも表示していないならインタラクト可否は変えない
            if (icon.ThisItem == null) continue;

            Button iconButton = icon.IconButton;
            if (iconButton == null) continue;
            iconButton.interactable = interactable;
        }
    }

    void Start()
    {
        UpdateItemIcons();
        // OnCloseInventry();
        _mainGameManager = MaingameManager.Instance;
        if (_mainGameManager != null) _previousState = _mainGameManager.CurrentOperate;
        // _mainGameManager?.ChangeOperate(OperateState.None);
    }

    void Update()
    {
        if (_mainGameManager == null) return;

        OperateState currentState = _mainGameManager.CurrentOperate;
        if (currentState.Equals(_previousState) is false)
        {
            _previousState = currentState;
            UpdateButtonsInteractable(currentState);
        }
    }
}
