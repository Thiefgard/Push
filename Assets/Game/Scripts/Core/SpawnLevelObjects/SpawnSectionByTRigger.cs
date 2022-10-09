using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSectionByTRigger : MonoBehaviour
{
    public bool _isSpawned = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isSpawned)
        {
            gameObject.GetComponentInParent<SpawnPoint>().SpawnEnemies();
        }
    }
}
