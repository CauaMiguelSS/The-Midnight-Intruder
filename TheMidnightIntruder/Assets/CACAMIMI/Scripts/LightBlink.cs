using UnityEngine;

public class LightBlink : MonoBehaviour
{
    [SerializeField] private Light pointLight;
    [SerializeField] private float minTime = 0.05f;
    [SerializeField] private float maxTime = 0.3f;

    private void Start()
    {
        if (pointLight == null)
            pointLight = GetComponent<Light>();

        StartCoroutine(Blink());
    }

    private System.Collections.IEnumerator Blink()
    {
        while (true)
        {
            pointLight.enabled = !pointLight.enabled;
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}

