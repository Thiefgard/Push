using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlyCoin : MonoBehaviour
{
    [SerializeField] GameObject coinEnd;
    [SerializeField] Vector3 finPos;
    // Start is called before the first frame update
    void Start()
    {
        finPos = GameObject.Find("coinTransform").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = finPos - transform.position;
        if(transform.position != finPos)
        {
            transform.position += dir.normalized * 3 * Time.deltaTime;
        }
    }
    
}
