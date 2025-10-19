using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-90)]
public class SoundManager : MonoBehaviour
{
    #region private�I�u�W�F�N�g
    [SerializeField, Header("BGM���X�g")]
    private List<AudioInfomation> _bgmList = new List<AudioInfomation>();

    [SerializeField, Header("SE���X�g")]
    private List<AudioInfomation> _seList = new List<AudioInfomation>();

    [SerializeField, Header("�{�C�X���X�g")]
    private List<AudioInfomation> _voiceList = new List<AudioInfomation>();

    [SerializeField, Header("���̑��̉����X�g")]
    private List<AudioInfomation> _othersList = new List<AudioInfomation>();

    [SerializeField, Header("�e�~�L�T�[")]
    private AudioMixer _mixer;

    [SerializeField, Header("BGM�~�L�T�[")]
    private AudioMixerGroup _bgmMixier;

    [SerializeField, Header("SE�~�L�T�[")]
    private AudioMixerGroup _seMixier;

    [SerializeField, Header("Voice�~�L�T�[")]
    private AudioMixerGroup _voiceMixier;

    [SerializeField, Header("���̑��~�L�T�[")]
    private AudioMixerGroup _othersMixier;

    [SerializeField, Header("�f�t�H���g�t�F�[�h����"), Range(0.0f, 5.0f)]
    private float _defaultFadeRate = 0.0f;

    private List<AudioSource> _bgmSource = new List<AudioSource>();
    private List<AudioSource> _seSource = new List<AudioSource>();
    private List<AudioSource> _voiceSource = new List<AudioSource>();
    private List<AudioSource> _othersSource = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        // �e���̃��X�g��������
        _bgmSource = new List<AudioSource>();
        _seSource = new List<AudioSource>();
        _voiceSource = new List<AudioSource>();
        _othersSource = new List<AudioSource>();
        foreach (AudioInfomation audio in _bgmList)
        {
            _bgmSource.Add(CreateNewAudioSource(audio));
            _bgmSource.LastOrDefault().outputAudioMixerGroup = _bgmMixier;
        }
        foreach (AudioInfomation audio in _seList)
        {
            _seSource.Add(CreateNewAudioSource(audio));
            _seSource.LastOrDefault().outputAudioMixerGroup = _seMixier;
        }
        foreach (AudioInfomation audio in _voiceList)
        {
            _voiceSource.Add(CreateNewAudioSource(audio));
            _voiceSource.LastOrDefault().outputAudioMixerGroup = _voiceMixier;
        }
        foreach (AudioInfomation audio in _othersList)
        {
            _othersSource.Add(CreateNewAudioSource(audio));
            _othersSource.LastOrDefault().outputAudioMixerGroup = _othersMixier;
        }
    }

    private AudioSource CreateNewAudioSource(AudioInfomation audio)
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();

        switch (audio.Type)
        {
            case AudioType.BGM:
                newSource.outputAudioMixerGroup = _bgmMixier;
                break;
            case AudioType.SE:
                newSource.outputAudioMixerGroup = _seMixier;
                break;
            case AudioType.VOICE:
                newSource.outputAudioMixerGroup = _voiceMixier;
                break;
            case AudioType.OTHERS:
                newSource.outputAudioMixerGroup = _othersMixier;
                break;
        }

        return newSource;
    }

    /// <summary>
    /// �T�E���h�I�����̌�n������
    /// </summary>
    /// <param name="source">�Ď�������AudioSource</param>
    /// <returns></returns>
    private IEnumerator FinishSoundProcess(AudioSource source)
    {
        if (source == null) yield break;

        yield return new WaitUntil(() => !source.isPlaying);

        source.clip = null;
        source.volume = 0f;
        source.loop = false;
        source.time = 0f;
    }

    /// <summary>
    /// �T�E���h�����ۂɖ炷
    /// </summary>
    /// <param name="source">�炷�\�[�X</param>
    /// <param name="audio">�炷���̏��</param>
    private void OnPlaySound(AudioSource source, AudioInfomation audio)
    {
        source.clip = audio.Clip;
        source.volume = audio.Volume;
        source.loop = audio.Loop;
        source.time = audio.Ofset;

        source.Play();
        StartCoroutine(FinishSoundProcess(source));
    }

    /// <summary>
    /// �T�E���h�̈ꎞ��~����
    /// </summary>
    /// <param name="source">�����������\�[�X</param>
    private void OnUnPauseSound(AudioSource source)
    {
        source.UnPause();
    }

    /// <summary>
    /// �T�E���h�̈ꎞ��~����
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="source">�����������\�[�X</param>
    /// <param name="endVolume">�ŏI�I�ȃ{�����[��</param>
    /// <param name="fadeTime">�t�F�[�h�̎���</param>
    private void OnUnPauseSoundWithFadeIn(AudioSource source, float endVolume, float fadeTime)
    {
        source.UnPause();
        StartCoroutine(FadeMoveSound(source, fadeTime, endVolume));
    }

    /// <summary>
    /// �T�E���h�����ۂɖ炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="source">�炷�\�[�X</param>
    /// <param name="audio">�炷���̏��</param>
    /// <param name="startVolume">�ŏ��̃{�����[��</param>
    /// <param name="endVolume">�ŏI�I�ȃ{�����[��</param>
    /// <param name="fadeTime">�t�F�[�h�̎���</param>
    private void OnPlaySoundWithFadeIn(AudioSource source, AudioInfomation audio, float startVolume, float endVolume, float fadeTime)
    {
        source.clip = audio.Clip;
        source.volume = startVolume;
        source.loop = audio.Loop;
        source.time = audio.Ofset;

        source.Play();
        StartCoroutine(FadeMoveSound(audio, fadeTime, endVolume));
        StartCoroutine(FinishSoundProcess(source));
    }

    /// <summary>
    /// �T�E���h�𒼐ڎ~�߂�
    /// </summary>
    /// <param name="source">�炷�\�[�X</param>
    private void OnStopSound(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.volume = 0f;
        source.loop = false;
        source.time = 0f;
    }

    /// <summary>
    /// �T�E���h�𒼐ڎ~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="source">�~�߂�\�[�X</param>
    /// <param name="audio">�~�߂鉹�̏��</param>
    /// <param name="endVolume">�ŏI�I�ȃ{�����[��</param>
    /// <param name="fadeTime">�t�F�[�h�̎���</param>
    private IEnumerator OnStopSoundWithFadeOut(AudioSource source, AudioInfomation audio, float endVolume, float fadeTime)
    {
        yield return StartCoroutine(FadeMoveSound(audio, fadeTime, endVolume));

        source.Stop();
        source.clip = null;
        source.volume = 0f;
        source.loop = false;
        source.time = 0f;
    }

    /// <summary>
    /// �T�E���h�̈ꎞ��~
    /// </summary>
    /// <param name="source">�����������\�[�X</param>
    private void OnPauseSound(AudioSource source)
    {
        source.Pause();
    }

    /// <summary>
    /// �T�E���h�̈ꎞ��~
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="source">�����������\�[�X</param>
    /// <param name="endVolume">�ŏI�I�ȃ{�����[��</param>
    /// <param name="fadeTime">�t�F�[�h�̎���</param>
    private IEnumerator OnPauseSoundWithFadeOut(AudioSource source, float endVolume, float fadeTime)
    {
        yield return StartCoroutine(FadeMoveSound(source, fadeTime, endVolume));
        source.Pause();
    }

    /// <summary>
    /// �T�E���h�̃^�C�v�ŏ������Ă���AudioSource�̃��X�g�����
    /// </summary>
    /// <param name="type">���ׂ�������AudioType</param>
    /// <returns></returns>
    private List<AudioSource> SearchSourceListByAudioType(AudioType type)
    {
        List<AudioSource> sources = new List<AudioSource>();

        // �w�肳�ꂽ���̃^�C�v�ɂ���ă\�[�X�̃��X�g������
        switch (type)
        {
            case AudioType.BGM:
                sources = _bgmSource;
                break;
            case AudioType.SE:
                sources = _seSource;
                break;
            case AudioType.VOICE:
                sources = _voiceSource;
                break;
            case AudioType.OTHERS:
                sources = _othersSource;
                break;
            default:
                break;
        }

        return sources;
    }

    /// <summary>
    /// ���ݍĐ����ł͂Ȃ�AudioSource�����X�g�̒����猟��
    /// ���ׂĖ��܂��Ă����ꍇ�A�V�������̂����
    /// </summary>
    /// <param name="sources">����������AudioSource�̃��X�g</param>
    /// <returns></returns>
    private AudioSource SearchEmptySource(List<AudioSource> sources)
    {
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
                return source;
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        sources.Add(newSource);

        return newSource;
    }

    /// <summary>
    /// �w�肵��AudioClip���Đ����Ă���AudioSource�����X�g�̒����猟��
    /// </summary>
    /// <param name="sources">����������AudioSource�̃��X�g</param>
    /// <param name="clip">����������AudioClip</param>
    /// <returns></returns>
    private AudioSource SearchSourceByClip(List<AudioSource> sources, AudioClip clip)
    {
        foreach (AudioSource source in sources)
        {
            if (source.clip == clip)
                return source;
        }

        return null;
    }

    /// <summary>
    /// �w�肵���T�E���h���t�F�[�h���ĉ��ʂ��ړ�
    /// </summary>
    /// <param name="audio">�t�F�[�h��������</param>
    /// <param name="fadeOutSec">�t�F�[�h����</param>
    /// <param name="volume">�t�F�[�h��̉���</param>
    /// <returns></returns>
    private IEnumerator FadeMoveSound(AudioInfomation audio, float fadeOutSec, float volume)
    {
        if (audio == null) yield break;

        // �����Ă���AudioSource������
        List<AudioSource> sources = SearchSourceListByAudioType(audio.Type);
        if (sources == null) yield break;
        AudioSource source = SearchSourceByClip(sources, audio.Clip);
        if (source == null) yield break;

        // �ω���̉��ʂ����ƂƓ����A�K�v�Ȃ��̂�
        if (volume == source.volume) yield break;

        if (fadeOutSec > 0f)
        {
            float timeCnt = 0;
            float startVolume = source.volume;
            float endVolume = volume;
            while (timeCnt <= fadeOutSec)
            {
                timeCnt += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, endVolume, timeCnt / fadeOutSec);
                yield return null;
            }
        }

        source.volume = volume;
    }

    /// <summary>
    /// �w�肵���T�E���h���t�F�[�h���ĉ��ʂ��ړ�
    /// </summary>
    /// <param name="audio">�t�F�[�h��������</param>
    /// <param name="fadeOutSec">�t�F�[�h����</param>
    /// <param name="volume">�t�F�[�h��̉���</param>
    /// <returns></returns>
    private IEnumerator FadeMoveSound(AudioSource source, float fadeOutSec, float volume)
    {
        if (source == null) yield break;

        // �ω���̉��ʂ����ƂƓ����A�K�v�Ȃ��̂�
        if (volume == source.volume) yield break;

        if (fadeOutSec > 0f)
        {
            float timeCnt = 0;
            float startVolume = source.volume;
            float endVolume = volume;
            while (timeCnt <= fadeOutSec)
            {
                timeCnt += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, endVolume, timeCnt / fadeOutSec);
                yield return null;
            }
        }

        source.volume = volume;
    }
    #endregion

    #region �p�����[�^�֌W

    // AudioMixer�`�����l���̍Œ�l
    public const float MIXER_MIN_VALUE = -80.0f;

    /// <summary>
    /// BGM�̃{�����[����ݒ�
    /// </summary>
    /// <param name="volume">�V�����{�����[��</param>
    public void SetBGMVolume(float volume)
    {
        _mixer.SetFloat("BGM", volume);
    }
    /// <summary>
    /// BGM�̃{�����[��
    /// </summary>
    public float GetBGMVolume
    {
        get
        {
            float volume = 0f;
            if (_mixer.GetFloat("BGM", out volume))
                return volume;
            else
                return -1f;
        }
    }

    /// <summary>
    /// SE�̃{�����[����ݒ�
    /// </summary>
    /// <param name="volume">�V�����{�����[��</param>
    public void SetSEVolume(float volume)
    {
        _mixer.SetFloat("SE", volume);
    }
    /// <summary>
    /// SE�̃{�����[��
    /// </summary>
    public float GetSEVolume
    {
        get
        {
            float volume = 0f;
            if (_mixer.GetFloat("SE", out volume))
                return volume;
            else
                return -1f;
        }
    }

    /// <summary>
    /// �{�C�X�̃{�����[����ݒ�
    /// </summary>
    /// <param name="volume">�V�����{�����[��</param>
    public void SetVoiceVolume(float volume)
    {
        _mixer.SetFloat("Voice", volume);
    }
    /// <summary>
    /// �{�C�X�̃{�����[��
    /// </summary>
    public float GetVoiceVolume
    {
        get
        {
            float volume = 0f;
            if (_mixer.GetFloat("Voice", out volume))
                return volume;
            else
                return -1f;
        }
    }

    /// <summary>
    /// ���̑��̃{�����[����ݒ�
    /// </summary>
    /// <param name="volume">�V�����{�����[��</param>
    public void SetOthersVolume(float volume)
    {
        _mixer.SetFloat("Others", volume);
    }
    /// <summary>
    /// ���̑��̃{�����[��
    /// </summary>
    public float GetOthersVolume
    {
        get
        {
            float volume = 0f;
            if (_mixer.GetFloat("Others", out volume))
                return volume;
            else
                return -1f;
        }
    }

    /// <summary>
    /// BGM�~���[�g����
    /// </summary>
    public void OnBGM()
    {
        foreach (AudioSource source in _bgmSource)
        {
            source.mute = false;
        }
    }
    /// <summary>
    /// BGM�~���[�g
    /// </summary>
    public void OffBGM()
    {
        foreach (AudioSource source in _bgmSource)
        {
            source.mute = true;
        }
    }

    /// <summary>
    /// SE�~���[�g����
    /// </summary>
    public void OnSE()
    {
        foreach (AudioSource source in _seSource)
        {
            source.mute = false;
        }
    }
    /// <summary>
    /// SE�~���[�g
    /// </summary>
    public void OffSE()
    {
        foreach (AudioSource source in _seSource)
        {
            source.mute = true;
        }
    }

    /// <summary>
    /// Voice�~���[�g����
    /// </summary>
    public void OnVoice()
    {
        foreach (AudioSource source in _voiceSource)
        {
            source.mute = false;
        }
    }
    /// <summary>
    /// Voice�~���[�g
    /// </summary>
    public void OffVoice()
    {
        foreach (AudioSource source in _voiceSource)
        {
            source.mute = true;
        }
    }

    /// <summary>
    /// ���̑��~���[�g����
    /// </summary>
    public void OnOthers()
    {
        foreach (AudioSource source in _othersSource)
        {
            source.mute = false;
        }
    }
    /// <summary>
    /// ���̑��~���[�g
    /// </summary>
    public void OffOthers()
    {
        foreach (AudioSource source in _othersSource)
        {
            source.mute = true;
        }
    }
    #endregion

    #region BGM����
    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM��炷
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void PlayBGM(int index)   // ���ʉ���炷(�P��)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_bgmSource);
        if (source == null) return;

        OnPlaySound(source, audio);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM��炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void PlayBGMWithFadeIn(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_bgmSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, 0f, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM��炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void PlayBGMWithFadeIn(int index, float startVolume, float endVolume, float fadeInSec)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_bgmSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, startVolume, endVolume, fadeInSec);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XBGM���~�߂�
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void StopBGM(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        OnStopSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XBGM���~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void StopBGMWithFadeOut(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XBGM���~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void StopBGMWithFadeOut(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void PauseBGM(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        OnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void PauseBGMWithFadeOut(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void PauseBGMWithFadeOut(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, endVolume, fadeOutTime));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void UnPauseBGM(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void UnPauseBGMWithFadeIn(int index)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��BGM���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">BGM�̃C���f�b�N�X</param>
    public void UnPauseBGMWithFadeIn(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, endVolume, fadeOutTime);
    }

    /// <summary>
    /// �w�肵��BGM�̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX������BGM�̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeBGMByIndex(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, fadeOutSec, endVolume));
    }

    /// <summary>
    /// �w�肵��BGM�̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX������BGM�̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeBGMByIndex(int index, float endVolume)
    {
        if (index < 0 || _bgmList.Count <= index) return;
        AudioInfomation audio = _bgmList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_bgmSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, _defaultFadeRate, endVolume));
    }
    #endregion

    #region SE����
    /// <summary>
    /// �w�肵���C���f�b�N�X��SE��炷
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void PlaySE(int index)   // ���ʉ���炷(�P��)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_seSource);
        if (source == null) return;

        OnPlaySound(source, audio);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE��炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void PlaySEWithFadeIn(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_seSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, 0f, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE��炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void PlaySEWithFadeIn(int index, float startVolume, float endVolume, float fadeInSec)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_seSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, startVolume, endVolume, fadeInSec);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XSE���~�߂�
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void StopSE(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        OnStopSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XSE���~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void StopSEWithFadeOut(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XSE���~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void StopSEWithFadeOut(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void PauseSE(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        OnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void PauseSEWithFadeOut(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void PauseSEWithFadeOut(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, endVolume, fadeOutTime));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void UnPauseSE(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void UnPauseSEWithFadeIn(int index)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��SE���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">SE�̃C���f�b�N�X</param>
    public void UnPauseSEWithFadeIn(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, endVolume, fadeOutTime);
    }

    /// <summary>
    /// �w�肵��SE�̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX������SE�̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeSEByIndex(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, fadeOutSec, endVolume));
    }

    /// <summary>
    /// �w�肵��SE�̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX������SE�̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeSEByIndex(int index, float endVolume)
    {
        if (index < 0 || _seList.Count <= index) return;
        AudioInfomation audio = _seList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_seSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, _defaultFadeRate, endVolume));
    }
    #endregion

    #region VOICE����
    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice��炷
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void PlayVoice(int index)   // ���ʉ���炷(�P��)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_voiceSource);
        if (source == null) return;

        OnPlaySound(source, audio);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice��炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void PlayVoiceWithFadeIn(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_voiceSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, 0f, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice��炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void PlayVoiceWithFadeIn(int index, float startVolume, float endVolume, float fadeInSec)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_voiceSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, startVolume, endVolume, fadeInSec);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XVoice���~�߂�
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void StopVoice(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        OnStopSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XVoice���~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void StopVoiceWithFadeOut(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�XVoice���~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void StopVoiceWithFadeOut(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void PauseVoice(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        OnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void PauseVoiceWithFadeOut(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void PauseVoiceWithFadeOut(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, endVolume, fadeOutTime));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void UnPauseVoice(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void UnPauseVoiceWithFadeIn(int index)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X��Voice���ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">Voice�̃C���f�b�N�X</param>
    public void UnPauseVoiceWithFadeIn(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, endVolume, fadeOutTime);
    }

    /// <summary>
    /// �w�肵��Voice�̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX������Voice�̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeVoiceByIndex(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, fadeOutSec, endVolume));
    }

    /// <summary>
    /// �w�肵��Voice�̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX������Voice�̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeVoiceByIndex(int index, float endVolume)
    {
        if (index < 0 || _voiceList.Count <= index) return;
        AudioInfomation audio = _voiceList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_voiceSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, _defaultFadeRate, endVolume));
    }
    #endregion

    #region OTHERS����
    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉���炷
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void PlayOthers(int index)   // ���ʉ���炷(�P��)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_othersSource);
        if (source == null) return;

        OnPlaySound(source, audio);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉���炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void PlayOthersWithFadeIn(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_othersSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, 0f, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉���炷
    /// �t�F�[�h�C������
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void PlayOthersWithFadeIn(int index, float startVolume, float endVolume, float fadeInSec)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchEmptySource(_othersSource);
        if (source == null) return;

        OnPlaySoundWithFadeIn(source, audio, startVolume, endVolume, fadeInSec);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X���̑��̉����~�߂�
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void StopOthers(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        OnStopSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X���̑��̉����~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void StopOthersWithFadeOut(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X���̑��̉����~�߂�
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void StopOthersWithFadeOut(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnStopSoundWithFadeOut(source, audio, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉����ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void PauseOthers(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        OnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉����ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void PauseOthersWithFadeOut(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, 0f, _defaultFadeRate));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉����ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void PauseOthersWithFadeOut(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(OnPauseSoundWithFadeOut(source, endVolume, fadeOutTime));
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉����ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void UnPauseOthers(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSound(source);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉����ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void UnPauseOthersWithFadeIn(int index)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, audio.Volume, _defaultFadeRate);
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̂��̑��̉����ꎞ��~����
    /// �t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="index">���̑��̉��̃C���f�b�N�X</param>
    public void UnPauseOthersWithFadeIn(int index, float endVolume, float fadeOutTime)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        OnUnPauseSoundWithFadeIn(source, endVolume, fadeOutTime);
    }

    /// <summary>
    /// �w�肵�����̑��̉��̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX���������̑��̉��̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeOthersByIndex(int index, float endVolume, float fadeOutSec)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, fadeOutSec, endVolume));
    }

    /// <summary>
    /// �w�肵�����̑��̉��̃{�����[�����t�F�[�h�ŕύX����
    /// </summary>
    /// <param name="index">�ύX���������̑��̉��̃C���f�b�N�X</param>
    /// <param name="endVolume">�ύX��̃{�����[��</param>
    /// <param name="fadeOutSec">�t�F�[�h�̎���</param>
    public void FadeMoveVolumeOthersByIndex(int index, float endVolume)
    {
        if (index < 0 || _othersList.Count <= index) return;
        AudioInfomation audio = _othersList[index];
        if (audio == null) return;
        AudioSource source = SearchSourceByClip(_othersSource, audio.Clip);
        if (source == null) return;

        StartCoroutine(FadeMoveSound(audio, _defaultFadeRate, endVolume));
    }
    #endregion



    #region �V���O���g���p�^�[��
    public static SoundManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
