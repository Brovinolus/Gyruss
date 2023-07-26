using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header ("Setting values")]
    [SerializeField] private float speed = 600f;
    [SerializeField] private float timeToDestroy = 2f;
    [SerializeField] private float damage = 10f;
    public void Setup(Vector3 shootDirection)
    {
        LookAtTheCenter(shootDirection);
        
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(shootDirection * speed, ForceMode2D.Force);
        
        Destroy(gameObject, timeToDestroy);
    }
    
    private void LookAtTheCenter(Vector3 shootDirection)
    {
        var angleToCenter = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angleToCenter - 90f, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<EnemyBehaviour>();

        if (target != null)
        {
            target.Damage(damage);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag($"ForProjectile"))
        {
            Destroy(gameObject);
        }
    }
}
