using System;
using System.Collections.Generic;

public class FlagDictionary<T> where T : Enum
{
    // フラグのディクショナリー
    private Dictionary<T, bool> _flags = new Dictionary<T, bool>();

    /// <summary>
    /// 渡されたEnumの数要素がある、EnumをKey,boolをvalueとしたDictionaryのクラス
    /// </summary>
    public FlagDictionary()
    {
        _flags = new Dictionary<T, bool>();
        foreach (T flag in Enum.GetValues(typeof(T)))
        {
            _flags[flag] = false;
        }
    }

    /// <summary>
    /// 指定されたEnumキーのbool値を取得する
    /// </summary>
    /// <param name="flag">確認したいEnum</param>
    /// <returns></returns>
    public bool GetFlagValue(T flag)
    {
        return _flags.TryGetValue(flag, out bool value) && value;
    }

    /// <summary>
    /// 指定されたEnumキーのbool値を設定する
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="value"></param>
    public void SetFlagValue(T flag, bool value)
    {
        _flags[flag] = value;
    }

    /// <summary>
    /// すべてのboolをfalseにする
    /// </summary>
    public void ResetAllFlags()
    {
        foreach (T flag in Enum.GetValues(typeof(T)))
        {
            _flags[flag] = false;
        }
    }
}

/// <summary>
/// フラグ管理enum
/// </summary>
public enum Flags
{
    // Stage01
    S01_RemoteTV,
    S01_ScrewDriver,
    S01_Key01,
    S01_Key02,
    S01_TurnMonitor,


}

public class FlagManager : SingletonMonoBehaviour<FlagManager>
{
    protected override bool dontDestroyOnLoad => false;

    /// <summary>
    /// フラグ管理クラス
    /// </summary>
    /// <typeparam name="Flags"></typeparam>
    /// <returns></returns>
    public FlagDictionary<Flags> Flags { get; private set; } = new FlagDictionary<Flags>();

}
