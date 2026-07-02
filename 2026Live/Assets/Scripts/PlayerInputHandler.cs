using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Bolinha bolinha;

    private SumoControls _controls;
    private bool _isJogador1;

    public void Inicializar(Bolinha b, bool jogador1)
    {
        bolinha = b;
        _isJogador1 = jogador1;
    }

    void OnEnable()
    {
        _controls = new SumoControls();

        if (_isJogador1)
        {
            _controls.Jogador1.Mover.performed += ctx => bolinha.OnMover(ctx.ReadValue<Vector2>());
            _controls.Jogador1.Mover.canceled += _ => bolinha.OnMover(Vector2.zero);
            _controls.Jogador1.Acao.performed += _ => bolinha.OnAcao();
            _controls.Jogador1.Enable();
        }
        else
        {
            _controls.Jogador2.Mover.performed += ctx => bolinha.OnMover(ctx.ReadValue<Vector2>());
            _controls.Jogador2.Mover.canceled += _ => bolinha.OnMover(Vector2.zero);
            _controls.Jogador2.Acao.performed += _ => bolinha.OnAcao();
            _controls.Jogador2.Enable();
        }
    }

    void OnDisable()
    {
        _controls?.Dispose();
    }
}