using UnityEngine;

public class MultiLightBlink : MonoBehaviour
{
    [Header("Lights That Will Blink")]
    public Light[] lightsToBlink;   // Arraste aqui a Point Light e outras luzes que devem piscar

    [Header("Blink Settings")]
    public float minInterval = 0.1f;  // tempo mínimo entre piscadas
    public float maxInterval = 0.4f;  // tempo máximo entre piscadas
    public float offDuration = 0.1f;  // quanto tempo fica apagada

    private void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    private System.Collections.IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Espera aleatória antes de piscar
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Desliga todas
            foreach (var l in lightsToBlink)
                l.enabled = false;

            yield return new WaitForSeconds(offDuration);

            // Liga todas
            foreach (var l in lightsToBlink)
                l.enabled = true;
        }
    }
}