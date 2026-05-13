using UnityEngine;

/// <summary>
/// CoinRotator
/// Script simples para fazer a moeda girar continuamente,
/// dando feedback visual ao jogador de que é coletável.
/// Adicione ao Prefab da Coin.
/// </summary>
public class CoinRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // graus por segundo

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
