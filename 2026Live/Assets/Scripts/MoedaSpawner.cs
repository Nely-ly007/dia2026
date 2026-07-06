using UnityEngine;

public class MoedaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject moedaPrefab;
    [SerializeField] private float intervaloMin = 3f;
    [SerializeField] private float intervaloMax = 7f;
    [SerializeField] private Vector2 areaSpawn = new Vector2(4f, 4f); // X e Z
    [SerializeField] private int maxMoedasNaCena = 5;

    private int _moedasAtivas = 0;

    void Start() => AgendarProximoSpawn();

    void AgendarProximoSpawn()
    {
        float tempo = Random.Range(intervaloMin, intervaloMax);
        Invoke(nameof(SpawnarMoeda), tempo);
    }

    void SpawnarMoeda()
    {
        if (_moedasAtivas < maxMoedasNaCena)
        {
            Vector3 pos = new Vector3(
                Random.Range(-areaSpawn.x, areaSpawn.x),
                0.5f,
                Random.Range(-areaSpawn.y, areaSpawn.y)
            );
            Instantiate(moedaPrefab, pos, Quaternion.identity);
            _moedasAtivas++;
        }
        AgendarProximoSpawn();
    }

    public void MoedaColetada() => _moedasAtivas--;
}