using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _radius = 1f;
    [SerializeField] float _force = 290f;
    [SerializeField]Transform _exposionPoint;
    [SerializeField] GameObject _exposionPart;
    [SerializeField] float _lifeTime = 7f;
    Rigidbody _rb;
    Vector3 _direction;
    Vector3 closestPoint;

    bool _isActive = false;
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
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Collider[] colliders = Physics.OverlapSphere(_exposionPoint.position, _radius);
            
            foreach (Collider nearbyObject in colliders)
            {
                //if (nearbyObject.CompareTag("Ground"))
                //{
                //    closestPoint = collision.collider.ClosestPoint(nearbyObject.transform.position);
                //}
                if (nearbyObject.tag == "Enemy" && nearbyObject.GetComponent<Rigidbody>() != null)
                {
                    EnemyBehavior eb = nearbyObject.GetComponent<EnemyBehavior>();
                    Vector3 direction = (nearbyObject.transform.position - transform.position).normalized;
                    direction.y = 0;
                    if(Vector3.Distance(nearbyObject.transform.position, Player.Instance.transform.position) > 4f)
                    {
                        nearbyObject.GetComponent<Rigidbody>().AddForce(direction * _force, ForceMode.Impulse);
                    }
                    else
                    {
                        direction = (nearbyObject.transform.position - Player.Instance.transform.position).normalized;
                        direction.y = 0;

                        nearbyObject.GetComponent<Rigidbody>().AddForce(direction * _force, ForceMode.Impulse);
                    }
                    StartCoroutine(BulletHit());
                    eb.GetHit();
                }
            }
            Destroy(gameObject);
        }
    }
    IEnumerator BulletHit()
    {
        GameObject explosion = Instantiate(_exposionPart, Level.Instance.transform);

        explosion.transform.position = transform.position;
        explosion.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.7f);
        Destroy(explosion);
        yield break;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(_exposionPoint.position, _radius);
    //}
}
