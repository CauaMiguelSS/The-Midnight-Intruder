using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; 
// ... outros usings, se aplicável

public class SceneTransition : MonoBehaviour
{
    // --- Campos existentes ---
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private string nextSceneName = "MainGameScene"; 

    // --- Novo Campo de Áudio ---
    [Header("Configuração de Áudio")]
    [Tooltip("Arraste o componente AudioSource com a música de fundo.")]
    [SerializeField] private AudioSource backgroundMusic;
    
    // O volume inicial que a música deve ter (DEVE ser o mesmo volume que o AudioSource está tocando)
    [Tooltip("O volume inicial/normal da música (Ex: 0.5f).")]
    [SerializeField] private float initialVolume = 0.5f; 

    // Este método é chamado pelo botão "Começar"
    public void StartGame()
    {
        // Inicia a Coroutine que fará o fade visual E o fade de áudio
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        float timer = 0f;

        // 1. Preparação (Opcional, mas útil para garantir o volume de início)
        if (backgroundMusic != null)
        {
            // Garante que o script usa o volume inicial que você definiu no Inspector
            backgroundMusic.volume = initialVolume;
        }

        // 2. Loop de Fade (Fade-In Visual e Fade-Out Áudio)
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Calcula a proporção de 0 a 1
            float t = Mathf.Clamp01(timer / fadeDuration);

            // APLICA FADE VISUAL: Fade-In (0 -> 1)
            // O painel preto fica gradualmente opaco
            float imageAlpha = t;
            fadeImage.color = new Color(0f, 0f, 0f, imageAlpha);

            // APLICA FADE DE ÁUDIO: Fade-Out (initialVolume -> 0)
            if (backgroundMusic != null)
            {
                // Interpola o volume do valor inicial até zero, usando 't'
                float currentVolume = Mathf.Lerp(initialVolume, 0f, t);
                backgroundMusic.volume = currentVolume;
            }

            yield return null; 
        }

        // 3. Garante que os valores finais sejam aplicados
        fadeImage.color = new Color(0f, 0f, 0f, 1f); 
        if (backgroundMusic != null)
        {
            // Garante volume zero e para a música
            backgroundMusic.volume = 0f;
            backgroundMusic.Stop(); 
        }

        // 4. Carrega a próxima cena
        SceneManager.LoadScene(nextSceneName);
    }
}