using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // --- Singleton ---
    public static GameManager Instance { get; private set; }

    private const string BootSceneName = "_boot";
    private const string SplashSceneName = "Splash";
    private const string MenuSceneName = "MenuPrincipal";
    private const string GameplaySceneName = "novo";

    // --- Estado atual ---
    private GameState _estadoAtual;
    public GameState EstadoAtual => _estadoAtual;

    void Awake()
    {
        // Padrão Singleton com DontDestroyOnLoad
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
        // Fluxo inicial: Boot (Iniciando) -> MenuPrincipal.
        if (SceneManager.GetActiveScene().name == BootSceneName)
        {
            CarregarCena(MenuSceneName);
            return;
        }

        AtualizarEstadoPorCena(SceneManager.GetActiveScene().name);
    }

    // --- Único ponto de mudança de cena no jogo ---
    public void CarregarCena(string nomeDaCena)
    {
        // Regras de autorização por estado.
        if (!PodeCarregarCena(nomeDaCena))
        {
            Debug.LogWarning($"[GameManager] Transicao bloqueada: {_estadoAtual} -> {nomeDaCena}");
            return;
        }

        AtualizarEstadoPorCena(nomeDaCena);
        SceneManager.LoadScene(nomeDaCena);
    }

    public void BotaoIniciar()
    {
        CarregarCena(GameplaySceneName);
    }

    public void BotaoSair()
    {
        CarregarCena(SplashSceneName);
    }

    // --- Muda o estado e loga no console ---
    private void MudarEstado(GameState novoEstado)
    {
        _estadoAtual = novoEstado;
        Debug.Log($"[GameManager] Estado alterado para: {_estadoAtual}");
    }

    // --- Alocação de input para o jogador ---
    public void AlocarInput(PlayerInput playerInput)
    {
        var devices = InputSystem.devices;
        foreach (var device in devices)
        {
            // Aloca o primeiro dispositivo disponível ao jogador
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
        {
            return nomeDaCena == MenuSceneName;
        }

        if (_estadoAtual == GameState.MenuPrincipal)
        {
            return nomeDaCena == GameplaySceneName || nomeDaCena == SplashSceneName;
        }

        if (_estadoAtual == GameState.Splash)
        {
            return nomeDaCena == MenuSceneName;
        }

        if (_estadoAtual == GameState.Gameplay)
        {
            return nomeDaCena == MenuSceneName || nomeDaCena == SplashSceneName;
        }

        return false;
    }

    private void AtualizarEstadoPorCena(string nomeDaCena)
    {
        if (nomeDaCena == SplashSceneName)
        {
            MudarEstado(GameState.Splash);
            return;
        }

        if (nomeDaCena == MenuSceneName)
        {
            MudarEstado(GameState.MenuPrincipal);
            return;
        }

        if (nomeDaCena == GameplaySceneName)
        {
            MudarEstado(GameState.Gameplay);
        }
    }

}