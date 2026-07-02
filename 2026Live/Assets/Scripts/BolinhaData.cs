using UnityEngine;

[CreateAssetMenu(fileName = "BolinhaData", menuName = "Sumo/Bolinha Data")]
public class BolinhaData : ScriptableObject
{
    [Header("Identificação")]
    public string nomeBolinha;
    public Sprite icone;
    public Color corJogador1;
    public Color corJogador2;

    [Header("Status")]
    public float tamanho = 1f;          // escala do GameObject
    public float velocidade = 6f;       // força de movimento
    public float massaBase = 1f;        // Rigidbody.mass base
    public float forcaEmpurrao = 10f;   // multiplicador do botão de ação

    [Header("Efeito das Moedas")]
    public float massaPorMoeda = 0.3f;  // quanto cada moeda aumenta a massa
    public float forcaPorMoeda = 1.5f;  // quanto cada moeda aumenta o empurrão
    public float lentidaoPorMoeda = 0.05f; // quanto cada moeda reduz velocidade
}