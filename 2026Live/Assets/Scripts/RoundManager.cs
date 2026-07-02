using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    [SerializeField] private Bolinha bolinhaJ1;
    [SerializeField] private Bolinha bolinhaJ2;
    [SerializeField] private Vector3 spawnJ1 = new Vector3(-3, 0.5f, 0);
    [SerializeField] private Vector3 spawnJ2 = new Vector3(3, 0.5f, 0);
    [SerializeField] private float alturaQueda = -5f;

    private int _vitoriasJ1 = 0;
    private int _vitoriasJ2 = 0;
    private int _roundAtual = 1;
    private bool _roundAtivo = true;

    public static event System.Action<int, int> OnPlacarAtualizado; // vJ1, vJ2
    public static event System.Action<int> OnRoundFinalizado;       // vencedor (1 ou 2)
    public static event System.Action<int> OnPartidaFinalizada;     // vencedor (1 ou 2)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
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
        _roundAtivo = false;

        if (vencedor == 1) _vitoriasJ1++;
        else _vitoriasJ2++;

        OnPlacarAtualizado?.Invoke(_vitoriasJ1, _vitoriasJ2);
        OnRoundFinalizado?.Invoke(vencedor);

        if (_vitoriasJ1 >= 2 || _vitoriasJ2 >= 2)
        {
            int vencedorPartida = _vitoriasJ1 >= 2 ? 1 : 2;
            OnPartidaFinalizada?.Invoke(vencedorPartida);
            SumoGameManager.Instance.IrParaVitoria(vencedorPartida);
            return;
        }

        Invoke(nameof(IniciarNovoRound), 2f);
    }

    void IniciarNovoRound()
    {
        _roundAtual++;
        bolinhaJ1.Resetar(spawnJ1);
        bolinhaJ2.Resetar(spawnJ2);
        _roundAtivo = true;
    }
}