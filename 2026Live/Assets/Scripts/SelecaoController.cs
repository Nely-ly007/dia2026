using UnityEngine;
using UnityEngine.UIElements;

public class SelecaoController : MonoBehaviour
{
    [SerializeField] private BolinhaData[] bolinhas;

    // Lista de cores disponíveis para escolher
    private readonly Color[] _coresDisponiveis = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        new Color(1f, 0.5f, 0f),     // laranja
        new Color(0.5f, 0f, 1f),     // roxo
        Color.cyan,
        Color.white,
        Color.black,
        new Color(1f, 0.4f, 0.7f),   // rosa
    };

    private readonly string[] _nomesCores = new string[]
    {
        "Vermelho", "Azul", "Verde", "Amarelo", "Laranja",
        "Roxo", "Ciano", "Branco", "Preto", "Rosa"
    };

    // Índices de bolinha
    private int _indiceJ1 = 0;
    private int _indiceJ2 = 0;

    // Índices de cor do corpo e faixa por jogador
    private int _indiceCorCorpoJ1 = 0;
    private int _indiceCorFaixaJ1 = 1;
    private int _indiceCorCorpoJ2 = 2;
    private int _indiceCorFaixaJ2 = 3;

    private bool _confirmadoJ1 = false;
    private bool _confirmadoJ2 = false;

    // UI J1
    private Button _anteriorJ1, _proximoJ1, _confirmarJ1;
    private Button _corCorpoAnteriorJ1, _corCorpoProximoJ1;
    private Button _corFaixaAnteriorJ1, _corFaixaProximoJ1;
    private VisualElement _iconeCorpoJ1, _iconeFaixaJ1;
    private Label _nomeJ1, _labelCorCorpoJ1, _labelCorFaixaJ1;

    // UI J2
    private Button _anteriorJ2, _proximoJ2, _confirmarJ2;
    private Button _corCorpoAnteriorJ2, _corCorpoProximoJ2;
    private Button _corFaixaAnteriorJ2, _corFaixaProximoJ2;
    private VisualElement _iconeCorpoJ2, _iconeFaixaJ2;
    private Label _nomeJ2, _labelCorCorpoJ2, _labelCorFaixaJ2;

    private Label _labelAviso;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<VisualElement>("painel-j1").pickingMode = PickingMode.Ignore;
        root.Q<VisualElement>("painel-j2").pickingMode = PickingMode.Ignore;

        // J1
        _nomeJ1              = root.Q<Label>("nome-j1");
        _anteriorJ1          = root.Q<Button>("anterior-j1");
        _proximoJ1           = root.Q<Button>("proximo-j1");
        _confirmarJ1         = root.Q<Button>("confirmar-j1");
        _iconeCorpoJ1        = root.Q<VisualElement>("icone-j1");
        _iconeFaixaJ1        = root.Q<VisualElement>("icone-faixa-j1");
        _labelCorCorpoJ1     = root.Q<Label>("label-cor-corpo-j1");
        _labelCorFaixaJ1     = root.Q<Label>("label-cor-faixa-j1");
        _corCorpoAnteriorJ1  = root.Q<Button>("corpo-anterior-j1");
        _corCorpoProximoJ1   = root.Q<Button>("corpo-proximo-j1");
        _corFaixaAnteriorJ1  = root.Q<Button>("faixa-anterior-j1");
        _corFaixaProximoJ1   = root.Q<Button>("faixa-proximo-j1");

        // J2
        _nomeJ2              = root.Q<Label>("nome-j2");
        _anteriorJ2          = root.Q<Button>("anterior-j2");
        _proximoJ2           = root.Q<Button>("proximo-j2");
        _confirmarJ2         = root.Q<Button>("confirmar-j2");
        _iconeCorpoJ2        = root.Q<VisualElement>("icone-j2");
        _iconeFaixaJ2        = root.Q<VisualElement>("icone-faixa-j2");
        _labelCorCorpoJ2     = root.Q<Label>("label-cor-corpo-j2");
        _labelCorFaixaJ2     = root.Q<Label>("label-cor-faixa-j2");
        _corCorpoAnteriorJ2  = root.Q<Button>("corpo-anterior-j2");
        _corCorpoProximoJ2   = root.Q<Button>("corpo-proximo-j2");
        _corFaixaAnteriorJ2  = root.Q<Button>("faixa-anterior-j2");
        _corFaixaProximoJ2   = root.Q<Button>("faixa-proximo-j2");

        _labelAviso = root.Q<Label>("aviso");

        // Eventos J1
        _anteriorJ1.clicked         += () => Navegar(ref _indiceJ1, -1, _confirmadoJ1, AtualizarJ1);
        _proximoJ1.clicked          += () => Navegar(ref _indiceJ1,  1, _confirmadoJ1, AtualizarJ1);
        _corCorpoAnteriorJ1.clicked += () => NavCor(ref _indiceCorCorpoJ1, -1, _confirmadoJ1, AtualizarJ1);
        _corCorpoProximoJ1.clicked  += () => NavCor(ref _indiceCorCorpoJ1,  1, _confirmadoJ1, AtualizarJ1);
        _corFaixaAnteriorJ1.clicked += () => NavCor(ref _indiceCorFaixaJ1, -1, _confirmadoJ1, AtualizarJ1);
        _corFaixaProximoJ1.clicked  += () => NavCor(ref _indiceCorFaixaJ1,  1, _confirmadoJ1, AtualizarJ1);
        _confirmarJ1.clicked        += () => Confirmar(1);

        // Eventos J2
        _anteriorJ2.clicked         += () => Navegar(ref _indiceJ2, -1, _confirmadoJ2, AtualizarJ2);
        _proximoJ2.clicked          += () => Navegar(ref _indiceJ2,  1, _confirmadoJ2, AtualizarJ2);
        _corCorpoAnteriorJ2.clicked += () => NavCor(ref _indiceCorCorpoJ2, -1, _confirmadoJ2, AtualizarJ2);
        _corCorpoProximoJ2.clicked  += () => NavCor(ref _indiceCorCorpoJ2,  1, _confirmadoJ2, AtualizarJ2);
        _corFaixaAnteriorJ2.clicked += () => NavCor(ref _indiceCorFaixaJ2, -1, _confirmadoJ2, AtualizarJ2);
        _corFaixaProximoJ2.clicked  += () => NavCor(ref _indiceCorFaixaJ2,  1, _confirmadoJ2, AtualizarJ2);
        _confirmarJ2.clicked        += () => Confirmar(2);

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

    void NavCor(ref int indice, int direcao, bool confirmado, System.Action atualizar)
    {
        if (confirmado) return;
        indice = (indice + direcao + _coresDisponiveis.Length) % _coresDisponiveis.Length;
        atualizar();
    }

    void Confirmar(int jogador)
    {
        if (jogador == 1)
        {
            _confirmadoJ1 = !_confirmadoJ1;
            _confirmarJ1.text = _confirmadoJ1 ? "✓ Confirmado" : "Confirmar";
            SetJ1Interactable(!_confirmadoJ1);
        }
        else
        {
            _confirmadoJ2 = !_confirmadoJ2;
            _confirmarJ2.text = _confirmadoJ2 ? "✓ Confirmado" : "Confirmar";
            SetJ2Interactable(!_confirmadoJ2);
        }

        TentarIniciar();
    }

    void SetJ1Interactable(bool ativo)
    {
        _anteriorJ1.SetEnabled(ativo);
        _proximoJ1.SetEnabled(ativo);
        _corCorpoAnteriorJ1.SetEnabled(ativo);
        _corCorpoProximoJ1.SetEnabled(ativo);
        _corFaixaAnteriorJ1.SetEnabled(ativo);
        _corFaixaProximoJ1.SetEnabled(ativo);
    }

    void SetJ2Interactable(bool ativo)
    {
        _anteriorJ2.SetEnabled(ativo);
        _proximoJ2.SetEnabled(ativo);
        _corCorpoAnteriorJ2.SetEnabled(ativo);
        _corCorpoProximoJ2.SetEnabled(ativo);
        _corFaixaAnteriorJ2.SetEnabled(ativo);
        _corFaixaProximoJ2.SetEnabled(ativo);
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
        SumoGameManager.Instance.DefinirCores(
            _coresDisponiveis[_indiceCorCorpoJ1], _coresDisponiveis[_indiceCorFaixaJ1],
            _coresDisponiveis[_indiceCorCorpoJ2], _coresDisponiveis[_indiceCorFaixaJ2]
        );
        SumoGameManager.Instance.IrParaGameplay();
    }

    void AtualizarJ1()
    {
        if (bolinhas == null || bolinhas.Length == 0) return;
        _nomeJ1.text = bolinhas[_indiceJ1].nomeBolinha;
        _iconeCorpoJ1.style.backgroundColor  = new StyleColor(_coresDisponiveis[_indiceCorCorpoJ1]);
        _iconeFaixaJ1.style.backgroundColor  = new StyleColor(_coresDisponiveis[_indiceCorFaixaJ1]);
        _labelCorCorpoJ1.text = $"Corpo: {_nomesCores[_indiceCorCorpoJ1]}";
        _labelCorFaixaJ1.text = $"Faixa: {_nomesCores[_indiceCorFaixaJ1]}";
    }

    void AtualizarJ2()
    {
        if (bolinhas == null || bolinhas.Length == 0) return;
        _nomeJ2.text = bolinhas[_indiceJ2].nomeBolinha;
        _iconeCorpoJ2.style.backgroundColor  = new StyleColor(_coresDisponiveis[_indiceCorCorpoJ2]);
        _iconeFaixaJ2.style.backgroundColor  = new StyleColor(_coresDisponiveis[_indiceCorFaixaJ2]);
        _labelCorCorpoJ2.text = $"Corpo: {_nomesCores[_indiceCorCorpoJ2]}";
        _labelCorFaixaJ2.text = $"Faixa: {_nomesCores[_indiceCorFaixaJ2]}";
    }
}