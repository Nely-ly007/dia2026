using UnityEngine;
using UnityEngine.UIElements;

public class VitoriaController : MonoBehaviour
{
    void Start()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        int vencedor = SumoGameManager.Instance.VencedorFinal;
        string nomeBolinha = SumoGameManager.Instance.NomeBolinhaVencedora;

        root.Q<Label>("label-vencedor").text = $"Jogador {vencedor} venceu!";
        root.Q<Label>("label-bolinha").text = $"Bolinha: {nomeBolinha}";

        root.Q<Button>("btn-voltar").clicked += () =>
            SumoGameManager.Instance.IrParaSelecao();
    }
}