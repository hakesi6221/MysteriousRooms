using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFrameRate : MonoBehaviour
{
    void Awake()
    {
        // フレームレートを固定
        Application.targetFrameRate = 60;
    }
}
