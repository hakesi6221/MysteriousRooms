using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// メイン画面の、基本画面のボタンの状態の管理を行う
/// 操作状態に応じて、
/// </summary>
public class CommonButtonManager : MonoBehaviour
{
    [SerializeField, Header("矢印のボタン")]
    private GameObject _arrowButtons = null;

    [SerializeField, Header("インベントリボタン")]
    private GameObject _inventryButton = null;

    [SerializeField, Header("設定ボタン")]
    private GameObject _configButton = null;

    [SerializeField, Header("注視終了ボタン")]
    private GameObject _finishFocusButton = null;

    // Update is called once per frame
    void Update()
    {
        if (MaingameManager.Instance == null)
        {
            Debug.LogError($"{this.name}：MaingameManagerが存在しません。配置忘れもしくは、メイン画面に配置されている可能性があります。");
            return;
        }

        EventSystem.current?.SetSelectedGameObject(null);
        _arrowButtons?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Common);
        _inventryButton?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Common || MaingameManager.Instance.CurrentOperate == OperateState.Focus);
        _configButton?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Common || MaingameManager.Instance.CurrentOperate == OperateState.Focus);
        _finishFocusButton?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Focus);
    }
}
