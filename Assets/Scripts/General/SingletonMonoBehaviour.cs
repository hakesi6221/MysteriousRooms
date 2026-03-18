using System;
using UnityEngine;

/// <summary>
/// MonoBehaviourのクラスを継承したクラスで、汎用的にシングルトンパターンを実装するための基底クラス
/// 継承したら、そのクラスはMonoBehaviourを継承したシングルトンパターンになる
/// 継承先でAwakeを使う場合、その最初にこのクラスのAwakeを呼ぶこと
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// このオブジェクトをDontDestroyOnRoadにするかどうか
    /// </summary>
    protected abstract bool dontDestroyOnLoad { get; }

    private static T instance;

    /// <summary>
    /// インスタンスを取得
    /// インスタンスがない場合エラーを出力
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (!instance)
                {
                    Debug.LogWarning(t + " is nothing.");
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// すでにインスタンスがある場合、これを削除、dontDestroyOnLoadがtrueの場合設定
    /// </summary>
    protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
