using UnityEngine;
using UnityEngine.InputSystem;

// Conecta as Input Actions (SumoControls) ao Bolinha.cs usando C# Events.
// Requer que o asset "SumoControls" tenha "Generate C# Class" marcado
// (selecione o asset no Project, veja no Inspector, marque a opção e clique em Apply).
[RequireComponent(typeof(Bolinha))]
public class BolinhaInput : MonoBehaviour
{
    private SumoControls _controls;
    private Bolinha _bolinha;
    private SumoControls.Jogador1Actions _mapaJ1;
    private SumoControls.Jogador2Actions _mapaJ2;

    void Awake()
    {
        _bolinha = GetComponent<Bolinha>();
        _controls = new SumoControls();
        _mapaJ1 = _controls.Jogador1;
        _mapaJ2 = _controls.Jogador2;
    }

    void OnEnable()
    {
        if (_bolinha.jogadorIndex == 0)
        {
            _mapaJ1.Enable();
            _mapaJ1.Mover.performed += OnMoverPerformed;
            _mapaJ1.Mover.canceled += OnMoverPerformed;
            _mapaJ1.Acao.performed += OnAcaoPerformed;
        }
        else
        {
            _mapaJ2.Enable();
            _mapaJ2.Mover.performed += OnMoverPerformed;
            _mapaJ2.Mover.canceled += OnMoverPerformed;
            _mapaJ2.Acao.performed += OnAcaoPerformed;
        }
    }

    void OnDisable()
    {
        if (_bolinha.jogadorIndex == 0)
        {
            _mapaJ1.Mover.performed -= OnMoverPerformed;
            _mapaJ1.Mover.canceled -= OnMoverPerformed;
            _mapaJ1.Acao.performed -= OnAcaoPerformed;
            _mapaJ1.Disable();
        }
        else
        {
            _mapaJ2.Mover.performed -= OnMoverPerformed;
            _mapaJ2.Mover.canceled -= OnMoverPerformed;
            _mapaJ2.Acao.performed -= OnAcaoPerformed;
            _mapaJ2.Disable();
        }
    }

    private void OnMoverPerformed(InputAction.CallbackContext ctx)
    {
        _bolinha.OnMover(ctx.ReadValue<Vector2>());
    }

    private void OnAcaoPerformed(InputAction.CallbackContext ctx)
    {
        _bolinha.OnAcao();
    }
}
