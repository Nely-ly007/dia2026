using UnityEngine;
using UnityEngine.UIElements;

public class CoinUIController : MonoBehaviour
{
    private Label coinLabel;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("[CoinUIController] UIDocument não encontrado neste GameObject!");
            return;
        }

        coinLabel = uiDocument.rootVisualElement.Q<Label>("coin-label");

        if (coinLabel == null)
            Debug.LogError("[CoinUIController] Label 'coin-label' não encontrado no UIDocument!");
        else
        {
            coinLabel.text = "Moedas: 0";
            Debug.Log("[CoinUIController] coinLabel encontrado com sucesso.");
        }

        // Inscreve no canal Observer
        PlayerOM.OnCoinCollected += UpdateCoinDisplay;
        Debug.Log("[CoinUIController] Inscrito no PlayerOM.OnCoinCollected.");
    }

    void OnDisable()
    {
        // Desinscreve para evitar chamadas em objeto destruído
        PlayerOM.OnCoinCollected -= UpdateCoinDisplay;
    }

    private void UpdateCoinDisplay(int total)
    {
        if (coinLabel == null)
        {
            Debug.LogWarning("[CoinUIController] coinLabel é nulo ao atualizar!");
            return;
        }

        coinLabel.text = $"Moedas: {total}";
    }
}