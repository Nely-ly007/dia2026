using System;

/// <summary>
/// PlayerOM (PlayerObserverManager)
/// Classe estática que funciona como Static Event Manager.
/// Contém o canal de comunicação relativo às moedas do jogador,
/// seguindo o padrão Observer para desacoplar o Player da GUI.
/// </summary>
public static class PlayerOM
{
    
    public static event Action<int> OnCoinCollected;

   
    /// <param name="totalCoins">Total acumulado de moedas do jogador.</param>
    public static void NotifyCoinCollected(int totalCoins)
    {
        OnCoinCollected?.Invoke(totalCoins);
    }
    
    public static void ResetChannel()
    {
        OnCoinCollected = null;
    }
}
