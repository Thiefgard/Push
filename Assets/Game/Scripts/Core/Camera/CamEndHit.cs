using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEndHit : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void EndHit()
    {
        anim.SetBool("camHit", false);
    }
}
