using System;
using UnityEngine;

/// <summary>
/// サウンド素材の種類を示すEnum
/// </summary>
public enum AudioType
{
    BGM,
    SE,
    VOICE,
    OTHERS,
}

/// <summary>
/// サウンドの情報を格納するためのクラス
/// サウンドの種類、クリップファイル、ボリューム、ループの有無が設定可能
/// </summary>
[Serializable]
public class AudioInfomation
{
    [SerializeField, Header("サウンドの種類")]
    private AudioType _type = AudioType.BGM;
    /// <summary>
    /// サウンドの種類
    /// </summary>
    public AudioType Type { get { return _type; } }

    [SerializeField, Header("サウンドのクリップ")]
    private AudioClip _clip = null;
    /// <summary>
    /// サウンドのクリップ
    /// </summary>
    public AudioClip Clip { get { return _clip; } }

    [SerializeField, Header("サウンドのボリューム"), Range(0.0f, 1.0f)]
    private float _volume = 1.0f;
    /// <summary>
    /// サウンドのボリューム
    /// </summary>
    public float Volume { get { return _volume; } }

    [SerializeField, Header("ループするか")]
    private bool _loop = false;
    /// <summary>
    /// ループするか
    /// </summary>
    public bool Loop { get { return _loop; } }

    [SerializeField, Header("再生のオフセット"), Range(0.0f, 1.0f)]
    private float _ofset = 0f;
    /// <summary>
    /// 再生のオフセット
    /// </summary>
    public float Ofset { get { return _ofset; } }
}
