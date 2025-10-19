using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    protected override bool dontDestroyOnLoad => false;

    [SerializeField, Header("動かすカメラ"), BoxGroup("Camera")]
    private Camera _operateCamera = null;

    [SerializeField, Header("カメラの操作管理クラス"), BoxGroup("Camera")]
    private CameraMove _operateCameraClass = null;

    [SerializeField, Header("イベントテキスト表示管理者"), BoxGroup("Text")]
    private OperableEventTextManager _operateText = null;

    [SerializeField, Header("イベントテキストを表示するTMP"), BoxGroup("Text")]
    private TextMeshProUGUI _operateEventTMP = null;

    [SerializeField, Header("イベントテキストの1文字の表示時間：秒"), BoxGroup("Text")]
    private float _eventTextCharDuration = 0.2f;

    [SerializeField, Header("イベントテキストウインドウのフェード時間"), BoxGroup("Text")]
    private float _textWindowFadeSec = 0.5f;

    [SerializeField, Header("イベントテキストウインドウの管理クラス"), BoxGroup("Text")]
    private EventTextWindow _textWindow = null;

    // 注目を終了したときの処理。注目開始時に設定
    private Action _onStopFocusEvent = null;

    /// <summary>
    /// カメラの視点方向を右に回転
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async void OnCameraRotateToRight(CancellationToken token)
    {
        MaingameManager.Instance.ChangeOperate(OperateState.Direction);
        await _operateCameraClass.OnChangeLookDiractionToRight(_operateCamera, token);
        MaingameManager.Instance.ReturnOperate();
    }

    /// <summary>
    /// カメラの視点方向を左に回転
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async void OnCameraRotateToLeft(CancellationToken token)
    {
        if (MaingameManager.Instance.CurrentOperate != OperateState.Common) return;

        MaingameManager.Instance.ChangeOperate(OperateState.Direction);
        await _operateCameraClass.OnChangeLookDiractionToLeft(_operateCamera, token);
        MaingameManager.Instance.ReturnOperate();
    }

    /// <summary>
    /// カメラの視点方向の回転をキャンセル
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async void OnCancelRotateCamera(CancellationToken token)
    {
        if (MaingameManager.Instance.CurrentOperate != OperateState.Common) return;

        MaingameManager.Instance.ChangeOperate(OperateState.Direction);
        await _operateCameraClass.OnCancelRotateCamera(_operateCamera, token);
        MaingameManager.Instance.ReturnOperate();
    }

    /// <summary>
    /// カメラを、特定のTransformの位置に移動させる
    /// </summary>
    /// <param name="focusTarget"></param>
    public void OnFocusToObject(Transform focusTarget, Action stopFocusEvent)
    {
        if (MaingameManager.Instance.CurrentOperate == OperateState.Focus) return;
        MaingameManager.Instance.ChangeOperate(OperateState.Focus);
        _operateCameraClass.OnFocusToAnyObject(_operateCamera, focusTarget);
        _onStopFocusEvent = stopFocusEvent;
    }

    /// <summary>
    /// カメラの注視をやめる
    /// </summary>
    public void OnStopFocus()
    {
        if (MaingameManager.Instance.CurrentOperate != OperateState.Focus) return;

        _operateCameraClass.OnStopFocus(_operateCamera);
        _onStopFocusEvent.Invoke();
        _onStopFocusEvent = null;
        MaingameManager.Instance.ChangeOperate(OperateState.Common);
    }

    /// <summary>
    /// カメラの正面方向にRayを放ち、当たって、インタラクトできるオブジェクトだった場合その情報を返す
    /// </summary>
    public IInteractiveObjBase OnGetObjInfoByRay(Vector3 _tapPos)
    {
        IInteractiveObjBase objInfo = null;
        Ray ray = _operateCamera.ScreenPointToRay(_tapPos);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            objInfo = hit.collider.gameObject.GetComponent<IInteractiveObjBase>();
        }

        return objInfo;
    }


    /// <summary>
    /// イベントテキストの表示開始
    /// </summary>
    /// <param name="sentences">表示する文のクラスのリスト</param>
    public async void OnStartEventText(EventSentences sentences)
    {
        MaingameManager.Instance.ChangeOperateWithoutLog(OperateState.Direction);
        await _textWindow.OnFadeInTextWindow(_textWindowFadeSec, this.GetCancellationTokenOnDestroy());
        _operateText.OnStartOperateText(sentences, _operateEventTMP);
    }

    /// <summary>
    /// イベントテキストの表示開始：テキスト終了後イベントを起こす
    /// </summary>
    /// <param name="sentences">表示する文のクラスのリスト</param>
    public async void OnStartEventText(EventSentences sentences, Action afterEvent)
    {
        MaingameManager.Instance.ChangeOperateWithoutLog(OperateState.Direction);
        await _textWindow.OnFadeInTextWindow(_textWindowFadeSec, this.GetCancellationTokenOnDestroy());
        _operateText.OnStartOperateTextWithEvent(sentences, _operateEventTMP, afterEvent);
    }

    /// <summary>
    /// イベントテキストの表示中、次の文章に送る
    /// 表示が終わっていたら、テキストウインドウを消す処理を再生
    /// </summary>
    public async void OnEventTextNextSentence()
    {
        Action afterEvent = _operateText.OnNextCentence();

        if (afterEvent != null)
        {
            MaingameManager.Instance.ChangeOperateAbs(OperateState.Direction);
            await _textWindow.OnFadeOutTextWindow(_textWindowFadeSec, this.GetCancellationTokenOnDestroy());
            MaingameManager.Instance.ChangeOperateAbs(OperateState.Text);
            afterEvent.Invoke();
        }
    }
}
