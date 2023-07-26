using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public delegate void StartMovementEvent(float value, bool state);
    public event StartMovementEvent OnStartMovement;
    public delegate void StartShootEvent();
    public event StartShootEvent OnStartShoot;
    
    public delegate void CancelMovementEvent(float value, bool state);
    public event CancelMovementEvent OnCancelMovement;
    public delegate void CancelShootEvent();
    public event CancelShootEvent OnCancelShoot;

    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    public void OnEnable()
    {
        _inputActions.Player.Enable();
        
        _inputActions.Player.Movement.performed += PerformMovement;
        _inputActions.Player.Shoot.performed += PerformShoot;
        _inputActions.Player.Movement.canceled += CancelMovement;
        _inputActions.Player.Shoot.canceled += CancelShoot;
    }

    public void OnDisable()
    {
        _inputActions.Player.Movement.performed -= PerformMovement;
        _inputActions.Player.Shoot.performed -= PerformShoot;
        _inputActions.Player.Movement.canceled -= CancelMovement;
        _inputActions.Player.Shoot.canceled -= CancelShoot;

        _inputActions.Player.Disable();
    }

    private void PerformMovement(InputAction.CallbackContext ctx)
    {
        OnStartMovement?.Invoke(ctx.ReadValue<float>(), true);
    }
    
    private void PerformShoot(InputAction.CallbackContext ctx)
    {
        OnStartShoot?.Invoke();
    }
    
    private void CancelMovement(InputAction.CallbackContext ctx)
    {
        OnCancelMovement?.Invoke(0, false);
    }
    
    private void CancelShoot(InputAction.CallbackContext ctx)
    {
        OnCancelShoot?.Invoke();
    }
}
