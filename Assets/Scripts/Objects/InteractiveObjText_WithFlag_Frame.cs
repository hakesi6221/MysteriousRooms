using UnityEngine;

public class InteractiveObjText_WithFlag_Frame : InteractiveObjText_WithFlagBase
{
    protected override bool _isAfterEvent => true;
    [SerializeField, Header("絵画のコライダー")]
    private Collider _collider;

    [SerializeField, Header("絵画のオブジェクト")]
    private Transform _frameObj = null;

    [SerializeField, Header("外した絵画の置き場所")]
    private Transform _framePlace = null;

    protected override void OnFlagTextAfterEvent()
    {
        Debug.Log("絵画外した");
        SoundManager.Instance.PlaySE(8);
        _collider.enabled = false;
        _frameObj.position = _framePlace.position;
        _frameObj.rotation = _framePlace.rotation;
    }
}
