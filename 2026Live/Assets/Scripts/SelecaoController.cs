using UnityEngine;
using UnityEngine.UIElements;

public class SelecaoController : MonoBehaviour
{
    [SerializeField] private BolinhaData[] bolinhas;

    private int _indiceJ1 = 0;
    private int _indiceJ2 = 0;
    private bool _corPrimariaJ1 = true; // true = primária, false = secundária
    private bool _corPrimariaJ2 = true;
    private bool _confirmadoJ1 = false;
    private bool _confirmadoJ2 = false;

    private Label _nomeJ1, _nomeJ2;
    private Button _anteriorJ1, _proximoJ1, _confirmarJ1, _trocarCorJ1;
    private Button _anteriorJ2, _proximoJ2, _confirmarJ2, _trocarCorJ2;
    private VisualElement _iconeJ1, _iconeJ2;
    private Label _labelAviso, _labelCorJ1, _labelCorJ2;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _nomeJ1      = root.Q<Label>("nome-j1");
        _anteriorJ1  = root.Q<Button>("anterior-j1");
        _proximoJ1   = root.Q<Button>("proximo-j1");
        _confirmarJ1 = root.Q<Button>("confirmar-j1");
        _trocarCorJ1 = root.Q<Button>("trocar-cor-j1");
        _iconeJ1     = root.Q<VisualElement>("icone-j1");
        _labelCorJ1  = root.Q<Label>("label-cor-j1");

        _nomeJ2      = root.Q<Label>("nome-j2");
        _anteriorJ2  = root.Q<Button>("anterior-j2");
        _proximoJ2   = root.Q<Button>("proximo-j2");
        _confirmarJ2 = root.Q<Button>("confirmar-j2");
        _trocarCorJ2 = root.Q<Button>("trocar-cor-j2");
        _iconeJ2     = root.Q<VisualElement>("icone-j2");
        _labelCorJ2  = root.Q<Label>("label-cor-j2");

        _labelAviso = root.Q<Label>("aviso");

        root.Q<VisualElement>("painel-j1").pickingMode = PickingMode.Ignore;
        root.Q<VisualElement>("painel-j2").pickingMode = PickingMode.Ignore;

        _anteriorJ1.clicked  += () => Navegar(ref _indiceJ1, -1, _confirmadoJ1, AtualizarJ1);
        _proximoJ1.clicked   += () => Navegar(ref _indiceJ1,  1, _confirmadoJ1, AtualizarJ1);
        _trocarCorJ1.clicked += () => TrocarCor(1);
        _confirmarJ1.clicked += () => Confirmar(1);

        _anteriorJ2.clicked  += () => Navegar(ref _indiceJ2, -1, _confirmadoJ2, AtualizarJ2);
        _proximoJ2.clicked   += () => Navegar(ref _indiceJ2,  1, _confirmadoJ2, AtualizarJ2);
        _trocarCorJ2.clicked += () => TrocarCor(2);
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

    void TrocarCor(int jogador)
    {
        if (jogador == 1 && !_confirmadoJ1)
        {
            _corPrimariaJ1 = !_corPrimariaJ1;
            AtualizarJ1();
        }
        else if (jogador == 2 && !_confirmadoJ2)
        {
            _corPrimariaJ2 = !_corPrimariaJ2;
            AtualizarJ2();
        }
    }

    void Confirmar(int jogador)
    {
        if (jogador == 1)
        {
            _confirmadoJ1 = !_confirmadoJ1;
            _confirmarJ1.text = _confirmadoJ1 ? "✓ Confirmado" : "Confirmar";
            _anteriorJ1.SetEnabled(!_confirmadoJ1);
            _proximoJ1.SetEnabled(!_confirmadoJ1);
            _trocarCorJ1.SetEnabled(!_confirmadoJ1);
        }
        else
        {
            _confirmadoJ2 = !_confirmadoJ2;
            _confirmarJ2.text = _confirmadoJ2 ? "✓ Confirmado" : "Confirmar";
            _anteriorJ2.SetEnabled(!_confirmadoJ2);
            _proximoJ2.SetEnabled(!_confirmadoJ2);
            _trocarCorJ2.SetEnabled(!_confirmadoJ2);
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
        SumoGameManager.Instance.DefinirCores(_corPrimariaJ1, _corPrimariaJ2);
        SumoGameManager.Instance.IrParaGameplay();
    }

    void AtualizarJ1()
    {
        if (bolinhas == null || bolinhas.Length == 0) return;
        var b = bolinhas[_indiceJ1];
        _nomeJ1.text = b.nomeBolinha;
        Color cor = _corPrimariaJ1 ? b.corPrimaria : b.corSecundaria;
        _iconeJ1.style.backgroundColor = new StyleColor(cor);
        _labelCorJ1.text = _corPrimariaJ1 ? "Cor: Primária" : "Cor: Secundária";
    }

    void AtualizarJ2()
    {
        if (bolinhas == null || bolinhas.Length == 0) return;
        var b = bolinhas[_indiceJ2];
        _nomeJ2.text = b.nomeBolinha;
        Color cor = _corPrimariaJ2 ? b.corPrimaria : b.corSecundaria;
        _iconeJ2.style.backgroundColor = new StyleColor(cor);
        _labelCorJ2.text = _corPrimariaJ2 ? "Cor: Primária" : "Cor: Secundária";
    }
}