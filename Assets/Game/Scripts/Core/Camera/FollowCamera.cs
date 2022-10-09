using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 0.125f;
    GameObject player;

    
    
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
    }
}
