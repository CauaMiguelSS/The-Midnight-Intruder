using UnityEngine;

public class DoorAnimatorTest : MonoBehaviour
{
    public Animator doorAnimator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Aperte T para testar
        {
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Open");
                Debug.Log("Trigger 'Open' enviado!");
            }
            else
            {
                Debug.LogWarning("doorAnimator não está atribuído!");
            }
        }
    }
}