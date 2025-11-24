using UnityEngine;
using UnityEngine.SceneManagement;

public class TankChase : MonoBehaviour
{
    public Transform player;
    public float tankSpeed = 3f;
    public float playerSpeed = 5f;
    public float acceleration = 0.5f;
    public float catchDistance = 1.2f;

    void Update()
    {
        Debug.Log("Distância: " + Vector3.Distance(transform.position, player.position));

        if (player == null) return;

        tankSpeed += acceleration * Time.deltaTime;
        playerSpeed += acceleration * Time.deltaTime;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * tankSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.position) < catchDistance)
        {
            Debug.Log("PEGOU!");

            SceneManager.LoadScene("CenaFinal");
        }
    }
}
