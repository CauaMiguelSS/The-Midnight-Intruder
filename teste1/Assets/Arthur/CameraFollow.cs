using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Referência ao jogador
    public Vector3 offset;     // Distância da câmera em relação ao jogador
    public float smoothSpeed = 5f; // Suavidade do movimento

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
