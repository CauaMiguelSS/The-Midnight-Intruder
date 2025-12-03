using UnityEngine;

public class FlashLightFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; 
    [SerializeField] private float followSpeed = 10f;   

    void LateUpdate()
    {
        if (cameraTransform == null) return;

      
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            cameraTransform.rotation,
            followSpeed * Time.deltaTime
        );
    }
}