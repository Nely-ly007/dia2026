using UnityEngine;

public class Moeda : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var bolinha = other.GetComponent<Bolinha>();
        if (bolinha == null) return;

        bolinha.ColetarMoeda();
        FindObjectOfType<MoedaSpawner>()?.MoedaColetada();
        Destroy(gameObject);
    }
}