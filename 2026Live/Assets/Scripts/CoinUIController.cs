using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class CoinUIController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label coinLabel;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("[CoinUIController] UIDocument não encontrado!");
            return;
        }

        if (uiDocument.rootVisualElement == null)
        {
            Debug.LogError("[CoinUIController] rootVisualElement é nulo — verifique o UXML!");
            return;
        }

        coinLabel = uiDocument.rootVisualElement.Q<Label>("coin-label");

        if (coinLabel == null)
            Debug.LogError("[CoinUIController] Label 'coin-label' não encontrado no UXML!");
        else
            Debug.Log("[CoinUIController] Label encontrado com sucesso!");
    }

    private void OnEnable()
    {
        PlayerOM.OnCoinCollected += UpdateCoinDisplay;
        UpdateCoinDisplay(0);
        Debug.Log("[CoinUIController] Inscrito no canal PlayerOM.");
    }

    private void OnDisable()
    {
        PlayerOM.OnCoinCollected -= UpdateCoinDisplay;
    }

    private void UpdateCoinDisplay(int totalCoins)
    {
        if (coinLabel != null)
            coinLabel.text = $"Moedas: {totalCoins}";
        else
            Debug.LogWarning("[CoinUIController] UpdateCoinDisplay chamado mas coinLabel é nulo!");
    }
}