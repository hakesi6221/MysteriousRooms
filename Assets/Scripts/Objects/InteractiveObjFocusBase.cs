using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveObjFocusBase : MonoBehaviour, IInteractiveObjBase
{
    protected virtual bool _hasStartFocusEvent { get; private set; } = false;

    protected virtual bool _hasFinishFocusEvent { get; private set; } = false;

    [SerializeField, Header("カメラを移動させるTransform")]
    private Transform _focusTarget = null;

    [SerializeField, Header("注目時だけオンにしたいコライダー")]
    private List<Collider> _interactiveObj = new List<Collider>();

    [SerializeField, Header("自分自身のコライダー")]
    private List<Collider> _thisObjColliders = new List<Collider>();

    void Start()
    {
        foreach (Collider thisObj in _thisObjColliders)
        {
            thisObj.enabled = true;
        }
        foreach (Collider interactive in _interactiveObj)
        {
            interactive.enabled = false;
        }
    }

    protected virtual void OnStartFocusEvent()
    {
        return;
    }

    protected virtual void OnFinishFocusEvent()
    {
        return;
    }

    public void OnIntractEvent()
    {
        foreach (Collider thisObj in _thisObjColliders)
        {
            thisObj.enabled = false;
        }
        foreach (Collider interactive in _interactiveObj)
        {
            interactive.enabled = true;
        }
        EventManager.Instance.OnFocusToObject(_focusTarget, OnStopFocusEvent);
        if (_hasStartFocusEvent)
            OnStartFocusEvent();
    }

    public void OnStopFocusEvent()
    {
        foreach (Collider thisObj in _thisObjColliders)
        {
            thisObj.enabled = true;
        }
        foreach (Collider interactive in _interactiveObj)
        {
            interactive.enabled = false;
        }
        if (_hasFinishFocusEvent)
            OnFinishFocusEvent();
    }
}
