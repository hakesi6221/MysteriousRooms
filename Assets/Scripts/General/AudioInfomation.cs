using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AudioType
{
    BGM,
    SE,
    VOICE,
    OTHERS,
}

[Serializable]
public class AudioInfomation
{
    [SerializeField, Header("�T�E���h�̎��")]
    private AudioType _type = AudioType.BGM;
    /// <summary>
    /// �T�E���h�̎��
    /// </summary>
    public AudioType Type { get { return _type; } }

    [SerializeField, Header("�T�E���h�̃N���b�v")]
    private AudioClip _clip = null;
    /// <summary>
    /// �T�E���h�̃N���b�v
    /// </summary>
    public AudioClip Clip { get { return _clip; } }

    [SerializeField, Header("�T�E���h�̃{�����[��"), Range(0.0f, 1.0f)]
    private float _volume = 1.0f;
    /// <summary>
    /// �T�E���h�̃{�����[��
    /// </summary>
    public float Volume { get { return _volume; } }

    [SerializeField, Header("���[�v���邩")]
    private bool _loop = false;
    /// <summary>
    /// ���[�v���邩
    /// </summary>
    public bool Loop { get { return _loop; } }

    [SerializeField, Header("�Đ��̃I�t�Z�b�g"), Range(0.0f, 1.0f)]
    private float _ofset = 0f;
    /// <summary>
    /// �Đ��̃I�t�Z�b�g
    /// </summary>
    public float Ofset { get { return _ofset; } }
}
