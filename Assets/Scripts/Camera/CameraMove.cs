using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    [SerializeField, Header("デフォルトのカメラの座標：unit")]
    private Vector3 _defaultPos = Vector3.zero;

    [SerializeField, Header("デフォルトのカメラの向き：オイラー角")]
    private Vector3 _defaultAngle = Vector3.zero;

    [SerializeField, Header("カメラの視点変更所要時間：秒")]
    private float _changeDiractionSec = 0.5f;

    // 回転もしくは移動中か
    private bool _isProcessing = false;

    // オブジェクトの注目中か
    private bool _isFocusing = false;

    // 現在の通常時のYRoatation
    private float _currentYAngle = 0f;

    // キャンセルソース
    private CancellationTokenSource _cts = new CancellationTokenSource();

    /// <summary>
    /// 視点方向の変更をキャンセル
    /// </summary>
    /// <param name="camera">動かすカメラ</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask OnCancelRotateCamera(Camera camera, CancellationToken token)
    {
        if (_isFocusing) return;
        if (!_isProcessing) return;
        _cts.Cancel();

        try
        {
            await camera.transform.DORotate(new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z), _changeDiractionSec)
               .SetEase(Ease.OutSine)
               .ToUniTask(cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            _isProcessing = false;
            return;
        }

        _currentYAngle = camera.transform.eulerAngles.y;
        _isProcessing = false;
    }

    /// <summary>
    /// 通常時、視点を右にに変更
    /// </summary>
    /// <param name="camera">動かすカメラ</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask OnChangeLookDiractionToRight(Camera camera, CancellationToken token)
    {
        if (_isFocusing) return;
        if (_isProcessing) return;

        var linkedSoures = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token);
        var linkedToken = linkedSoures.Token;

        _isProcessing = true;
        _currentYAngle = camera.transform.eulerAngles.y;

        SoundManager.Instance.PlaySE(0);
        try
        {
            await camera.transform.DORotate(new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y + 90f, camera.transform.eulerAngles.z), _changeDiractionSec)
               .SetEase(Ease.OutSine)
               .ToUniTask(cancellationToken: linkedToken);
        }
        catch (OperationCanceledException)
        {
            _cts = new CancellationTokenSource();
            return;
        }

        _currentYAngle = camera.transform.eulerAngles.y;
        _isProcessing = false;
    }

    /// <summary>
    /// 通常時、視点を左に変更
    /// </summary>
    /// <param name="camera">動かすカメラ</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask OnChangeLookDiractionToLeft(Camera camera, CancellationToken token)
    {
        if (_isFocusing) return;
        if (_isProcessing) return;

        var linkedSoures = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, token);
        var linkedToken = linkedSoures.Token;

        _isProcessing = true;
        _currentYAngle = camera.transform.eulerAngles.y;

        SoundManager.Instance.PlaySE(0);
        try
        {
            await camera.transform.DORotate(new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y - 90f, camera.transform.eulerAngles.z), _changeDiractionSec)
                                   .SetEase(Ease.OutSine)
                                   .ToUniTask(cancellationToken: linkedToken);
        }
        catch (OperationCanceledException)
        {
            _cts = new CancellationTokenSource();
            return;
        }

        _currentYAngle = camera.transform.eulerAngles.y;
        _isProcessing = false;
    }

    /// <summary>
    /// 対象のtransformに座標と回転を合わせる
    /// </summary>
    /// <param name="camera">動かすカメラ</param>
    /// <param name="focusObj">注視対象</param>
    public void OnFocusToAnyObject(Camera camera, Transform focusObj)
    {
        if (_isProcessing) return;
        if (_isFocusing) return;

        SoundManager.Instance.PlaySE(0);
        camera.transform.position = focusObj.position;
        camera.transform.rotation = focusObj.rotation;

        _isFocusing = true;
    }

    /// <summary>
    /// 通常状態に戻る
    /// </summary>
    /// <param name="camera">動かすカメラ</param>
    public void OnStopFocus(Camera camera)
    {
        if (_isProcessing) return;
        if (!_isFocusing) return;

        SoundManager.Instance.PlaySE(1);
        camera.transform.position = _defaultPos;
        camera.transform.rotation = Quaternion.Euler(_defaultAngle.x, _currentYAngle, _defaultAngle.z);

        _isFocusing = false;
    }
}
