using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SumoGameManager : MonoBehaviour
{
    public string NomeBolinhaVencedora { get; private set; }

    public void DefinirVencedor(int vencedor, string nomeBolinha)
    {
        VencedorFinal = vencedor;
        NomeBolinhaVencedora = nomeBolinha;
    }
    public static SumoGameManager Instance { get; private set; }

    private const string SelecaoSceneName  = "SelecaoBolinha";
    private const string GameplaySceneName = "SumoGameplay";
    private const string VitoriaSceneName  = "Vitoria";
    private const string GUI_SCENE_NAME    = "SumoGUI";
    private const string MenuSceneName     = "MenuPrincipal";

    public BolinhaData DadosJ1 { get; private set; }
    public BolinhaData DadosJ2 { get; private set; }
    public int VencedorFinal   { get; private set; }

    // Cores escolhidas na seleção
    public Color CorCorpoJ1  { get; private set; }
    public Color CorFaixaJ1  { get; private set; }
    public Color CorCorpoJ2  { get; private set; }
    public Color CorFaixaJ2  { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DefinirEscolhas(BolinhaData j1, BolinhaData j2)
    {
        DadosJ1 = j1;
        DadosJ2 = j2;
        Debug.Log($"[SumoGameManager] Escolhas definidas: {j1.nomeBolinha} vs {j2.nomeBolinha}");
    }

    public void DefinirCores(Color corCorpoJ1, Color corFaixaJ1, Color corCorpoJ2, Color corFaixaJ2)
    {
        CorCorpoJ1 = corCorpoJ1;
        CorFaixaJ1 = corFaixaJ1;
        CorCorpoJ2 = corCorpoJ2;
        CorFaixaJ2 = corFaixaJ2;
    }

    public void IrParaSelecao()
    {
        DescarregarGUI();
        SceneManager.LoadScene(SelecaoSceneName);
        Debug.Log("[SumoGameManager] Indo para SelecaoBolinha.");
    }

    public void IrParaGameplay()
    {
        SceneManager.LoadScene(GameplaySceneName);
        StartCoroutine(CarregarGUI());
        Debug.Log("[SumoGameManager] Iniciando SumoGameplay.");
    }

    public void IrParaVitoria(int vencedor)
    {
        VencedorFinal = vencedor;
        DescarregarGUI();
        SceneManager.LoadScene(VitoriaSceneName);
        Debug.Log($"[SumoGameManager] Jogador {vencedor} venceu a partida!");
    }

    public void IrParaMenu()
    {
        DescarregarGUI();
        SceneManager.LoadScene(MenuSceneName);
    }

    private void DescarregarGUI()
    {
        if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
            SceneManager.UnloadSceneAsync(GUI_SCENE_NAME);
    }

    private IEnumerator CarregarGUI()
    {
        if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
            yield break;

        AsyncOperation op = SceneManager.LoadSceneAsync(GUI_SCENE_NAME, LoadSceneMode.Additive);
        while (!op.isDone)
            yield return null;

        Debug.Log("[SumoGameManager] SumoGUI carregada de forma aditiva.");
    }
}