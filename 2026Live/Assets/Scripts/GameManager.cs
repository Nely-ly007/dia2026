using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string BootSceneName    = "_boot";
    private const string SplashSceneName  = "Splash";
    private const string MenuSceneName    = "MenuPrincipal";
    private const string SelecaoSceneName = "SelecaoBolinha";
    private const string GUI_SCENE_NAME   = "GUI";

    private GameState _estadoAtual;
    public GameState EstadoAtual => _estadoAtual;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        MudarEstado(GameState.Iniciando);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == BootSceneName)
            CarregarCena(MenuSceneName);
    }

    public void CarregarCena(string nomeDaCena)
    {
        if (!PodeCarregarCena(nomeDaCena))
        {
            Debug.LogWarning($"[GameManager] Transicao bloqueada: {_estadoAtual} -> {nomeDaCena}");
            return;
        }

        if (_estadoAtual == GameState.Gameplay)
        {
            if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
                SceneManager.UnloadSceneAsync(GUI_SCENE_NAME);
        }

        AtualizarEstadoPorCena(nomeDaCena);
        SceneManager.LoadScene(nomeDaCena);
    }

    // Botão Iniciar do Menu agora delega ao SumoGameManager
    public void BotaoIniciar()
    {
        SumoGameManager.Instance.IrParaSelecao();
    }

    public void BotaoSair()
    {
        QuitGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("[GameManager] Saindo do jogo.");
    }

    public void RestartGame()
    {
        PlayerOM.ResetChannel();
        if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
            SceneManager.UnloadSceneAsync(GUI_SCENE_NAME);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MudarEstado(GameState novoEstado)
    {
        _estadoAtual = novoEstado;
        Debug.Log($"[GameManager] Estado alterado para: {_estadoAtual}");
    }

    public void AlocarInput(PlayerInput playerInput)
    {
        var devices = InputSystem.devices;
        foreach (var device in devices)
        {
            if (device is Gamepad || device is Keyboard)
            {
                playerInput.SwitchCurrentControlScheme(device);
                Debug.Log($"[GameManager] Input alocado: {device.displayName}");
                return;
            }
        }
        Debug.LogWarning("[GameManager] Nenhum dispositivo de input encontrado.");
    }

    private bool PodeCarregarCena(string nomeDaCena)
    {
        if (_estadoAtual == GameState.Iniciando)
            return nomeDaCena == MenuSceneName;

        if (_estadoAtual == GameState.MenuPrincipal)
            return nomeDaCena == SelecaoSceneName || nomeDaCena == SplashSceneName;

        if (_estadoAtual == GameState.Splash)
            return nomeDaCena == MenuSceneName;

        if (_estadoAtual == GameState.Selecao)
            return nomeDaCena == MenuSceneName;

        return false;
    }

    private void AtualizarEstadoPorCena(string nomeDaCena)
    {
        if (nomeDaCena == SplashSceneName)   { MudarEstado(GameState.Splash);        return; }
        if (nomeDaCena == MenuSceneName)     { MudarEstado(GameState.MenuPrincipal); return; }
        if (nomeDaCena == SelecaoSceneName)  { MudarEstado(GameState.Selecao);       return; }
    }
}