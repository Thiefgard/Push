using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeizerFollow : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] routes;
    float timeToDestroy;
    private int routeToGo;
    private float tParam;
    private Vector2 coinPos;
    [SerializeField]private float speedModifire;
    private bool coroutineAllowed;

    private void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifire = 0.9f;
        coroutineAllowed = true;
        timeToDestroy = speedModifire + 0.2f;
    }

    private void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
            StartCoroutine(DestroyGO(timeToDestroy));
        }
        if (transform.position == routes[routes.Length - 1].transform.position) Destroy(gameObject);
    }

    IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNum].GetChild(0).position; 
        Vector2 p1 = routes[routeNum].GetChild(1).position; 
        Vector2 p2 = routes[routeNum].GetChild(2).position; 
        Vector2 p3 = routes[routeNum].GetChild(3).position; 

        while( tParam < 1)
        {
            tParam += Time.deltaTime * speedModifire;

            coinPos = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            transform.position = coinPos;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;
        if (routeToGo > routes.Length - 1) yield  break ;
        coroutineAllowed = true;
    }
    IEnumerator DestroyGO(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
    }
}
