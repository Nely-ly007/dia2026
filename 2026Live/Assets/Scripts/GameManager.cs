using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // --- Singleton ---
    public static GameManager Instance { get; private set; }

    private const string BootSceneName = "_boot";
    private const string SplashSceneName = "Splash";
    private const string MenuSceneName = "MenuPrincipal";
    private const string GameplaySceneName = "novo";
    private const string GUI_SCENE_NAME = "GUI";

    // --- Estado atual ---
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
        {
            CarregarCena(MenuSceneName);
        }
    }

    // --- Único ponto de mudança de cena no jogo ---
    public void CarregarCena(string nomeDaCena)
    {
        if (!PodeCarregarCena(nomeDaCena))
        {
            Debug.LogWarning($"[GameManager] Transicao bloqueada: {_estadoAtual} -> {nomeDaCena}");
            return;
        }

        // Se estava no Gameplay e vai sair, descarrega a GUI antes
        if (_estadoAtual == GameState.Gameplay)
        {
            if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
                SceneManager.UnloadSceneAsync(GUI_SCENE_NAME);
        }

        AtualizarEstadoPorCena(nomeDaCena);
        SceneManager.LoadScene(nomeDaCena);

        // Se entrou no Gameplay, carrega a GUI de forma aditiva
        if (_estadoAtual == GameState.Gameplay)
        {
            StartCoroutine(LoadGUIScene());
        }
    }

    private IEnumerator LoadGUIScene()
    {
        if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
        {
            Debug.Log("[GameManager] Cena GUI já estava carregada.");
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GUI_SCENE_NAME, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
            yield return null;

        Debug.Log("[GameManager] Cena GUI carregada de forma aditiva com sucesso.");
    }

    public void BotaoIniciar()
    {
        CarregarCena(GameplaySceneName);
    }

    public void BotaoSair()
    {
        QuitGame();
    }

    public void RestartGame()
    {
        PlayerOM.ResetChannel();

        if (SceneManager.GetSceneByName(GUI_SCENE_NAME).isLoaded)
            SceneManager.UnloadSceneAsync(GUI_SCENE_NAME);

        SceneManager.LoadScene(GameplaySceneName);
        StartCoroutine(LoadGUIScene());
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("[GameManager] Saindo do jogo.");
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
            return nomeDaCena == GameplaySceneName || nomeDaCena == SplashSceneName;

        if (_estadoAtual == GameState.Splash)
            return nomeDaCena == MenuSceneName;

        if (_estadoAtual == GameState.Gameplay)
            return nomeDaCena == MenuSceneName || nomeDaCena == SplashSceneName || nomeDaCena == GameplaySceneName;

        return false;
    }

    private void AtualizarEstadoPorCena(string nomeDaCena)
    {
        if (nomeDaCena == SplashSceneName)   { MudarEstado(GameState.Splash);        return; }
        if (nomeDaCena == MenuSceneName)     { MudarEstado(GameState.MenuPrincipal); return; }
        if (nomeDaCena == GameplaySceneName) { MudarEstado(GameState.Gameplay);      return; }
    }
}