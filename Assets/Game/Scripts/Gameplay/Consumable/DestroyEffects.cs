using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffects : MonoBehaviour
{
    [SerializeField] float _lifeTime = 1f;
    private Material mat;

    private void Start()
    {
        //mat = GetComponentInChildren<Material>();
       
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        
    }
}
