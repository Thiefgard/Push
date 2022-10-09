using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _force = 500f;
    [SerializeField] float _lifeTime = 7f;

    Rigidbody _rb;
    Vector3 _direction;
    bool _isActive = false;
    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        if (_isActive)
        {

            _rb.transform.position += _direction * _speed * Time.fixedDeltaTime;
            float timer = 0;
            timer += Time.fixedDeltaTime;
            if (timer > _lifeTime) Destroy(gameObject);
        }
    }

    public void Init(Vector3 dir)
    {
        _rb.velocity = Vector3.zero;
        _isActive = true;
        _direction = dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("hitted");
            Rigidbody playerRb = Player.Instance.GetComponent<Rigidbody>();
            Vector3 direction = playerRb.transform.position - transform.position;
            print(direction);
            direction.y = 0;
            playerRb.AddForce(direction * _force, ForceMode.Impulse);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }
}
