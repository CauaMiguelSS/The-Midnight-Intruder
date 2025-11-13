using UnityEngine;

public class FlashLightFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // arrasta tua câmera aqui no inspetor
    [SerializeField] private float followSpeed = 10f;   // velocidade de acompanhamento

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Suaviza o movimento da lanterna pra acompanhar a câmera
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            cameraTransform.rotation,
            followSpeed * Time.deltaTime
        );
    }
}

