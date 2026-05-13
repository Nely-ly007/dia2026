using System;

/// <summary>
/// PlayerOM (PlayerObserverManager)
/// Classe estática que funciona como Static Event Manager.
/// Contém o canal de comunicação relativo às moedas do jogador,
/// seguindo o padrão Observer para desacoplar o Player da GUI.
/// </summary>
public static class PlayerOM
{
    // -------------------------------------------------------
    // CANAL DE MOEDAS
    // -------------------------------------------------------

    /// <summary>
    /// Evento disparado sempre que o jogador coleta uma moeda.
    /// Parâmetro: quantidade TOTAL de moedas coletadas até o momento.
    /// </summary>
    public static event Action<int> OnCoinCollected;

    /// <summary>
    /// Dispara o evento de coleta de moeda para todos os inscritos.
    /// Deve ser chamado pelo PlayerController ao encostar em uma moeda.
    /// </summary>
    /// <param name="totalCoins">Total acumulado de moedas do jogador.</param>
    public static void NotifyCoinCollected(int totalCoins)
    {
        OnCoinCollected?.Invoke(totalCoins);
    }

    /// <summary>
    /// Reseta o evento (remove todos os inscritos).
    /// Útil ao reiniciar a cena para evitar múltiplas inscrições.
    /// </summary>
    public static void ResetChannel()
    {
        OnCoinCollected = null;
    }
}
