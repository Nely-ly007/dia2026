using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button botaoIniciar;
    [SerializeField] private Button botaoSair;

    private const string BotaoIniciarNome = "BotaoIniciar";
    private const string BotaoSairNome = "BotaoSair";

    private void Awake()
    {
        if (botaoIniciar == null)
        {
            botaoIniciar = EncontrarBotao(BotaoIniciarNome);
        }

        if (botaoSair == null)
        {
            botaoSair = EncontrarBotao(BotaoSairNome);
        }
    }

    private void OnEnable()
    {
        if (botaoIniciar != null)
        {
            botaoIniciar.onClick.AddListener(OnCliqueBotaoIniciar);
        }

        if (botaoSair != null)
        {
            botaoSair.onClick.AddListener(OnCliqueBotaoSair);
        }
    }

    private void OnDisable()
    {
        if (botaoIniciar != null)
        {
            botaoIniciar.onClick.RemoveListener(OnCliqueBotaoIniciar);
        }

        if (botaoSair != null)
        {
            botaoSair.onClick.RemoveListener(OnCliqueBotaoSair);
        }
    }

    private void OnCliqueBotaoIniciar()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[MenuController] GameManager nao encontrado.");
            return;
        }

        GameManager.Instance.BotaoIniciar();
    }

    private void OnCliqueBotaoSair()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[MenuController] GameManager nao encontrado.");
            return;
        }

        GameManager.Instance.BotaoSair();
    }

    private Button EncontrarBotao(string nomeDoObjeto)
    {
        GameObject objeto = GameObject.Find(nomeDoObjeto);
        if (objeto == null)
        {
            Debug.LogWarning($"[MenuController] Botao nao encontrado: {nomeDoObjeto}");
            return null;
        }

        Button botao = objeto.GetComponent<Button>();
        if (botao == null)
        {
            Debug.LogWarning($"[MenuController] Componente Button ausente em: {nomeDoObjeto}");
        }

        return botao;
    }
}

