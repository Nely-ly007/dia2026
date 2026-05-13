using UnityEngine;

/// <summary>
/// CoinSpawner
/// Espalha prefabs de moeda pela fase em posições configuráveis no Inspector.
/// Alternativa: posicione as moedas manualmente na cena Gameplay.
///
/// ─── SETUP NA UNITY ───────────────────────────────────────
/// 1. Crie um Prefab "Coin":
///      - Mesh: Cylinder (escale para (0.5, 0.05, 0.5)) ou modelo 3D importado.
///      - Adicione Collider (ex: CapsuleCollider) com isTrigger = TRUE.
///      - Adicione uma tag "Coin" (crie a tag em Edit → Tags & Layers).
///      - Opcional: adicione rotação animada (veja CoinRotator abaixo).
/// 2. Adicione este script a um GameObject vazio "CoinSpawner" na cena.
/// 3. Arraste o Prefab para o campo CoinPrefab no Inspector.
/// 4. Defina as posições em SpawnPoints, ou marque UseRandomPositions.
/// ──────────────────────────────────────────────────────────
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    [Header("Prefab da Moeda")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Posições Manuais (arraste ou edite no Inspector)")]
    [SerializeField] private Vector3[] spawnPoints = new Vector3[]
    {
        new Vector3( 3f, 0.5f,  3f),
        new Vector3(-3f, 0.5f,  3f),
        new Vector3( 3f, 0.5f, -3f),
        new Vector3(-3f, 0.5f, -3f),
        new Vector3( 5f, 0.5f,  0f),
        new Vector3(-5f, 0.5f,  0f),
        new Vector3( 0f, 0.5f,  5f),
        new Vector3( 0f, 0.5f, -5f),
    };

    [Header("Posições Aleatórias (ignora SpawnPoints se marcado)")]
    [SerializeField] private bool useRandomPositions = false;
    [SerializeField] private int  randomCoinCount    = 10;
    [SerializeField] private float spawnAreaRadius   = 8f;
    [SerializeField] private float coinHeight        = 0.5f;

    // -------------------------------------------------------
    // UNITY CALLBACKS
    // -------------------------------------------------------
    private void Start()
    {
        SpawnCoins();
    }

    // -------------------------------------------------------
    // SPAWN
    // -------------------------------------------------------
    private void SpawnCoins()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("[CoinSpawner] CoinPrefab não atribuído!");
            return;
        }

        if (useRandomPositions)
        {
            SpawnRandom();
        }
        else
        {
            SpawnAtFixedPoints();
        }
    }

    private void SpawnAtFixedPoints()
    {
        foreach (var pos in spawnPoints)
        {
            Instantiate(coinPrefab, pos, Quaternion.Euler(90f, 0f, 0f));
        }
        Debug.Log($"[CoinSpawner] {spawnPoints.Length} moedas posicionadas.");
    }

    private void SpawnRandom()
    {
        for (int i = 0; i < randomCoinCount; i++)
        {
            Vector2 cube = Random.insideUnitCircle * spawnAreaRadius;
            Vector3 pos    = new Vector3(cube.x, coinHeight, cube.y);
            Instantiate(coinPrefab, pos, Quaternion.Euler(90f, 0f, 0f));
        }
        Debug.Log($"[CoinSpawner] {randomCoinCount} moedas aleatórias posicionadas.");
    }
}
