using System.Collections;
using UnityEngine;
using TMPro;

public class TextoComFadeManager : MonoBehaviour
{
    public static TextoComFadeManager Instance;

    [Header("Referências")]
    public TextMeshProUGUI texto;

    [Header("Configurações")]
    public float duracao = 2f;
    public float tempoVisivel = 6f;

    private Coroutine rotinaAtual;

    void Awake()
    {
        Instance = this;
        if (texto != null)
        {
            Color c = texto.color;
            c.a = 0f;           // força invisível no começo
            texto.color = c;
            texto.gameObject.SetActive(false); // opcional: começa desativado
        }
    }

    public void MostrarTexto(string mensagem)
    {
        if (rotinaAtual != null)
            StopCoroutine(rotinaAtual);

        rotinaAtual = StartCoroutine(MostrarTextoComFade(mensagem));
    }

    IEnumerator MostrarTextoComFade(string mensagem)
    {
        texto.text = mensagem;
        texto.gameObject.SetActive(true);

        Color cor = texto.color;

        // Fade in
        for (float t = 0; t < duracao; t += Time.deltaTime)
        {
            cor.a = Mathf.Lerp(0, 1, t / duracao);
            texto.color = cor;
            yield return null;
        }

        cor.a = 1;
        texto.color = cor;

        yield return new WaitForSeconds(tempoVisivel);

        // Fade out
        for (float t = 0; t < duracao; t += Time.deltaTime)
        {
            cor.a = Mathf.Lerp(1, 0, t / duracao);
            texto.color = cor;
            yield return null;
        }

        cor.a = 0;
        texto.color = cor;

        texto.gameObject.SetActive(false);
    }


}
