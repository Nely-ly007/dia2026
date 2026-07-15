using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bolinha : MonoBehaviour
{
    [Header("Dados")]
    public BolinhaData dados;
    public int jogadorIndex;
    public Bolinha inimiga;

    [Header("Referências visuais")]
    [SerializeField] private Renderer rendererCorpo;
    [SerializeField] private Renderer rendererFaixa;

    [Header("Stats fixos")]
    public float velocidade = 6f;
    public float forcaEmpurrao = 10f;
    public float cooldownMax = 3f;

    private Rigidbody _rb;
    private float _cooldownAtual = 0f;
    private int _moedasColetadas = 0;
    private Vector2 _moveInput;

    public bool PodeUsar => _cooldownAtual <= 0f;
    public float CooldownNormalizado => Mathf.Clamp01(_cooldownAtual / cooldownMax);

    public static event System.Action<int, float> OnCooldownAtualizado;
    public static event System.Action<int, int> OnMoedasAtualizadas;

    void Awake() => _rb = GetComponent<Rigidbody>();

    void Start() => AplicarCores();

    public void AplicarCores()
    {
        if (SumoGameManager.Instance == null) return;

        Color corCorpo, corFaixa;
        if (jogadorIndex == 0)
        {
            corCorpo = SumoGameManager.Instance.CorCorpoJ1;
            corFaixa = SumoGameManager.Instance.CorFaixaJ1;
        }
        else
        {
            corCorpo = SumoGameManager.Instance.CorCorpoJ2;
            corFaixa = SumoGameManager.Instance.CorFaixaJ2;
        }

        if (rendererCorpo != null)
        {
            rendererCorpo.material = new Material(rendererCorpo.material);
            rendererCorpo.material.color = corCorpo;
        }

        if (rendererFaixa != null)
        {
            rendererFaixa.material = new Material(rendererFaixa.material);
            rendererFaixa.material.color = corFaixa;
        }
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
        Vector3 dir = new Vector3(_moveInput.x, 0f, _moveInput.y);
        _rb.AddForce(dir * velocidade, ForceMode.Acceleration);
    }

    public void OnMover(Vector2 input) => _moveInput = input;

    public void OnAcao()
    {
        if (!PodeUsar || inimiga == null) return;

        Vector3 direcao = (inimiga.transform.position - transform.position);
        float distancia = direcao.magnitude;
        direcao.y = 0f;
        direcao.Normalize();

        float multiplicador = Mathf.Clamp(1f / Mathf.Max(distancia, 0.1f), 1f, 5f);
        float forcaFinal = (forcaEmpurrao + _moedasColetadas * 1.5f) * multiplicador;

        inimiga.GetComponent<Rigidbody>().AddForce(direcao * forcaFinal, ForceMode.Impulse);
        _cooldownAtual = cooldownMax;
        OnCooldownAtualizado?.Invoke(jogadorIndex, 1f);
    }

    public void ColetarMoeda()
    {
        _moedasColetadas++;
        OnMoedasAtualizadas?.Invoke(jogadorIndex, _moedasColetadas);
    }

    public void Resetar(Vector3 posicao)
    {
        _moedasColetadas = 0;
        _cooldownAtual = 0f;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = posicao;
        AplicarCores();
    }
}