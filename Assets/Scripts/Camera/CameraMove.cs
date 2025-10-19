using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    [SerializeField, Header("�f�t�H���g�̃J�����̍��W�Funit")]
    private Vector3 _defaultPos = Vector3.zero;

    [SerializeField, Header("�f�t�H���g�̃J�����̌����F�I�C���[�p")]
    private Vector3 _defaultAngle = Vector3.zero;

    [SerializeField, Header("�J�����̎��_�ύX���v���ԁF�b")]
    private float _changeDiractionSec = 0.5f;

    // ��]�������͈ړ�����
    private bool _isProcessing = false;

    // �I�u�W�F�N�g�̒��ڒ���
    private bool _isFocusing = false;

    // ���݂̒ʏ펞��YRoatation
    private float _currentYAngle = 0f;

    // �L�����Z���\�[�X
    private CancellationTokenSource _cts = new CancellationTokenSource();

    /// <summary>
    /// ���_�����̕ύX���L�����Z��
    /// </summary>
    /// <param name="camera">�������J����</param>
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
    /// �ʏ펞�A���_���E�ɂɕύX
    /// </summary>
    /// <param name="camera">�������J����</param>
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
    /// �ʏ펞�A���_�����ɕύX
    /// </summary>
    /// <param name="camera">�������J����</param>
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
    /// �Ώۂ�transform�ɍ��W�Ɖ�]�����킹��
    /// </summary>
    /// <param name="camera">�������J����</param>
    /// <param name="focusObj">�����Ώ�</param>
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
    /// �ʏ��Ԃɖ߂�
    /// </summary>
    /// <param name="camera">�������J����</param>
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
