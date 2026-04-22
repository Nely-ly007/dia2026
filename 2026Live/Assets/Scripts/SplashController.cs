using System.Collections;
using UnityEngine;

public class SplashController : MonoBehaviour
{
    [SerializeField] private float tempoExibicao = 2f;

    private const string MenuSceneName = "MenuPrincipal";
    private Coroutine _retornoCoroutine;

    private void OnEnable()
    {
        _retornoCoroutine = StartCoroutine(RetornarAoMenuAposDelay());
    }

    private void OnDisable()
    {
        if (_retornoCoroutine != null)
        {
            StopCoroutine(_retornoCoroutine);
            _retornoCoroutine = null;
        }
    }

    private IEnumerator RetornarAoMenuAposDelay()
    {
        yield return new WaitForSeconds(tempoExibicao);
        _retornoCoroutine = null;

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[SplashController] GameManager nao encontrado.");
            yield break;
        }

        GameManager.Instance.CarregarCena(MenuSceneName);
    }
}

