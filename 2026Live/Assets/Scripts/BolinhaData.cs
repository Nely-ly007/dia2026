using UnityEngine;

// Agora é usado só para definir a aparência (cor) da bolinha.
// Crie 5 assets diferentes (um por opção de cor) via botão direito > Create > Sumo > Bolinha Data.
[CreateAssetMenu(fileName = "BolinhaData", menuName = "Sumo/Bolinha Data")]
public class BolinhaData : ScriptableObject
{
    [Header("Identificação")]
    public string nomeBolinha;

    [Header("Visual")]
    public Color corPrimaria;
    public Color corSecundaria;
}