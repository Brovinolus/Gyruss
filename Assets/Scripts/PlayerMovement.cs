using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    public float DistanceFromCenter => distanceFromCenter;
    
    [Header ("References")]
    [SerializeField] private InputManager inputManager;
    
    [Header ("Setting values")]
    [SerializeField] private float rotationSpeed = 100f;
    [Range(1f, 4f)]
    [SerializeField] private float distanceFromCenter = 4f;

    private Rigidbody2D _rigidbody2D;
    private Vector3 _centerPosition;
    private float _horizontalInput = 0f;
    private float _currentAngle = 0f;
    private bool _canMove = false;

    private void Start()
    {
        if (Camera.main != null)
            _centerPosition = Utils.ScreenToWorld(Camera.main,new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        inputManager.OnStartMovement += Move;
        inputManager.OnCancelMovement += Move;
    }

    private void OnDisable()
    {
        inputManager.OnStartMovement -= Move;
        inputManager.OnCancelMovement -= Move;
    }

    private void Move(float value, bool state)
    {
        _horizontalInput = value;
        _canMove = state;
    }

    private void Update()
    {
        LookAtTheCenter();

        if (_canMove)
        {
            UpdateInput(_horizontalInput);
        }
    }
    
    private void UpdateInput(float horizontalInput)
    {
        var rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        _currentAngle += rotationAmount;
    }

    private void FixedUpdate()
    {
        CircularMovement();
    }

    private void CircularMovement()
    {
        var x = _centerPosition.x + distanceFromCenter * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        var y = _centerPosition.y + distanceFromCenter * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);
        var newPosition = new Vector2(x, y);

        _rigidbody2D.MovePosition(newPosition);
    }

    private void LookAtTheCenter()
    {
        var lookAtDirection = _centerPosition - transform.position;
        var angleToCenter = Mathf.Atan2(lookAtDirection.y, lookAtDirection.x) * Mathf.Rad2Deg;
        _rigidbody2D.MoveRotation(Quaternion.AngleAxis(angleToCenter - 90f, Vector3.forward));
    }
}
