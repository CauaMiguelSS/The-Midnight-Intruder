using System.Collections;
using UnityEngine;
using TMPro;

public class ApresentacaoDoMapa : MonoBehaviour
{
    public TextMeshProUGUI texto;
    public float duracao = 2f; 
    public float tempoVisivel = 6f; 

    void Start()
    {
        StartCoroutine(MostrarTextoComFade("O Campo"));
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