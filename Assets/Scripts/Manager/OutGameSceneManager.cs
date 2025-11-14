using UnityEngine;

public class OutGameSceneManager : MonoBehaviour
{
    [SerializeField, Header("移動するシーン")]
    private string _moevScene = "MainScene";

    /// <summary>
    /// シーン移動処理
    /// </summary>
    public void OnMoveScene()
    {
        SoundManager.Instance.PlaySE(0);
        FadeManager.Instance.CallScene(_moevScene);
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void OnFinishGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                    Application.Quit();//ゲームプレイ終了
#endif
    }

    void Start()
    {
        FadeManager.Instance.FadeInDisplay();
    }
}
