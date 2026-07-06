using UnityEngine;
using UnityEngine.UIElements;

public class SelecaoController : MonoBehaviour
{
    [Header("Bolinhas disponíveis")] [SerializeField]
    private BolinhaData[] bolinhas;

    private int _indiceJ1 = 0;
    private int _indiceJ2 = 0;
    private bool _confirmadoJ1 = false;
    private bool _confirmadoJ2 = false;

    // Elementos UI - Jogador 1
    private Label _nomeJ1;
    private Label _statusJ1;
    private Button _anteriorJ1;
    private Button _proximoJ1;
    private Button _confirmarJ1;
    private VisualElement _iconeJ1;

    // Elementos UI - Jogador 2
    private Label _nomeJ2;
    private Label _statusJ2;
    private Button _anteriorJ2;
    private Button _proximoJ2;
    private Button _confirmarJ2;
    private VisualElement _iconeJ2;

    private Label _labelAviso;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Jogador 1
        _nomeJ1 = root.Q<Label>("nome-j1");
        _statusJ1 = root.Q<Label>("status-j1");
        _anteriorJ1 = root.Q<Button>("anterior-j1");
        _proximoJ1 = root.Q<Button>("proximo-j1");
        _confirmarJ1 = root.Q<Button>("confirmar-j1");
        _iconeJ1 = root.Q<VisualElement>("icone-j1");

        // Jogador 2
        _nomeJ2 = root.Q<Label>("nome-j2");
        _statusJ2 = root.Q<Label>("status-j2");
        _anteriorJ2 = root.Q<Button>("anterior-j2");
        _proximoJ2 = root.Q<Button>("proximo-j2");
        _confirmarJ2 = root.Q<Button>("confirmar-j2");
        _iconeJ2 = root.Q<VisualElement>("icone-j2");

        _labelAviso = root.Q<Label>("aviso");

        // Botões J1
        _anteriorJ1.clicked += () => Navegar(ref _indiceJ1, -1, _confirmadoJ1, AtualizarJ1);
        _proximoJ1.clicked += () => Navegar(ref _indiceJ1, 1, _confirmadoJ1, AtualizarJ1);
        _confirmarJ1.clicked += () => Confirmar(1);

        // Botões J2
        _anteriorJ2.clicked += () => Navegar(ref _indiceJ2, -1, _confirmadoJ2, AtualizarJ2);
        _proximoJ2.clicked += () => Navegar(ref _indiceJ2, 1, _confirmadoJ2, AtualizarJ2);
        _confirmarJ2.clicked += () => Confirmar(2);

        AtualizarJ1();
        AtualizarJ2();
        _labelAviso.text = "";
    }

    void Navegar(ref int indice, int direcao, bool confirmado, System.Action atualizar)
    {
        if (confirmado) return;
        indice = (indice + direcao + bolinhas.Length) % bolinhas.Length;
        atualizar();
    }

    void Confirmar(int jogador)
    {
        if (jogador == 1)
        {
            _confirmadoJ1 = !_confirmadoJ1;
            _confirmarJ1.text = _confirmadoJ1 ? "✓ Confirmado" : "Confirmar";
            _anteriorJ1.SetEnabled(!_confirmadoJ1);
            _proximoJ1.SetEnabled(!_confirmadoJ1);
        }
        else
        {
            _confirmadoJ2 = !_confirmadoJ2;
            _confirmarJ2.text = _confirmadoJ2 ? "✓ Confirmado" : "Confirmar";
            _anteriorJ2.SetEnabled(!_confirmadoJ2);
            _proximoJ2.SetEnabled(!_confirmadoJ2);
        }

        TentarIniciar();
    }

    void TentarIniciar()
    {
        if (!_confirmadoJ1 || !_confirmadoJ2)
        {
            _labelAviso.text = "Aguardando os dois jogadores confirmarem...";
            return;
        }

        _labelAviso.text = "Iniciando partida!";
        SumoGameManager.Instance.DefinirEscolhas(bolinhas[_indiceJ1], bolinhas[_indiceJ2]);
        SumoGameManager.Instance.IrParaGameplay();
    }

    void AtualizarJ1()
    {
        if (bolinhas == null || bolinhas.Length == 0) return;
        var b = bolinhas[_indiceJ1];
        _nomeJ1.text = b.nomeBolinha;
        _statusJ1.text = FormatarStatus(b);
        AtualizarIcone(_iconeJ1, b.corJogador1);
    }
}