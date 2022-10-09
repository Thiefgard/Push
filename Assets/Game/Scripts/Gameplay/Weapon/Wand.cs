using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] Transform _explodePoint;
    [SerializeField] float _baseRadius = 6f;
    [SerializeField] float _superRaius = 15f;
    [SerializeField] float _baseForce = 190f;
    [SerializeField] float _superForce = 360f;
    [SerializeField] GameObject wandHit;

    public bool _isSuper;

    float _force;
    float _radius;
    Animator _anim;



    private void Start()
    {
        _force = _superForce;
        _radius = _superRaius;
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        //if (_isSuper)
        //{
        //    _force = _superForce;
        //    _radius = _superRaius;
        //}
        //else
        //{
        //    _force = _baseForce;
        //    _radius = _baseRadius;
        //}
    }

    void HitEffect()
    {
        if (_isSuper)
        {
            _force = _superForce;
            _radius = _superRaius;
            StartCoroutine(ParticleHit(0.2f));
        }
        else
        {
            _force = _baseForce;
            _radius = _baseRadius;
            StartCoroutine(ParticleHit(0.1f));
        }
        Collider[] colliders = Physics.OverlapSphere(_explodePoint.position, _radius);
        
        foreach(Collider nearbyObject in colliders)
        {
            
            if(nearbyObject.tag == "Enemy" && nearbyObject.GetComponent<Rigidbody>() != null)
            {
                EnemyBehavior eb = nearbyObject.GetComponent<EnemyBehavior>();
                Vector3 direction = (nearbyObject.transform.position - _explodePoint.position).normalized;
                direction.y = 0;
                nearbyObject.GetComponent<Rigidbody>().AddForce(direction 
                    * _force, ForceMode.Impulse);
                eb.GetHit();
            }
        }
    }

    IEnumerator ParticleHit(float scale)
    {
        GameObject explosion = Instantiate(wandHit, Level.Instance.transform);
        
        explosion.transform.position = _explodePoint.position + new Vector3 (0,0.5f,0);
        explosion.transform.localScale = new Vector3(scale, scale, scale);
        yield return new WaitForSeconds(0.7f);
        Destroy(explosion);
        yield break;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(_explodePoint.position, _radius);
    //}

    void EndHit()
    {
        WeaponHolder.Instace._canHit = true;
        _anim.SetBool("callAttack", false);
        WeaponHolder.Instace.PlayerHitAnim(false);
    }

    private void EndSuperHit()
    {
        _anim.SetBool("superAttack", false);
        WeaponHolder.Instace._canHit = true;
        _isSuper = false;
        WeaponHolder.Instace.PlayerHitAnim(false);
    }

}
