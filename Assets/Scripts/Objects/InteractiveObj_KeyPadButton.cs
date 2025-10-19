using System.Collections;
using System.Collections.Generic;
using NavKeypad;
using UnityEngine;

public class InteractiveObj_KeyPadButton : MonoBehaviour, IInteractiveObjBase
{
    [SerializeField, Header("このキーパッドボタン")]
    private KeypadButton _keyPadButton = null;

    public void OnIntractEvent()
    {
        _keyPadButton.PressButton();
    }
}
