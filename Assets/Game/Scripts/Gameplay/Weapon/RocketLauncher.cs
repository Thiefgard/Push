using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] Transform _bullet_ins;
    [SerializeField] Bullet _bulletPref;
    [SerializeField] float _speed = 2f;
    [SerializeField] float _angle = 15f;

    public bool _isSuper = false;

    Animator _anim;


    private void Start()
    {
        _anim = GetComponent<Animator>();
        Debug.DrawLine(transform.position, Vector3.forward, Color.red, 5f);
    }

    void HitEffect()
    {
        Vector3 direction = Player.Instance.transform.forward * _speed;
        
        Bullet bullet = Instantiate(_bulletPref, Level.Instance.transform);
        bullet.gameObject.transform.position = _bullet_ins.position;
        bullet.gameObject.transform.rotation = _bullet_ins.rotation;
        bullet.Init(direction);
        
    }

    public void EndHit()
    {
        WeaponHolder.Instace._canHit = true;
        WeaponHolder.Instace.PlayerHitAnim(false);
        _anim.SetBool("callAttack", false);
    }

    void SuperHit()
    {
        Bullet[] bullets = new Bullet[3];
        Vector3[] directions = new Vector3[3];
        Vector3 direction = Player.Instance.transform.forward * _speed;

        directions[0] = Quaternion.Euler(0, _angle, 0) * direction;
        directions[1] = direction;
        directions[2] = Quaternion.Euler(0, -_angle, 0) * direction;
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(_bulletPref, Level.Instance.transform);
            bullets[i].transform.position = _bullet_ins.position;
            if (i == 0) bullets[i].gameObject.transform.rotation = Quaternion.Euler(0, _angle, 0) * _bullet_ins.rotation;
            else if(i == 2) bullets[i].gameObject.transform.rotation = Quaternion.Euler(0, -_angle, 0) * _bullet_ins.rotation;
            else bullets[i].transform.rotation = _bullet_ins.rotation;
            bullets[i].Init(directions[i]);
            
        }
        
    }
    private void EndSuperHit()
    {
        WeaponHolder.Instace.PlayerHitAnim(false);
        _anim.SetBool("superAttack", false);
        WeaponHolder.Instace._canHit = true;
        
    }

}
