using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioConfigManager : MonoBehaviour
{
    [SerializeField, Header("音量設定スライダー")]
    private Slider _configSlider = null;

    [SerializeField, Header("音量表示テキスト")]
    private TextMeshProUGUI _configText = null;

    [SerializeField, Header("サウンドタイプ")]
    private AudioType _audioType = AudioType.BGM;

    /// <summary>
    /// スライダーの値を更新したときの処理
    /// </summary>
    public void UpdateValue()
    {
        _configText.text = $"{_configSlider.value.ToString("F0")}%";
        switch (_audioType)
        {
            case AudioType.BGM:
                SoundManager.Instance?.SetBGMVolume(SoundManager.MIXER_MIN_VALUE + _configSlider.value);
                break;
            case AudioType.SE:
                SoundManager.Instance?.SetSEVolume(SoundManager.MIXER_MIN_VALUE + _configSlider.value);
                break;
            case AudioType.VOICE:
                SoundManager.Instance?.SetVoiceVolume(SoundManager.MIXER_MIN_VALUE + _configSlider.value);
                break;
            case AudioType.OTHERS:
                SoundManager.Instance?.SetOthersVolume(SoundManager.MIXER_MIN_VALUE + _configSlider.value);
                break;
        }
    }
}
