using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour
{
    [SerializeField] Trap[] _traps;
    Animator anim;
    bool _isActiveButton = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isActiveButton)
        {
            _isActiveButton = false;
            anim.SetBool("isClicked", true);
        }
    }
   

    void Activate()
    {
        foreach(Trap trap in _traps)
        {
            trap.TrapHit();
        }
    }

    void ReactivateButton()
    {
        _isActiveButton = true;
        anim.SetBool("isClicked", false);
    }


}
