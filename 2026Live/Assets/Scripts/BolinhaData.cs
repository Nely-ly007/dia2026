using UnityEngine;

[CreateAssetMenu(fileName = "BolinhaData", menuName = "Sumo/Bolinha Data")]
public class BolinhaData : ScriptableObject
{
    [Header("Identificação")]
    public string nomeBolinha;

    [Header("Stats")]
    public float velocidade = 6f;
    public float forcaEmpurrao = 10f;
    public float massaBase = 1f;
    public float tamanho = 1f;
}