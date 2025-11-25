using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    public bool chave1;
    public bool chave2;
    public bool chave3;
    public bool martelo;
    public bool lanterna;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(string itemName)
    {
        switch (itemName)
        {
            case "Chave1": chave1 = true; break;
            case "Chave2": chave2 = true; break;
            case "Chave3": chave3 = true; break;
            case "Martelo": martelo = true; break;
            case "Lanterna":lanterna = true;break;

        }
    }

    public bool HasItem(string itemName)
    {
        return itemName switch
        {
            "Chave1" => chave1,
            "Chave2" => chave2,
            "Chave3" => chave3,
            "Martelo" => martelo,
            "Lanterna" => lanterna,
            _ => false
        };
    }
}

