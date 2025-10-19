using UnityEngine;
using UnityEngine.EventSystems;

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
        EventSystem.current?.SetSelectedGameObject(null);
        _arrowButtons?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Common);
        _inventryButton?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Common || MaingameManager.Instance.CurrentOperate == OperateState.Focus);
        _configButton?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Common || MaingameManager.Instance.CurrentOperate == OperateState.Focus);
        _finishFocusButton?.SetActive(MaingameManager.Instance.CurrentOperate == OperateState.Focus);
    }
}
