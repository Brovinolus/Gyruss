using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerShoot : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform firstProjectilePosition;
    [SerializeField] private Transform secondProjectilePosition;
    [SerializeField] private AudioSource audioSource;
    
    private Vector3 _centerPosition;

    private void Start()
    {
        if (Camera.main != null)
            _centerPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        _centerPosition.z = 0f;
    }

    private void OnEnable()
    {
        inputManager.OnStartShoot += Shoot;
    }

    private void OnDisable()
    {
        inputManager.OnStartShoot -= Shoot;
    }

    private void Shoot()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        
        SpawnProjectile(firstProjectilePosition);
        SpawnProjectile(secondProjectilePosition);
    }

    private void SpawnProjectile(Transform origin)
    {
        var position = origin.position;
        var projectileTransform = Instantiate(projectile, position, Quaternion.identity);
        var shootDirection = (_centerPosition - position).normalized;
        projectileTransform.GetComponent<Projectile>().Setup(shootDirection);
    }
}
