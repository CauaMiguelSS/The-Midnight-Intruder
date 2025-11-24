using UnityEngine;

public class ResetCaixas : MonoBehaviour
{
    public GameObject[] caixas;

    
    private Vector3[] posicoesOriginais;

    void Start()
    {
        posicoesOriginais = new Vector3[caixas.Length];
        for (int i = 0; i < caixas.Length; i++)
        {
            posicoesOriginais[i] = caixas[i].transform.position;
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Caixa") || collision.CompareTag("Player"))
        {
            ResetarCaixas();
        }
    }

    void ResetarCaixas()
    {
        for (int i = 0; i < caixas.Length; i++)
        {
            caixas[i].transform.position = posicoesOriginais[i];
        }
    }
}
