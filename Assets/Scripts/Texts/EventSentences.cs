using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
