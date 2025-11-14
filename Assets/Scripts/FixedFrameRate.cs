using UnityEngine;

public class FixedFrameRate : MonoBehaviour
{
    void Awake()
    {
        // フレームレートを固定
        Application.targetFrameRate = 60;
    }
}
