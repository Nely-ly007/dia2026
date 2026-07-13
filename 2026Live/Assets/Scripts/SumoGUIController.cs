using UnityEngine;
using UnityEngine.UIElements;

public class SumoGUIController : MonoBehaviour
{
    private Label _roundsJ1, _roundsJ2;
    private Label _moedasJ1, _moedasJ2;
    private ProgressBar _cooldownJ1, _cooldownJ2;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _roundsJ1   = root.Q<Label>("rounds-j1");
        _roundsJ2   = root.Q<Label>("rounds-j2");
        _moedasJ1   = root.Q<Label>("moedas-j1");
        _moedasJ2   = root.Q<Label>("moedas-j2");
        _cooldownJ1 = root.Q<ProgressBar>("cooldown-j1");
        _cooldownJ2 = root.Q<ProgressBar>("cooldown-j2");

        // Inscreve nos eventos Observer
        Bolinha.OnCooldownAtualizado    += AtualizarCooldown;
        Bolinha.OnMoedasAtualizadas     += AtualizarMoedas;
        RoundManager.OnPlacarAtualizado += AtualizarPlacar;
    }

    void OnDisable()
    {
        Bolinha.OnCooldownAtualizado    -= AtualizarCooldown;
        Bolinha.OnMoedasAtualizadas     -= AtualizarMoedas;
        RoundManager.OnPlacarAtualizado -= AtualizarPlacar;
    }

    void AtualizarCooldown(int jogador, float normalizado)
    {
        // 100 = pronto para usar, 0 = em cooldown
        float valor = (1f - normalizado) * 100f;
        if (jogador == 0) _cooldownJ1.value = valor;
        else              _cooldownJ2.value = valor;
    }

    void AtualizarMoedas(int jogador, int total)
    {
        if (jogador == 0) _moedasJ1.text = $"🪙 {total}";
        else              _moedasJ2.text = $"🪙 {total}";
    }

    void AtualizarPlacar(int vJ1, int vJ2)
    {
        _roundsJ1.text = new string('●', vJ1) + new string('○', 2 - vJ1);
        _roundsJ2.text = new string('●', vJ2) + new string('○', 2 - vJ2);
    }
}