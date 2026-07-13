using UnityEngine;

public class SumoSetup : MonoBehaviour
{
    [SerializeField] private Bolinha bolinhaJ1;
    [SerializeField] private Bolinha bolinhaJ2;

    void Start()
    {
        if (SumoGameManager.Instance == null) return;

        bolinhaJ1.dados = SumoGameManager.Instance.DadosJ1;
        bolinhaJ2.dados = SumoGameManager.Instance.DadosJ2;

        // Força a aplicação das cores após atribuir os dados
        bolinhaJ1.SendMessage("AplicarCores");
        bolinhaJ2.SendMessage("AplicarCores");
    }
}