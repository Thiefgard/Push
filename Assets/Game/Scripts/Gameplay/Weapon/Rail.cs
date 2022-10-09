using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Rail : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    

    private void EndHit()
    {
        anim.SetBool("callAttack", false);
        WeaponHolder.Instace._canHit = true;
    }

    private void EndSuperHit()
    {
        anim.SetBool("superAttack", false);
        WeaponHolder.Instace._canHit = true;
    }
}
