using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    [SerializeField] private Bolinha bolinhaJ1;
    [SerializeField] private Bolinha bolinhaJ2;
    [SerializeField] private Vector3 spawnJ1 = new Vector3(-3, 2.5f, 0);
    [SerializeField] private Vector3 spawnJ2 = new Vector3(3, 2.5f, 0);
    [SerializeField] private float alturaQueda = -3f;

    private int _vitoriasJ1 = 0;
    private int _vitoriasJ2 = 0;
    private bool _roundAtivo = true;

    public static event System.Action<int, int> OnPlacarAtualizado;
    public static event System.Action<int> OnRoundFinalizado;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (!_roundAtivo) return;

        if (bolinhaJ1.transform.position.y < alturaQueda)
            FinalizarRound(vencedor: 2);
        else if (bolinhaJ2.transform.position.y < alturaQueda)
            FinalizarRound(vencedor: 1);
    }

    void FinalizarRound(int vencedor)
    {
        // Trava extra contra chamadas duplicadas no mesmo frame/ciclo
        if (!_roundAtivo) return;
        _roundAtivo = false;

        // Cancela qualquer reset pendente de uma chamada anterior
        CancelInvoke(nameof(IniciarNovoRound));

        if (vencedor == 1) _vitoriasJ1++;
        else _vitoriasJ2++;

        OnPlacarAtualizado?.Invoke(_vitoriasJ1, _vitoriasJ2);
        OnRoundFinalizado?.Invoke(vencedor);

        Debug.Log(
            $"[RoundManager] Round finalizado! Vencedor: J{vencedor} | Placar: J1={_vitoriasJ1} J2={_vitoriasJ2} | Tempo={Time.time:F2}");

        if (_vitoriasJ1 >= 2 || _vitoriasJ2 >= 2)
        {
            int vencedorPartida = _vitoriasJ1 >= 2 ? 1 : 2;
            string nomeBolinha = vencedorPartida == 1
                ? SumoGameManager.Instance.DadosJ1.nomeBolinha
                : SumoGameManager.Instance.DadosJ2.nomeBolinha;

            Debug.Log($"[RoundManager] Partida finalizada! Vencedor: J{vencedorPartida} ({nomeBolinha})");
            SumoGameManager.Instance.DefinirVencedor(vencedorPartida, nomeBolinha);
            SumoGameManager.Instance.IrParaVitoria(vencedorPartida);
            return;
        }

        Invoke(nameof(IniciarNovoRound), 2f);
    }

    void IniciarNovoRound()
    {
        bolinhaJ1.Resetar(spawnJ1);
        bolinhaJ2.Resetar(spawnJ2);
        _roundAtivo = true;

        Debug.Log(
            $"[RoundManager] Novo round iniciado | J1 pos={bolinhaJ1.transform.position} J2 pos={bolinhaJ2.transform.position} | Tempo={Time.time:F2}");
    }
}