using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventSentenceをリストとして持ち、一つのテキストイベントの情報として保存するためのScriptableObjectクラス
/// </summary>
[CreateAssetMenu(fileName = "EventText", menuName = "ScriptableObjects/CreateEventText")]
public class EventSentences : ScriptableObject
{
    [SerializeField, Header("イベントで表示したいテキスト情報")]
    private List<EventSentence> _sentences = new List<EventSentence>();

    /// <summary>
    /// イベントで表示したいテキスト情報
    /// </summary>
    public List<EventSentence> Sentences => new List<EventSentence>(_sentences);
}

/// <summary>
/// 何かを調べた時などに表示されるテキストのじょうっ法を持ったクラス
/// ここから情報を拾って、テキストを表示する
/// </summary>
[System.Serializable]
public class EventSentence
{
    [SerializeField, Header("テキストの表示間隔：秒")]
    private float _textDuration = 0.2f;

    [SerializeField, Header("テキストのフォントサイズ")]
    private float _textFontSize = 50f;

    [SerializeField, Header("表示する文章"), TextArea(6, 10)]
    private string _sentence = string.Empty;

    /// <summary>
    /// テキストの表示間隔：秒
    /// </summary>
    public float TextDuration => _textDuration;

    /// <summary>
    /// テキストのフォントサイズ
    /// </summary>
    public float TextFontSize => _textFontSize;

    /// <summary>
    /// 表示する文章
    /// </summary>
    public string Sentence => _sentence;
}
