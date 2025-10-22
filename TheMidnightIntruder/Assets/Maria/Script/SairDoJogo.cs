using UnityEngine;

public class SairDoJogo : MonoBehaviour
{
    public void FecharJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}
