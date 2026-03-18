using UnityEngine;

/// <summary>
/// ゲーム開始時にフレームレートを固定するクラス
/// </summary>
public class FixedFrameRate : MonoBehaviour
{
    void Awake()
    {
        // フレームレートを固定
        Application.targetFrameRate = 60;
    }
}
