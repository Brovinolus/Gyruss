using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviour : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private ParticleSystem particleSystemPrefab;

    [Header ("Setting values")]
    [Range(0.5f, 3f)]
    [SerializeField] private float initialDiameter = 1f;
    [SerializeField] private float health = 10f;
    [SerializeField] private bool canDamage = true;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float radiusIncreaseRate = 0.1f;

    private AudioSource _audioSource;
    private PlayerMovement _player;
    private EnemySpawner _enemySpawner;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _centerPosition;
    private Quaternion _rotation;
    private float _targetDiameter;
    private float _currentDiameter;
    private float _initialAngle = 0f;
    private UI _uiDocument;
    
    private void Start()
    {
        if (Camera.main != null)
            _centerPosition = Utils.ScreenToWorld(Camera.main,new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        _player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        _audioSource = GameObject.Find("DamagedSound").GetComponent<AudioSource>();
        _uiDocument = GameObject.Find("UIDocument").GetComponent<UI>();

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _targetDiameter = _player.GetComponent<PlayerMovement>().DistanceFromCenter - 1f;

        _currentDiameter = initialDiameter;

       Vector2 initialOffset = transform.position - _centerPosition;
       _initialAngle = Mathf.Atan2(initialOffset.y, initialOffset.x) * Mathf.Rad2Deg;
    }

    private void FixedUpdate()
    {
        CircularMovement();
        LookInTheDirection();
    }
    
    private void CircularMovement()
    {
        if (_currentDiameter < _targetDiameter)
        {
            _currentDiameter += radiusIncreaseRate * Time.fixedDeltaTime;
            _currentDiameter = Mathf.Clamp(_currentDiameter, initialDiameter, _targetDiameter);
        }
        
        _initialAngle += rotationSpeed * Time.fixedDeltaTime;

        var x = _centerPosition.x + _currentDiameter * Mathf.Cos(_initialAngle * Mathf.Deg2Rad);
        var y = _centerPosition.y + _currentDiameter * Mathf.Sin(_initialAngle * Mathf.Deg2Rad);
        var newPosition = new Vector2(x, y);
        _rigidbody2D.MovePosition(newPosition);
    }

    private void LookInTheDirection()
    {
        var direction = new Vector2(Mathf.Cos(_initialAngle * Mathf.Deg2Rad + (Mathf.PI / 2f)), Mathf.Sin(_initialAngle * Mathf.Deg2Rad + (Mathf.PI / 2f)));

        _rotation = Quaternion.LookRotation(Vector3.forward, direction);
        
        _rigidbody2D.MoveRotation(_rotation);
    }
    
    public void Damage(float damage)
    {
        if (!canDamage) return;
        health -= damage;
        
        ScoreCalculation();
        PlayParticle();
        PlaySound();
        
        if (!(health <= 0)) return;
        _enemySpawner.EnemyDestroyed(gameObject);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PlayParticle();
    }

    private void PlayParticle()
    {
        var particleSystemInstance = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
            
        particleSystemInstance.Play();
    }
    
    private void PlaySound()
    {
        if (_audioSource.clip != null)
        {
            _audioSource.Play();
        }
    }

    private void ScoreCalculation()
    {
        var normalizedRadius =
            Mathf.Clamp01((_targetDiameter - _currentDiameter) / (_targetDiameter - initialDiameter) + 0.1f);
        Debug.Log(normalizedRadius);
        var newScore = Mathf.RoundToInt(normalizedRadius * 10f);
        Debug.Log(newScore);
        _uiDocument.IncreaseScore(newScore);
    }
}
