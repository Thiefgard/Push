using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    enum TrapType
    {
        box,
        plane,
        rock
    }
    [SerializeField] TrapType _type;
    [SerializeField] GameObject _platform;
    Animator _anim;
    public bool _activate;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = 0.3f;
        if(_type == TrapType.plane)
        {
            _anim.speed = 0.7f;
        }
    }

    public void TrapHit()
    {
        if (!_activate)
        {
            _activate = true;
            _anim.SetBool("isActivate", true);
        }
    }

    //anim effect
    void EndTrapHit()
    {
        _activate = false;
        _anim.SetBool("isActivate", false);
    }

    void PlaneHitEffect()
    {
        GetComponentInChildren<PlaneHit>()._hit = true;
    }
}
