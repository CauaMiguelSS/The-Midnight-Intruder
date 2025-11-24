using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    // Chame esta função a partir do botão no Inspector
    public void ExitGame()
    {
        Debug.Log("Saindo do jogo..."); // só aparece no Editor
        Application.Quit();

        // ⚠️ Importante:
        // Isso só fecha o jogo compilado (EXE, APK, etc).
        // No Editor da Unity, ele não fecha o Play Mode.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
