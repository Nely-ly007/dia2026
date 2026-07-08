using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SumoGameManager : MonoBehaviour
{
    public static SumoGameManager Instance { get; private set; }

    private const string SelecaoSceneName  = "SelecaoBolinha";
    private const string GameplaySceneName = "SumoGameplay";
    private const string VitoriaSceneName  = "Vitoria";
    private const string GUI_SCENE_NAME    = "SumoGUI";
    private const string MenuSceneName     = "MenuPrincipal";
    
    public bool CorPrimariaJ1 { get; private set; }
    public bool CorPrimariaJ2 { get; private set; }

    public void DefinirCores(bool j1Primaria, bool j2Primaria)
    {
        CorPrimariaJ1 = j1Primaria;
        CorPrimariaJ2 = j2Primaria;
    }

    public BolinhaData DadosJ1 { get; private set; }
    public BolinhaData DadosJ2 { get; private set; }
    public int VencedorFinal   { get; private set; }

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