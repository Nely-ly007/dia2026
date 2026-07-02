using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Bolinha : MonoBehaviour
{
    [Header("Dados")]
    public BolinhaData dados;
    public int jogadorIndex; // 0 = J1, 1 = J2
    public Bolinha inimiga;

    [Header("Cooldown")]
    public float cooldownMax = 3f;
    private float _cooldownAtual = 0f;
    public float CooldownNormalizado => Mathf.Clamp01(_cooldownAtual / cooldownMax);
    public bool PodeUsar => _cooldownAtual <= 0f;

    private Rigidbody _rb;
    private int _moedasColetadas = 0;
    private Vector2 _moveInput;

    // Eventos Observer
    public static event System.Action<int, float> OnCooldownAtualizado; // jogadorIndex, 0-1
    public static event System.Action<int, int> OnMoedasAtualizadas;    // jogadorIndex, total

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        AplicarDados();
    }

    void AplicarDados()
    {
        if (dados == null) return;

        transform.localScale = Vector3.one * dados.tamanho;
        _rb.mass = dados.massaBase;

        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = jogadorIndex == 0 ? dados.corJogador1 : dados.corJogador2;
    }

    void Update()
    {
        if (_cooldownAtual > 0f)
        {
            _cooldownAtual -= Time.deltaTime;
            if (_cooldownAtual < 0f) _cooldownAtual = 0f;
            OnCooldownAtualizado?.Invoke(jogadorIndex, CooldownNormalizado);
        }
    }

    void FixedUpdate()
    {
        float velAtual = dados != null
            ? dados.velocidade - (dados.lentidaoPorMoeda * _moedasColetadas)
            : 6f;
        velAtual = Mathf.Max(velAtual, 1f);

        Vector3 direcao = new Vector3(_moveInput.x, 0f, _moveInput.y);
        _rb.AddForce(direcao * velAtual, ForceMode.Acceleration);
    }

    // Chamado pelo PlayerInputHandler
    public void OnMover(Vector2 input) => _moveInput = input;

    public void OnAcao()
    {
        if (!PodeUsar || inimiga == null) return;

        Vector3 direcao = (inimiga.transform.position - transform.position);
        float distancia = direcao.magnitude;
        direcao.y = 0f;
        direcao.Normalize();

        float forcaBase = dados != null ? dados.forcaEmpurrao : 10f;
        float forcaMoedas = dados != null ? dados.forcaPorMoeda * _moedasColetadas : 0f;
        // Quanto mais perto, mais forte
        float multiplicadorDistancia = Mathf.Clamp(1f / Mathf.Max(distancia, 0.1f), 1f, 5f);

        float forcaFinal = (forcaBase + forcaMoedas) * multiplicadorDistancia;
        inimiga.GetComponent<Rigidbody>().AddForce(direcao * forcaFinal, ForceMode.Impulse);

        _cooldownAtual = cooldownMax;
        OnCooldownAtualizado?.Invoke(jogadorIndex, 1f);
    }

    public void ColetarMoeda()
    {
        _moedasColetadas++;
        if (dados != null)
            _rb.mass = dados.massaBase + (dados.massaPorMoeda * _moedasColetadas);

        OnMoedasAtualizadas?.Invoke(jogadorIndex, _moedasColetadas);
    }

    public void Resetar(Vector3 posicaoInicial)
    {
        _moedasColetadas = 0;
        _cooldownAtual = 0f;
        _rb.mass = dados != null ? dados.massaBase : 1f;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = posicaoInicial;
        AplicarDados();
    }
}