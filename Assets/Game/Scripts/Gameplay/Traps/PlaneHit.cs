using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlaneHit : MonoBehaviour
{
    public bool _hit;
    bool _isForced;
    GameObject _spawnPoint;

    private void FixedUpdate()
    {
        if (_hit)
        {
            foreach(EnemyBehavior enemy in GetComponentsInChildren<EnemyBehavior>())
            {
                Rigidbody enemyrb = enemy.GetComponent<Rigidbody>();
                enemyrb.isKinematic = false;
                enemy.transform.parent = _spawnPoint.transform ;
                
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && collision.collider.GetComponent<EnemyBehavior>().isTrapped == false)
        {
            NavMeshAgent agent = collision.collider.GetComponent<EnemyBehavior>().agent;
            agent.isStopped = true;
            agent.enabled = false;
            _spawnPoint = collision.collider.GetComponentInParent<SpawnPoint>().gameObject;
            collision.collider.GetComponent<EnemyBehavior>().isTrapped = true;
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<Rigidbody>().isKinematic = true;
            enemy.transform.parent = this.transform;
            Vector3 closestToEnemy = collision.collider.ClosestPoint(this.transform.position);
            enemy.transform.position = closestToEnemy;
        }
    }
}