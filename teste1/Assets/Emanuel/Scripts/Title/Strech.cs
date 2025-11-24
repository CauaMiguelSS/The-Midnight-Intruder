using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Strech
   : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform botaoTransform;
    public float tempoAnimacao = 0.1f;
    public Vector3 escalaEsticada = new Vector3(1.1f, 0.9f, 1f);

    private Vector3 escalaOriginal;

    void Start()
    {
        if (botaoTransform == null)
            botaoTransform = GetComponent<RectTransform>();

        escalaOriginal = botaoTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarBotao(escalaEsticada));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarBotao(escalaOriginal));
    }

    IEnumerator AnimarBotao(Vector3 alvo)
    {
        Vector3 inicio = botaoTransform.localScale;
        float tempo = 0;

        while (tempo < tempoAnimacao)
        {
            tempo += Time.deltaTime;
            botaoTransform.localScale = Vector3.Lerp(inicio, alvo, tempo / tempoAnimacao);
            yield return null;
        }

        botaoTransform.localScale = alvo;
    }
}
