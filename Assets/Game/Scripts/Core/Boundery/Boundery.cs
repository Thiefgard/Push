using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundery : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    { 
        if(other != null)
        {
            if (other.tag == "Player")
            {
                Player.Instance.isInBoundery = false;
                Player.Instance.Die();
            }
            if (other.tag == "Enemy")
            {
                EnemyBehavior enemyB = other.gameObject.GetComponent<EnemyBehavior>();
                if (enemyB != null)
                {
                    enemyB.FallDownOutZone();
                }
            }
        }
       
    }

}
